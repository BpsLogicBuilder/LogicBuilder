using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFromDomain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class SelectDomainItemCommand : ClickCommandBase
    {
        private readonly IDomainRichInputBoxValueControl richInputBoxValueControl;

        public SelectDomainItemCommand(
            IDomainRichInputBoxValueControl literalParameterDomainRichInputBoxControl)
        {
            this.richInputBoxValueControl = literalParameterDomainRichInputBoxControl;
        }

        private RichInputBox RichInputBox => richInputBoxValueControl.RichInputBox;

        public override void Execute()
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using ISelectFromDomainForm selectFromDomainForm = disposableManager.GetSelectFromDomainForm(richInputBoxValueControl.Domain, richInputBoxValueControl.Comments);

            selectFromDomainForm.ShowDialog(RichInputBox);
            if (selectFromDomainForm.DialogResult != DialogResult.OK)
                return;

            RichInputBox.Clear();
            RichInputBox.InsertText(selectFromDomainForm.SelectedItem);
            richInputBoxValueControl.Focus();
        }
    }
}
