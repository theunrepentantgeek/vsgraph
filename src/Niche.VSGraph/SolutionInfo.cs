using System.Collections.Generic;
using System.IO;
using Niche.Shared;

namespace Niche.VSGraph
{
    public class SolutionInfo
    {
        public string SolutionFile
        {
            get { return mSolutionFile; }
        }

        public List<ProjectReference> Projects
        {
            get { return mProjects; }
        }

        public SolutionInfo(string filePath)
        {
            mSolutionFile = Path.GetFullPath(filePath);
            LoadDetails();
        }

        private void LoadDetails()
        {

            foreach (var l in File.ReadLines(mSolutionFile))
            {
                if (FoundCsproj(l))
                {
                    AddProjectReference(l);
                }
            }
        }

        private void AddProjectReference(string definition)
        {
            var details = definition.Substring(mPrefixForCsproj.Length).Split(',');

            var projectName = details[0].Trim().WithoutDelimiters("\"", "\"");
            var projectFile = details[1].Trim().WithoutDelimiters("\"", "\"");

            var dirPath = Path.GetDirectoryName(mSolutionFile);
            var filePath = Path.Combine(dirPath, projectFile);

            var projectRef = new ProjectReference(projectName, filePath);
            mProjects.Add(projectRef);
        }

        private bool FoundCsproj(string solutionLine)
        {
            return solutionLine.StartsWith(mPrefixForCsproj);
        }

        private const string mPrefixForCsproj = @"Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ";

        private readonly string mSolutionFile;

        private readonly List<ProjectReference> mProjects = new List<ProjectReference>();
    }
}
