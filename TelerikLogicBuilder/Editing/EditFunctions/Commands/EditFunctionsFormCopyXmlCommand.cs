using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands
{
    internal class EditFunctionsFormCopyXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditFunctionsForm editFunctionsForm;

        public EditFunctionsFormCopyXmlCommand(IXmlDocumentHelpers xmlDocumentHelpers, IEditFunctionsForm editFunctionsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editFunctionsForm = editFunctionsForm;
        }

        public override void Execute()
        {
            Clipboard.SetText
            (
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument
                    (
                        editFunctionsForm.ShapeXml
                    )
                )
            );
        }
    }
}
