using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Niche.Graphs;
using Niche.Shared;

namespace Niche.VSGraph
{
    /// <summary>
    /// Group a set of assemblies to apply a common style
    /// </summary>
    public class AssemblyGroup
    {
        /// <summary>
        /// Gets a sequence of the assemblies included in this group
        /// </summary>
        public IEnumerable<AssemblyInfo> Assemblies
        {
            get { return mAssemblies; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether these assemblies are shown
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets the style to use when displaying these assemblies
        /// </summary>
        public NodeStyle Style { get; private set; }

        public AssemblyGroup(string wildcards)
        {
            mWildcards
                = wildcards.Split(';')
                    .Select(s => s.RegexFromWildcard())
                    .Select(r => new Regex(r, RegexOptions.IgnoreCase))
                    .ToList();
            Style = new NodeStyle
                {
                    Shape = NodeShape.MRecord,
                    FontSize = 11
                };

            Visible = true;
        }

        public bool IncludesAssembly(AssemblyInfo assemblyInfo)
        {
            return mWildcards.Any(r => r.IsMatch(assemblyInfo.Name));
        }

        public void AddAssembly(AssemblyInfo assemblyInfo)
        {
            mAssemblies.Add(assemblyInfo);
        }

        public void AddAssemblies(IEnumerable<AssemblyInfo> assemblies)
        {
            foreach (var a in assemblies)
            {
                AddAssembly(a);
            }
        }

        public void CreateNodes(Dictionary<string, Node> assemblyNodes)
        {
            foreach (var a in mAssemblies)
            {
                assemblyNodes[a.Key] = a.CreateAssemblyNode(Style);
            }
        }

        public Node CreateNode(AssemblyInfo assemblyInfo)
        {
            return assemblyInfo.CreateAssemblyNode(Style);
        }

        private readonly List<Regex> mWildcards;

        private readonly List<AssemblyInfo> mAssemblies = new List<AssemblyInfo>();
    }
}
