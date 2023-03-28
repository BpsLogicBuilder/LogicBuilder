using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectConstructor;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Commands
{
    internal class SelectConstructorCommand : ClickCommandBase
    {
        private readonly IEditConstructorForm editConstructorForm;

        public SelectConstructorCommand(IEditConstructorForm editConstructorForm)
        {
            this.editConstructorForm = editConstructorForm;
        }

        private HelperButtonDropDownList CmbSelectConstructor => editConstructorForm.CmbSelectConstructor;

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            ISelectConstructorForm selectConstructorForm = disposableManager.GetSelectConstructorForm(editConstructorForm.AssignedTo);
            selectConstructorForm.SetConstructor(CmbSelectConstructor.Text);
            selectConstructorForm.ShowDialog((IWin32Window)editConstructorForm);
            if (selectConstructorForm.DialogResult != DialogResult.OK)
                return;

            CmbSelectConstructor.Text = selectConstructorForm.ConstructorName;
        }
    }
}
