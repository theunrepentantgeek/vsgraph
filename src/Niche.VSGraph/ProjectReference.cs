using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Niche.Graphs;

namespace Niche.VSGraph
{
    [DebuggerDisplay("Project: {Name}; Path={Path}.")]
    public class ProjectReference
    {
        public string Name
        {
            get { return mName; }
        }

        public string Path
        {
            get { return mPath; }
        }

        public ProjectReference(string name, string path)
        {
            mName = name;
            mPath = path;
        }

        public Node CreateOrphanProjectNode(NodeStyle style)
        {
            var graphNode = new Node(Name, Name, style);
            return graphNode;
        }

        public bool Equals(ProjectReference other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.mName, mName) && Equals(other.mPath, mPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(ProjectReference)) return false;
            return Equals((ProjectReference)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((mName != null ? mName.GetHashCode() : 0) * 397) ^ (mPath != null ? mPath.GetHashCode() : 0);
            }
        }

        public static bool operator ==(ProjectReference left, ProjectReference right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ProjectReference left, ProjectReference right)
        {
            return !Equals(left, right);
        }

        private readonly string mName;

        private readonly string mPath;
    }
}
