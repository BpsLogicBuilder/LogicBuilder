using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal interface IStringHelper
    {
        string[] SplitWithQuoteQualifier(string argument, params string[] delimiters);
        string ToTitleCase(string str);
        string EnsureUniqueName(string nameString, Dictionary<string, string> names);
    }
}
