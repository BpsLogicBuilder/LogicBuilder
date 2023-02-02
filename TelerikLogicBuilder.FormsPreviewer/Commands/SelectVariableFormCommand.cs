using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class SelectVariableFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;
        public SelectVariableFormCommand(RadForm1 radForm)
        {
            this.radForm = radForm;
        }

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            ISelectVariableForm selectVariableForm = disposableManager.GetSelectVariableForm(typeof(object));
            selectVariableForm.ShowDialog(radForm);
            if (selectVariableForm.DialogResult != DialogResult.OK)
                return;
        }
    }
}
