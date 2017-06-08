using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Niche.Graphs
{
    /// <summary>
    /// Represents styling for an entire graph
    /// </summary>
    public class GraphStyle
    {
        /// <summary>
        /// Gets or sets a value indicating whether to merge edges
        /// </summary>
        public bool MergeEdges { get; set; }


        /// <summary>
        /// Gets or sets the orientation to use for the graph.
        /// </summary>
        public Orientation Orientation { get; set; }

        /// <summary>
        /// Gets or sets the resolution to use for the graph.
        /// </summary>
        public int DotsPerInch { get; set; }
    }

    public enum Orientation
    {
        TopDown,
        LeftRight,
        BottomUp,
        RightLeft
    }

}
