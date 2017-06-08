using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Niche.Graphs;
using Niche.Shared;

namespace Niche.VSGraph
{
    [DebuggerDisplay("Project {Name}")]
    public class ProjectInfo
    {
        public string Name { get; private set; }

        public string ProjectFile
        {
            get { return mProjectFile; }
        }

        public IEnumerable<ProjectReference> ProjectReferences
        {
            get { return mProjectReferences; }
        }

        public List<AssemblyInfo> AssemblyReferences
        {
            get { return mAssemblyReferences; }
        }

        public ProjectInfo(string projectFile)
        {
            mProjectFile = projectFile;
            LoadDetails();
        }

        public bool NameContains(string partial)
        {
            return Name.IndexOf(partial, StringComparison.OrdinalIgnoreCase) != -1;
        }

        public Node Node
        {
            get { return mNode; }
        }

        public Node CreateProjectNode(NodeStyle style)
        {
            //var label
            //    = string.Format(
            //        "<<table>{0}</table>>",
            //        mDetails.OrderBy(d => d.Heading)
            //            .Select(d => d.AsHtmlTableRows())
            //            .JoinWith(string.Empty));

            var b = new StringBuilder();

            b.Append("{ ");
            b.Append(Name);

            //foreach (var d in mDetails)
            //{
            //    b.Append(" | ");
            //    b.Append(d.AsLabel());
            //}

            b.Append(" }");

            mNode = new Node(Name, b.ToString(), style);
            return mNode;
        }

        private void LoadDetails()
        {
            var details = XDocument.Load(mProjectFile);

            var ns = details.Root.Name.Namespace;
            Name = (string)details.Descendants(ns + "AssemblyName").SingleOrDefault();

            foreach (var g in details.Descendants(ns + "PropertyGroup"))
            {
                if (g.HasAttributes)
                {
                    LoadConfiguration(g);
                }
                else
                {
                    LoadProjectDetails(g);
                }
            }


            var projectPath = Path.GetDirectoryName(mProjectFile);

            foreach (var p in details.Descendants(ns + "ProjectReference"))
            {
                var path = (string)p.Attribute("Include");
                var name = (string)p.Element(ns + "Name");
                var fullPath = Path.GetFullPath(Path.Combine(projectPath, path));

                var projectRef = new ProjectReference(name, fullPath);
                mProjectReferences.Add(projectRef);
            }

            foreach (var assembly in details.Descendants(ns + "Reference"))
            {
                var s = (string)assembly.Attribute("Include");
                if (s == null)
                {
                    continue;
                }

                var name = s.Split(',')[0];

                //if (name.StartsWith("System"))
                //{
                //    continue;
                //}

                var hintPath = (string)assembly.Element(ns + "HintPath");
                if (hintPath != null)
                {
                    hintPath = Path.Combine(projectPath, hintPath);
                    mAssemblyReferences.Add(new AssemblyInfo(name, hintPath));
                }
                else
                {
                    mAssemblyReferences.Add(new AssemblyInfo(name));
                }
            }
        }

        private void LoadProjectDetails(XElement propertyGroup)
        {
            var ns = propertyGroup.GetDefaultNamespace();

            var properties = new PropertyBag();
            properties.Add("Framework " + (string)propertyGroup.Element(ns + "TargetFrameworkVersion"));
            properties.Add("Output: " + (string)propertyGroup.Element(ns + "OutputType"));

            mDetails.Add(properties);
        }

        private void LoadConfiguration(XElement propertyGroup)
        {
            var ns = propertyGroup.GetDefaultNamespace();

            var name = (string)propertyGroup.Attribute("Condition");
            name = name.Substring(name.IndexOf("==") + 2).Trim().WithoutDelimiters("'", "'");

            var properties = new PropertyBag(name);
            properties.Add("Debug Type " + (string)propertyGroup.Element(ns + "DebugType"));

            var optimized = (bool?)propertyGroup.Element(ns + "Optimize");
            if (optimized.HasValue)
            {
                if (optimized.Value)
                {
                    properties.Add("Optimized");
                }
                else
                {
                    properties.Add("Not Optimized");
                }
            }

            properties.Add((string)propertyGroup.Element(ns + "DefineConstants"));

            mDetails.Add(properties);
        }

        private readonly string mProjectFile;

        private readonly List<ProjectReference> mProjectReferences = new List<ProjectReference>();

        private List<AssemblyInfo> mAssemblyReferences = new List<AssemblyInfo>();

        private Node mNode;



        private readonly List<PropertyBag> mDetails = new List<PropertyBag>();
    }
}