using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface IFindAndReplaceHelper
    {
        Shape? FindItemAllPages(Document visioDocument,
            RadListControl listOccurrences,
            RadGroupBox groupBoxOccurrences,
            ref int searchPageIndex,
            ref int searchShapeIndex,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, List<string>> matchFunc);

        Shape? FindItemCurrentPage(Document visioDocument,
            RadListControl listOccurrences,
            RadGroupBox groupBoxOccurrences,
            ref int searchShapeIndex,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, List<string>> matchFunc);
        string GetVisibleText(int columnIndex, string cellXml);
        string GetVisibleText(string universalMasterName, string shapeXml);
    }
}
