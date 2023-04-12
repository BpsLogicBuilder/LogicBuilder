using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IDataGraphEditingHost : IApplicationHostControl, IAssignedTo, IEditVariableHost, IExpandedNodes, IRequestDocumentUpdate, ISetDialogMessages
    {
        bool DenySpecialCharacters { get; }
        bool DisplayNotCheckBox { get; }
        RadPanel RadPanelFields { get; }
        RadTreeView TreeView { get; }
        string VisibleText { get; }
        XmlDocument XmlDocument { get; }
        XmlElement XmlResult { get; }
        void DisableControlsDuringEdit(bool disable);
        void RebuildTreeView();
        void ReloadXmlDocument(string xmlString);
        void ValidateXmlDocument();
    }
}
