using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Commands
{
    internal class SelectDialogFunctionCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditDialogFunctionForm editDialogFunctionForm;

        public SelectDialogFunctionCommand(
            IConfigurationService configurationService,
            IEditDialogFunctionForm editDialogFunctionForm)
        {
            _configurationService = configurationService;
            this.editDialogFunctionForm = editDialogFunctionForm;
        }

        private HelperButtonDropDownList CmbSelectFunction => editDialogFunctionForm.CmbSelectFunction;

        public override void Execute()
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using ISelectFunctionForm selectFunctionForm = disposableManager.GetSelectFunctionForm
            (
                editDialogFunctionForm.AssignedTo,
                _configurationService.FunctionList.DialogFunctions,
                new TreeFolder[] { _configurationService.FunctionList.BuiltInDialogFunctionsTreeFolder, _configurationService.FunctionList.DialogFunctionsTreeFolder }
            );
            selectFunctionForm.SetFunction(CmbSelectFunction.Text);
            selectFunctionForm.ShowDialog((IWin32Window)editDialogFunctionForm);
            if (selectFunctionForm.DialogResult != DialogResult.OK)
                return;

            CmbSelectFunction.Text = selectFunctionForm.FunctionName;
        }
    }
}
