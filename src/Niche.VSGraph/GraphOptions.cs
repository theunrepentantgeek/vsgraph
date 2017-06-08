using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Niche.Graphs;

namespace Niche.VSGraph
{
    public class GraphOptions
    {
        [Description("Display help.")]
        public bool Help { get; set; }

        [Description("Output image file.")]
        public string Output { get; set; }

        [Description("Font to use for labels.")]
        public string Font
        {
            get
            {
                return mFont;
            }

            set
            {
                mFont = value;
                ProjectNodeStyle.Font = mFont;
                OrphanProjectNodeStyle.Font = mFont;
                GacAssemblyNodeStyle.Font = mFont;
            }
        }

        [Description("Orientation for graph layout.")]
        public Orientation Orientation
        {
            get { return GraphStyle.Orientation; }
            set { GraphStyle.Orientation = value; }
        }

        [Description("Merge dependency edges.")]
        public bool MergeEdges
        {
            get { return GraphStyle.MergeEdges; }
            set { GraphStyle.MergeEdges = value; }
        }

        [Description("Dots per inch for rendering.")]
        public int DotsPerInch
        {
            get { return GraphStyle.DotsPerInch; }
            set { GraphStyle.DotsPerInch = value; }
        }

        [Description("Save Dot script to this file.")]
        public string DotFile { get; set; }

        [Description("Simplify the graph by removing transitive dependencies.")]
        public bool RemoveTransitiveDependencies { get; private set; }

        internal AssemblyGroup CurrentAssemblyGroup { get; set; }

        internal NodeStyle ProjectNodeStyle { get; private set; }

        internal NodeStyle OrphanProjectNodeStyle { get; private set; }

        internal NodeStyle GacAssemblyNodeStyle { get; private set; }

        internal EdgeStyle ProjectReferenceStyle { get; private set; }

        internal EdgeStyle AssemblyReferenceStyle { get; private set; }

        internal GraphStyle GraphStyle { get; private set; }

        internal IList<AssemblyGroup> AssemblyGroups { get; private set; }

        public GraphOptions()
        {
            AssemblyGroups = new List<AssemblyGroup>();

            ProjectNodeStyle
                = new NodeStyle
                      {
                          Shape = NodeShape.Record,
                          FontSize = 14
                      };

            OrphanProjectNodeStyle
                = new NodeStyle
                      {
                          Shape = NodeShape.Record,
                          FontSize = 14
                      };

            GacAssemblyNodeStyle
                = new NodeStyle
                      {
                          Shape = NodeShape.MRecord,
                          FontSize = 11,
                          FontColor = Color.DarkGoldenrod,
                          Color = Color.Goldenrod
                      };

            ProjectReferenceStyle
                = new EdgeStyle
                      {
                          ArrowHead = ArrowShape.Normal,
                          Color = Color.Gray,
                          PenWidth = 2.0
                      };

            AssemblyReferenceStyle
                = new EdgeStyle
                      {
                          ArrowHead = ArrowShape.Vee,
                          Color = Color.Gray
                      };

            GraphStyle
                = new GraphStyle()
                      {
                          Orientation = Orientation.TopDown,
                          DotsPerInch = 96
                      };

            Font = "Verdana";
        }

        [Description("Specify assemblies to hide by wildcard")]
        public void HideAssemblies(string wildcards)
        {
            var group
                = new AssemblyGroup(wildcards)
                      {
                          Visible = false
                      };
            AssemblyGroups.Add(group);
        }

        private string mFont;
    }
}
