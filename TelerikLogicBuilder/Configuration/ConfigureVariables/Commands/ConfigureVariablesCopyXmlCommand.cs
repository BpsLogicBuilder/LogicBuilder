using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class ConfigureVariablesCopyXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesCopyXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, IConfigureVariablesForm configureVariablesForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            RadTreeNode? selectedNode = configureVariablesForm.TreeView.SelectedNode;
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
                        _xmlDocumentHelpers.SelectSingleElement(configureVariablesForm.XmlDocument, selectedNode.Name)
                    )
                )
            );
        }
    }
}
