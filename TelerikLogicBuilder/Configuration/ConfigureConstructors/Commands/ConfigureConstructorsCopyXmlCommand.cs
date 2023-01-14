using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsCopyXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsCopyXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, IConfigureConstructorsForm configureConstructorsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public override void Execute()
        {
            RadTreeNode? selectedNode = configureConstructorsForm.TreeView.SelectedNode;
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
                        _xmlDocumentHelpers.SelectSingleElement(configureConstructorsForm.XmlDocument, selectedNode.Name)
                    )
                )
            );
        }
    }
}
