using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IStringHelper
    {
        string EnsureUniqueName(string nameString, HashSet<string> names);
        string[] SplitWithQuoteQualifier(string argument, params string[] delimiters);
        string ToCamelCase(string s);
        string ToShortName(string fullName);
    }
}
