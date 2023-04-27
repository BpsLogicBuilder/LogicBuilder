using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands
{
    internal class SelectVoidFunctionCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditVoidFunctionControl editVoidFunctionControl;

        public SelectVoidFunctionCommand(IConfigurationService configurationService, IEditVoidFunctionControl editVoidFunctionControl)
        {
            _configurationService = configurationService;
            this.editVoidFunctionControl = editVoidFunctionControl;
        }

        private HelperButtonDropDownList CmbSelectFunction => editVoidFunctionControl.CmbSelectFunction;

        public override void Execute()
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            ISelectFunctionForm selectFunctionForm = disposableManager.GetSelectFunctionForm
            (
                typeof(object),
                editVoidFunctionControl.FunctionDictionary,
                editVoidFunctionControl.TreeFolders
            );
            selectFunctionForm.SetFunction(CmbSelectFunction.Text);
            selectFunctionForm.ShowDialog((IWin32Window)editVoidFunctionControl);
            if (selectFunctionForm.DialogResult != DialogResult.OK)
                return;

            CmbSelectFunction.Text = selectFunctionForm.FunctionName;
        }
    }
}
