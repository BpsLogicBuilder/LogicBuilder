using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands
{
    internal class EditDecisionFormEditXmlCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditDecisionForm editDecisionForm;

        public EditDecisionFormEditXmlCommand(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditDecisionForm editDecisionForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editDecisionForm = editDecisionForm;
        }

        public override void Execute()
        {
            using IEditXmlFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditXmlFormFactory>();
            IEditBuildDecisionFormXml editXmlForm = disposableManager.GetEditBuildDecisionFormXml
            (
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument(editDecisionForm.DecisionXml)
                )
            );
            editXmlForm.ShowDialog((IWin32Window)editDecisionForm);
            if (editXmlForm.DialogResult != DialogResult.OK)
                return;

            editDecisionForm.UpdateDecisionFunctionsList(editXmlForm.XmlResult);
        }
    }
}
