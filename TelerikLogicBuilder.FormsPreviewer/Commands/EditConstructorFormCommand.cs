using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class EditConstructorFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;
        public EditConstructorFormCommand(RadForm1 radForm)
        {
            this.radForm = radForm;
        }

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditConstructorForm selectConstructorForm = disposableManager.GetEditConstructorForm(typeof(object));
            selectConstructorForm.ShowDialog(radForm);
            if (selectConstructorForm.DialogResult != DialogResult.OK)
                return;
        }
    }
}
