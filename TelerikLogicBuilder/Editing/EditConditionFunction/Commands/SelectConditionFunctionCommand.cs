using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Commands
{
    internal class SelectConditionFunctionCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditConditionFunctionControl editConditionFunctionControl;

        public SelectConditionFunctionCommand(
            IConfigurationService configurationService,
            IEditConditionFunctionControl editConditionFunctionControl)
        {
            _configurationService = configurationService;
            this.editConditionFunctionControl = editConditionFunctionControl;
        }

        private HelperButtonDropDownList CmbSelectFunction => editConditionFunctionControl.CmbSelectFunction;

        public override void Execute()
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using ISelectFunctionForm selectFunctionForm = disposableManager.GetSelectFunctionForm
            (
                typeof(object),
                _configurationService.FunctionList.BooleanFunctions,
                new TreeFolder[] { _configurationService.FunctionList.BuiltInBooleanFunctionsTreeFolder, _configurationService.FunctionList.BooleanFunctionsTreeFolder }
            );
            selectFunctionForm.SetFunction(CmbSelectFunction.Text);
            selectFunctionForm.ShowDialog((IWin32Window)editConditionFunctionControl);
            if (selectFunctionForm.DialogResult != DialogResult.OK)
                return;

            CmbSelectFunction.Text = selectFunctionForm.FunctionName;
        }
    }
}
