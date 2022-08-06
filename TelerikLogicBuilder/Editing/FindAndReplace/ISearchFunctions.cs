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
        List<string> FindTextMatches(XmlDocument xmlDocument, string searchString, bool matchCase, bool matchWholeWord);
        List<string> FindTextMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord);
    }
}
