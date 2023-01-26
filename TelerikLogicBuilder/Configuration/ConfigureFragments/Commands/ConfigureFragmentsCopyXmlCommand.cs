using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands
{
    internal class ConfigureFragmentsCopyXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsCopyXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, IConfigureFragmentsForm configureFragmentsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public override void Execute()
        {
            RadTreeNode? selectedNode = configureFragmentsForm.TreeView.SelectedNode;
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
                        _xmlDocumentHelpers.SelectSingleElement(configureFragmentsForm.XmlDocument, selectedNode.Name)
                    )
                )
            );
        }
    }
}
