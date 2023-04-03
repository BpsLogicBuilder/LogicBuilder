using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Commands
{
    internal class SelectBooleanFunctionCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditBooleanFunctionForm editBooleanFunctionForm;

        public SelectBooleanFunctionCommand(
            IConfigurationService configurationService,
            IEditBooleanFunctionForm editBooleanFunctionForm)
        {
            _configurationService = configurationService;
            this.editBooleanFunctionForm = editBooleanFunctionForm;
        }

        private HelperButtonDropDownList CmbSelectFunction => editBooleanFunctionForm.CmbSelectFunction;

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            ISelectFunctionForm selectFunctionForm = disposableManager.GetSelectFunctionForm
            (
                editBooleanFunctionForm.AssignedTo,
                _configurationService.FunctionList.BooleanFunctions,
                new TreeFolder[] { _configurationService.FunctionList.BuiltInBooleanFunctionsTreeFolder, _configurationService.FunctionList.BooleanFunctionsTreeFolder }
            );
            selectFunctionForm.SetFunction(CmbSelectFunction.Text);
            selectFunctionForm.ShowDialog((IWin32Window)editBooleanFunctionForm);
            if (selectFunctionForm.DialogResult != DialogResult.OK)
                return;

            CmbSelectFunction.Text = selectFunctionForm.FunctionName;
        }
    }
}
