using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands
{
    internal class EditDecisionsFormEditXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditDecisionsForm editDecisionsForm;

        public EditDecisionsFormEditXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditDecisionsForm editDecisionsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editDecisionsForm = editDecisionsForm;
        }

        public override void Execute()
        {
            IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            using IEditDecisionsFormXml editXmlForm = disposableManager.GetEditDecisionsFormXml
            (
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument(editDecisionsForm.ShapeXml)
                )
            );

            editXmlForm.ShowDialog((IWin32Window)editDecisionsForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editDecisionsForm.ClearMessage();
            editDecisionsForm.UpdateDecisionsList(editXmlForm.XmlResult);
        }
    }
}
