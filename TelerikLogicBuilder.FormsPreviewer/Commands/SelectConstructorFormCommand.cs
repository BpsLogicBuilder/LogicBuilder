using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class SelectConstructorFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;
        public SelectConstructorFormCommand(RadForm1 radForm)
        {
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using ISelectConstructorForm selectConstructorForm = disposableManager.GetSelectConstructorForm(typeof(object));
            selectConstructorForm.ShowDialog(radForm);
            if (selectConstructorForm.DialogResult != DialogResult.OK)
                return;
        }
    }
}
