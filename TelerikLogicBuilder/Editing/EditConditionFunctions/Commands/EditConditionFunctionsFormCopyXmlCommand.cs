using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Commands
{
    internal class EditConditionFunctionsFormCopyXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditConditionFunctionsForm editConditionFunctionsForm;

        public EditConditionFunctionsFormCopyXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditConditionFunctionsForm editConditionFunctionsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editConditionFunctionsForm = editConditionFunctionsForm;
        }

        public override void Execute()
        {
            Clipboard.SetText
            (
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument
                    (
                        editConditionFunctionsForm.ShapeXml
                    )
                )
            );
        }
    }
}
