﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class EditVariableFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;
        public EditVariableFormCommand(RadForm1 radForm)
        {
            this.radForm = radForm;
        }

        public override void Execute()
        {
            IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IEditVariableForm selectVariableForm = disposableManager.GetEditVariableForm(typeof(object));
            selectVariableForm.ShowDialog(radForm);
            if (selectVariableForm.DialogResult != DialogResult.OK)
                return;
        }
    }
}
