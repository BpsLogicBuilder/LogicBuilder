using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Commands
{
    internal class CopyXmlToClipboardCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IDataGraphEditingHost dataGraphEditingHost;

        public CopyXmlToClipboardCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
        }

        public override void Execute()
        {
            RadTreeNode? selectedNode = dataGraphEditingHost.TreeView.SelectedNode;
            if (selectedNode == null)
            {
                Clipboard.Clear();
                return;
            }

            Clipboard.SetText
            (
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument
                    (
                        _xmlDocumentHelpers.SelectSingleElement(dataGraphEditingHost.XmlDocument, selectedNode.Name)
                    )
                )
            );
        }
    }
}
