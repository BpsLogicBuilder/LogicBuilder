using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class StringHelperMock : IStringHelper
    {
        public string EnsureUniqueNameResult { get; set; }
        public string EnsureUniqueName(string nameString, HashSet<string> names)
        {
            return EnsureUniqueNameResult;
        }

        public string[] SplitWithQuoteQualifierResult { get; set; }
        public string[] SplitWithQuoteQualifier(string argument, params string[] delimiters)
        {
            return SplitWithQuoteQualifierResult;
        }

        public string ToShortNameResult { get; set; }
        public string ToShortName(string fullName)
        {
            return ToShortNameResult;
        }

        public string ToCamelCaseResult { get; set; }
        public string ToCamelCase(string s)
        {
            return ToCamelCaseResult;
        }
    }
}
