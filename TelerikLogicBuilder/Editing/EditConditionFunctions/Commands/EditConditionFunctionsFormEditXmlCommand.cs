using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions.Commands
{
    internal class EditConditionFunctionsFormEditXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditConditionFunctionsForm editConditionFunctionsForm;

        public EditConditionFunctionsFormEditXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditConditionFunctionsForm editConditionFunctionsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editConditionFunctionsForm = editConditionFunctionsForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditConditionsFormXml editXmlForm = disposableManager.GetEditConditionsFormXml
            (
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument(editConditionFunctionsForm.ShapeXml)
                )
            );
            editXmlForm.ShowDialog((IWin32Window)editConditionFunctionsForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editConditionFunctionsForm.ClearMessage();
            editConditionFunctionsForm.UpdateConditionsList(editXmlForm.XmlResult);
        }
    }
}
