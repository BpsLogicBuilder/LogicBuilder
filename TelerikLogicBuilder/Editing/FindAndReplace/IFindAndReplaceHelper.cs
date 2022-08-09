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
            Func<string, string, bool, bool, IList<string>> matchFunc);

        GridViewCellInfo? FindItemAllRows(RadGridView dataGridView,
            RadListControl listOccurrences,
            RadGroupBox groupBoxOccurrences,
            ref int searchRowIndex,
            ref int searchCellIndex,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, IList<string>> matchFunc);

        Shape? FindItemCurrentPage(Document visioDocument,
            RadListControl listOccurrences,
            RadGroupBox groupBoxOccurrences,
            ref int searchShapeIndex,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, IList<string>> matchFunc);

        GridViewCellInfo? FindItemCurrentRow(RadGridView dataGridView,
            RadListControl listOccurrences,
            RadGroupBox groupBoxOccurrences,
            ref int searchCellIndex,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, IList<string>> matchFunc);

        string GetVisibleText(int columnIndex, string cellXml);
        string GetVisibleText(string universalMasterName, string shapeXml);
    }
}
