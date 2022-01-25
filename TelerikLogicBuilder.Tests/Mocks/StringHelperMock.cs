using ABIS.LogicBuilder.FlowBuilder.Services;
using System.Collections.Generic;

namespace TelerikLogicBuilder.Tests.Mocks
{
    internal class StringHelperMock : IStringHelper
    {
        public string EnsureUniqueNameResult { get; set; }
        public string EnsureUniqueName(string nameString, Dictionary<string, string> names)
        {
            return EnsureUniqueNameResult;
        }

        public string[] SplitWithQuoteQualifierResult { get; set; }
        public string[] SplitWithQuoteQualifier(string argument, params string[] delimiters)
        {
            return SplitWithQuoteQualifierResult;
        }

        public string ToTitleCaseResult { get; set; }
        public string ToTitleCase(string str)
        {
            return ToTitleCaseResult;
        }
    }
}
