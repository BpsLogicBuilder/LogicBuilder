using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TelerikLogicBuilder.FormsPreviewer.Commands
{
    internal class SelectFunctionFormCommand : ClickCommandBase
    {
        private readonly RadForm1 radForm;
        private readonly IDictionary<string, Function> functionDistionary;
        private readonly IList<TreeFolder> treeFolders;

        public SelectFunctionFormCommand(RadForm1 radForm, IDictionary<string, Function> functionDistionary, IList<TreeFolder> treeFolders)
        {
            this.radForm = radForm;
            this.functionDistionary = functionDistionary;
            this.treeFolders = treeFolders;
        }

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = ABIS.LogicBuilder.FlowBuilder.Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            ISelectFunctionForm selectFunctionForm = disposableManager.GetSelectFunctionForm(typeof(object), this.functionDistionary, this.treeFolders);
            selectFunctionForm.ShowDialog(radForm);
            if (selectFunctionForm.DialogResult != DialogResult.OK)
                return;
        }
    }
}
