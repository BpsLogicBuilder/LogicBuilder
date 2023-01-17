using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Factories
{
    internal interface IEditGenericArgumentsCommandFactory
    {
        AddGenericArgumentCommand GetAddGenericArgumentCommand(IEditGenericArgumentsControl editGenericArgumentsControl);
        UpdateGenericArgumentCommand GetUpdateGenericArgumentCommand(IEditGenericArgumentsControl editGenericArgumentsControl);
    }
}
