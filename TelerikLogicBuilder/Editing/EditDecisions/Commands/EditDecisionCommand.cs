using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecision;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Commands
{
    internal class EditDecisionCommand : ClickCommandBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditDecisionsForm editDecisionsForm;

        public EditDecisionCommand(
            IXmlDocumentHelpers xmlDocumentHelpers, 
            IEditDecisionsForm editDecisionsForm)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editDecisionsForm = editDecisionsForm;
        }

        private HelperButtonTextBox TxtEditDecision => editDecisionsForm.TxtEditDecision;

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditDecisionForm editDecisionForm = disposableManager.GetEditDecisionForm
            (
                GetDecisionXmlDocument()
            );

            editDecisionForm.ShowDialog((IWin32Window)editDecisionsForm);
            if (editDecisionForm.DialogResult != DialogResult.OK)
                return;

            TxtEditDecision.Tag = editDecisionForm.DecisionXml;
            TxtEditDecision.Text = editDecisionForm.DecisionVisibleText;

            XmlDocument? GetDecisionXmlDocument()
            {
                if (TxtEditDecision.Tag == null)
                    return null;

                return _xmlDocumentHelpers.ToXmlDocument((string)TxtEditDecision.Tag);
            }
        }
    }
}
