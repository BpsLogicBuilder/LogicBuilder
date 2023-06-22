using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands
{
    internal class EditFunctionsFormEditXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditFunctionsForm editFunctionsForm;

        public EditFunctionsFormEditXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditFunctionsForm editFunctionsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editFunctionsForm = editFunctionsForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditFunctionsFormXml editXmlForm = disposableManager.GetEditFunctionsFormXml
            (
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument(editFunctionsForm.ShapeXml)
                )
            );
            editXmlForm.ShowDialog((IWin32Window)editFunctionsForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editFunctionsForm.ClearMessage();
            editFunctionsForm.UpdateFunctionsList(editXmlForm.XmlResult);
        }
    }
}
