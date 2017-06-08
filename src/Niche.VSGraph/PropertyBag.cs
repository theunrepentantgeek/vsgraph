using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Niche.Shared;

namespace Niche.VSGraph
{
    public class PropertyBag
    {
        public string Heading
        {
            get { return mHeading; }
        }

        public PropertyBag()
        {
            // Nothing
        }

        public PropertyBag(string heading)
            : base()
        {
            mHeading = heading;
        }

        public void Add(string property)
        {
            mProperties.Add(property);
        }

        public string AsLabel()
        {
            var details = mProperties.JoinWith("\\l") + "\\l";
            if (string.IsNullOrEmpty(Heading))
            {
                return details;
            }

            return string.Format("{0} | {1}", Encoded(Heading), details);
        }

        private string Encoded(string value)
        {
            return value.Replace("<", "\\<")
                .Replace(">", "\\>")
                .Replace("{", "\\{")
                .Replace("}", "\\}")
                .Replace("|", "::");
        }

        private readonly string mHeading;

        private readonly List<string> mProperties = new List<string>();
    }
}
