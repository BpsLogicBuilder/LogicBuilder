using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface ISearchFunctions
    {
        IList<string> FindConstructorMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord);
        IList<string> FindFunctionMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord);
        IList<string> FindTextMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord);
        IList<string> FindVariableMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord);
        string ReplaceConstructorMatches(string xmlString, string searchString, string replacement, bool matchCase, bool matchWholeWord);
        string ReplaceFunctionMatches(string xmlString, string searchString, string replacement, bool matchCase, bool matchWholeWord);
        string ReplaceTextMatches(string xmlString, string searchString, string replacement, bool matchCase, bool matchWholeWord);
        string ReplaceVariableMatches(string xmlString, string searchString, string replacement, bool matchCase, bool matchWholeWord);
    }
}
