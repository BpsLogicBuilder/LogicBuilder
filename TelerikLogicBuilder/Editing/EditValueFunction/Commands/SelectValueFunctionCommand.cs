using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Commands
{
    internal class SelectValueFunctionCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditValueFunctionForm editValueFunctionForm;

        public SelectValueFunctionCommand(
            IConfigurationService configurationService,
            IEditValueFunctionForm editValueFunctionForm)
        {
            _configurationService = configurationService;
            this.editValueFunctionForm = editValueFunctionForm;
        }

        private HelperButtonDropDownList CmbSelectFunction => editValueFunctionForm.CmbSelectFunction;

        public override void Execute()
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using ISelectFunctionForm selectFunctionForm = disposableManager.GetSelectFunctionForm
            (
                editValueFunctionForm.AssignedTo,
                _configurationService.FunctionList.ValueFunctions,
                new TreeFolder[] { _configurationService.FunctionList.BuiltInValueFunctionsTreeFolder, _configurationService.FunctionList.ValueFunctionsTreeFolder }
            );
            selectFunctionForm.SetFunction(CmbSelectFunction.Text);
            selectFunctionForm.ShowDialog((IWin32Window)editValueFunctionForm);
            if (selectFunctionForm.DialogResult != DialogResult.OK)
                return;

            CmbSelectFunction.Text = selectFunctionForm.FunctionName;
        }
    }
}
