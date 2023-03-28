using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IDataGraphEditingForm : IEditingForm
    {
        event FormClosingEventHandler? FormClosing;
        bool DenySpecialCharacters { get; }
        bool DisplayNotCheckBox { get; }
        RadPanel RadPanelFields { get; }
        RadTreeView TreeView { get; }
        XmlDocument XmlDocument { get; }
        void DisableControlsDuringEdit(bool disable);
        void ValidateXmlDocument();
    }
}
