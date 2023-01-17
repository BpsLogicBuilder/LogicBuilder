using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Commands
{
    internal class AddGenericArgumentCommand : ClickCommandBase
    {
        private readonly IRadListBoxManager<GenericArgumentName> radListBoxManager;
        private readonly RadTextBox txtArgument;

        public AddGenericArgumentCommand(IEditGenericArgumentsControl editGenericArgumentsControl)
        {
            radListBoxManager = editGenericArgumentsControl.RadListBoxManager;
            txtArgument = editGenericArgumentsControl.TxtArgument; 
        }

        public override void Execute()
        {
            radListBoxManager.Add
            (
                new GenericArgumentName(txtArgument.Text)
            );
        }
    }
}
