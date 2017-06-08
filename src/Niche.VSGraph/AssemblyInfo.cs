using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Niche.Graphs;

namespace Niche.VSGraph
{
    [DebuggerDisplay("Assembly {Name}")]
    public class AssemblyInfo : IEquatable<AssemblyInfo>
    {
        public string Name
        {
            get { return mName; }
        }

        public string AssemblyPath
        {
            get { return mAssemblyPath; }
        }

        public bool IsGacAssembly
        {
            get
            {
                return string.IsNullOrEmpty(AssemblyPath);
            }
        }

        public string Key
        {
            get
            {
                if (IsGacAssembly)
                {
                    return Name;
                }

                return AssemblyPath;
            }
        }

        public Node Node
        {
            get { return mNode; }
        }

        public AssemblyInfo(string name)
        {
            mName = name;
        }

        public AssemblyInfo(string name, string path)
            : this(name)
        {
            mAssemblyPath = Path.GetFullPath(path);
        }

        public Node CreateAssemblyNode(NodeStyle assemblyNodeStyle)
        {
            mNode = new Node(Key, Name, assemblyNodeStyle);
            return mNode;
        }

        public bool Equals(AssemblyInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.mName, mName) && Equals(other.mAssemblyPath, mAssemblyPath);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AssemblyInfo)) return false;
            return Equals((AssemblyInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((mName != null ? mName.GetHashCode() : 0)*397) ^ (mAssemblyPath != null ? mAssemblyPath.GetHashCode() : 0);
            }
        }

        private readonly string mName;

        private readonly string mAssemblyPath;
        private Node mNode;
    }
}
