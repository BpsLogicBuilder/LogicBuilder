using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsCopyXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsCopyXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public override void Execute()
        {
            RadTreeNode? selectedNode = configureFunctionsForm.TreeView.SelectedNode;
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
                        _xmlDocumentHelpers.SelectSingleElement(configureFunctionsForm.XmlDocument, selectedNode.Name)
                    )
                )
            );
        }
    }
}
