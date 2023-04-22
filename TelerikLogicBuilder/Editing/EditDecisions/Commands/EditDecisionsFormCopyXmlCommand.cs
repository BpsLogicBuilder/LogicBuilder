using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands
{
    internal class EditDecisionsFormCopyXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditDecisionsForm editDecisionsForm;

        public EditDecisionsFormCopyXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditDecisionsForm editDecisionsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editDecisionsForm = editDecisionsForm;
        }

        public override void Execute()
        {
            Clipboard.SetText
            (
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument
                    (
                        editDecisionsForm.ShapeXml
                    )
                )
            );
        }
    }
}
