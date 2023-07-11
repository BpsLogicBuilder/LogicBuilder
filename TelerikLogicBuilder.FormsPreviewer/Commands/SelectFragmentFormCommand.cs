using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class SelectFragmentFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;
        public SelectFragmentFormCommand(RadForm1 radForm)
        {
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using ISelectFragmentForm selectFragmentForm = disposableManager.GetSelectFragmentForm();
            selectFragmentForm.ShowDialog(radForm);
            if (selectFragmentForm.DialogResult != DialogResult.OK)
                return;
        }
    }
}
