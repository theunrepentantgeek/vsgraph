using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Niche.Shared
{
    /// <summary>
    /// Extensions for working with Strings
    /// </summary>
    public static class StringExtensions
    {
        public static string WithoutDelimiters(this string value, string start, string finish)
        {
            if (value.StartsWith(start) && value.EndsWith(finish))
            {
                return value.Substring(start.Length, value.Length - start.Length - finish.Length);
            }

            return value;
        }

        public static string RegexFromWildcard(this string wildcard)
        {
            return "^" + Regex.Escape(wildcard).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }

    }
}
