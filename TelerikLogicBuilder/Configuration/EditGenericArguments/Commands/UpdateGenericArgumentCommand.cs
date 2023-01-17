using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Commands
{
    internal class UpdateGenericArgumentCommand : ClickCommandBase
    {
        private readonly IRadListBoxManager<GenericArgumentName> radListBoxManager;
        private readonly RadTextBox txtArgument;

        public UpdateGenericArgumentCommand(IEditGenericArgumentsControl editGenericArgumentsControl)
        {
            radListBoxManager = editGenericArgumentsControl.RadListBoxManager;
            txtArgument = editGenericArgumentsControl.TxtArgument;
        }

        public override void Execute()
        {
            radListBoxManager.Update
            (
                new GenericArgumentName(txtArgument.Text)
            );
        }
    }
}
