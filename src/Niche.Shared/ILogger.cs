using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Niche.Shared
{
    public interface ILogger
    {
        void Heading(string template, params object[] arguments);
        void Subheading(string template, params object[] arguments);
        void Action(string template, params object[] arguments);
        void Detail(string template, params object[] arguments);
        void Success(string template, params object[] arguments);
        void Failure(string template, params object[] arguments);
    }
}
