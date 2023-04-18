using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Commands
{
    internal class SelectVariableCommand : ClickCommandBase
    {
        private readonly HelperButtonDropDownList helperButtonDropDownList;

        public SelectVariableCommand(HelperButtonDropDownList helperButtonDropDownList)
        {
            this.helperButtonDropDownList = helperButtonDropDownList;
        }

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditVariableForm editVariableForm = disposableManager.GetEditVariableForm(typeof(object));

            editVariableForm.SetVariable(helperButtonDropDownList.Text);
            editVariableForm.ShowDialog(helperButtonDropDownList);
            if (editVariableForm.DialogResult != DialogResult.OK)
                return;

            helperButtonDropDownList.Text = editVariableForm.VariableName;
        }
    }
}
