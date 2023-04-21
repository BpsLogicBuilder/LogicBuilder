using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands
{
    internal class EditDecisionFormCopyXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditDecisionForm editDecisionForm;

        public EditDecisionFormCopyXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditDecisionForm editDecisionForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editDecisionForm = editDecisionForm;
        }

        public override void Execute()
        {
            Clipboard.SetText
            (
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument
                    (
                        editDecisionForm.DecisionXml
                    )
                )
            );
        }
    }
}
