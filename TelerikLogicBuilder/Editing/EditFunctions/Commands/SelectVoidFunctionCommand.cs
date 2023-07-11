using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectFunction;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Commands
{
    internal class SelectVoidFunctionCommand : ClickCommandBase
    {
        private readonly IEditVoidFunctionControl editVoidFunctionControl;

        public SelectVoidFunctionCommand(IEditVoidFunctionControl editVoidFunctionControl)
        {
            this.editVoidFunctionControl = editVoidFunctionControl;
        }

        private HelperButtonDropDownList CmbSelectFunction => editVoidFunctionControl.CmbSelectFunction;

        public override void Execute()
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using ISelectFunctionForm selectFunctionForm = disposableManager.GetSelectFunctionForm
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
