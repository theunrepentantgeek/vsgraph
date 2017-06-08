using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Niche.Graphs;
using Niche.Shared;

namespace Niche.VSGraph
{
    public class VisualStudioGraph
    {
        private readonly ILogger mLogger;
        public GraphOptions Options { get; private set; }

        public VisualStudioGraph(ILogger logger)
        {
            mLogger = logger;
            Options = new GraphOptions();
            DefaultAssemblyGroup = new AssemblyGroup("*");
        }

        /// <summary>
        /// Read in a solution file, identifying the projects referenced and loading those as well.
        /// </summary>
        /// <param name="solutionFile">Filename of the solution file to load</param>
        public void LoadSolution(string solutionFile)
        {
            mLogger.Action("Loading solution {0}", Path.GetFileName(solutionFile));

            var solutionInfo = new SolutionInfo(solutionFile);
            mSolutions.Add(solutionInfo);

            foreach (var r in solutionInfo.Projects)
            {
                LoadProject(r);
            }

            if (string.IsNullOrEmpty(Options.Output))
            {
                Options.Output = Path.ChangeExtension(solutionFile, ".png");
            }
        }

        public void GenerateGraphImage()
        {
            mAssemblyGroups.AddRange(Options.AssemblyGroups);
            mAssemblyGroups.Add(DefaultAssemblyGroup);

            Graph graph = GenerateGraph();

            mLogger.Action("Rendering Image");

            var renderer = new DotRenderer(graph);
            var image = renderer.RenderImage();

            mLogger.Detail("Saving image to {0}", Options.Output);
            image.Save(Options.Output);

            if (!string.IsNullOrWhiteSpace(Options.DotFile))
            {
                mLogger.Detail("Saving dot script to {0}", Options.DotFile);
                File.WriteAllText(Options.DotFile, renderer.DotText);
            }
        }

        private Graph GenerateGraph()
        {
            mLogger.Action("Generating dependency graph");

            var assemblyNodes = CreateAssemblyNodes().ToList();
            var projectNodes = CreateProjectNodes().ToList();

            var projectReferences = CreateProjectReferenceEdges().ToList();
            var assemblyReferences = CreateAssemblyReferenceEdges().ToList();

            var nodes = projectNodes.Union(assemblyNodes).ToList();
            var edges = projectReferences.Union(assemblyReferences).ToList();

            if (Options.RemoveTransitiveDependencies)
            {
                RemoveTransitiveDependencies(edges);
            }

            return new Graph(nodes, edges, new List<Graph>(), Options.GraphStyle);
        }

        private void RemoveTransitiveDependencies(List<Edge> edges)
        {
            var allEdges = edges.ToList();

            var edgesToRemove
                = (from e in allEdges
                   from s in allEdges
                   where e.Start == s.Start
                   from f in allEdges
                   where s.Finish == f.Start
                   where e.Finish == f.Finish
                   select e).ToList();

            edges.RemoveAll(e => edgesToRemove.Contains(e));
        }

        private ProjectInfo FindProject(ProjectReference project)
        {
            return mProjects.SingleOrDefault(p => p.ProjectFile == project.Path);
        }

        private IEnumerable<Node> CreateAssemblyNodes()
        {
            foreach (var a in GetAssemblies())
            {
                var group = mAssemblyGroups.First(g => g.IncludesAssembly(a));
                if (group.Visible)
                {
                    yield return group.CreateNode(a);
                }
            }
        }

        private IEnumerable<AssemblyInfo> GetAssemblies()
        {
            if (mAssemblies == null)
            {
                mAssemblies = mProjects
                   .SelectMany(p => p.AssemblyReferences)
                   .Distinct();
            }

            return mAssemblies;
        }

        private IEnumerable<Node> CreateProjectNodes()
        {
            return mProjects.Select(p => p.CreateProjectNode(Options.ProjectNodeStyle));
        }

        private IEnumerable<Edge> CreateAssemblyReferenceEdges()
        {
            /*
             * We skip references to assemblies not in the list -- they're not in the list
             * because they are not visible.
             */
            var assemblies = GetAssemblies().ToList();
            return from project in mProjects
                   from reference in project.AssemblyReferences
                   let destination = assemblies.SingleOrDefault(a => a.Key == reference.Key)
                   where destination != null && destination.Node != null
                   select new Edge(project.Node, destination.Node, Options.AssemblyReferenceStyle);
        }

        private IEnumerable<Edge> CreateProjectReferenceEdges()
        {
            // If we can't find a project from it's reference, we skip it because it's likely
            // a hidden project
            return from origin in mProjects
                   from reference in origin.ProjectReferences
                   let destination = FindProject(reference)
                   where destination != null
                   select new Edge(origin.Node, destination.Node, Options.ProjectReferenceStyle);
        }

        private void LoadProject(ProjectReference projectReference)
        {
            mLogger.Detail("Loading project {0}", Path.GetFileName(projectReference.Path));

            var projectInfo = new ProjectInfo(projectReference.Path);
            mProjects.Add(projectInfo);
        }

        private AssemblyGroup DefaultAssemblyGroup { get; set; }

        private readonly List<SolutionInfo> mSolutions = new List<SolutionInfo>();

        private readonly List<ProjectInfo> mProjects = new List<ProjectInfo>();

        private readonly List<AssemblyGroup> mAssemblyGroups = new List<AssemblyGroup>();

        private IEnumerable<AssemblyInfo> mAssemblies;
    }
}
