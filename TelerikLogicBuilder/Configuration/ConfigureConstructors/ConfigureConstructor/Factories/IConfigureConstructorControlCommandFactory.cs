using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Factories
{
    internal interface IConfigureConstructorControlCommandFactory
    {
        EditConstructorTypeNameCommand GetEditConstructorTypeNameCommand(IConfigureConstructorControl configureConstructorControl);
        EditGenericArgumentsCommand GetEditGenericArgumentsCommand(IConfigureConstructorControl configureConstructorControl);
    }
}
