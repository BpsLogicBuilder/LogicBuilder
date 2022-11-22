using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Factories
{
    internal interface IConfigureGenericArgumentsCommandFactory
    {
        ReplaceWithListOfLiteralsParameterCommand GetReplaceWithListOfLiteralsParameterCommand(IConfigureGenericArgumentsForm configureGenericArgumentsForm);
        ReplaceWithListOfObjectsParameterCommand GetReplaceWithListOfObjectsParameterCommand(IConfigureGenericArgumentsForm configureGenericArgumentsForm);
        ReplaceWithLiteralParameterCommand GetReplaceWithLiteralParameterCommand(IConfigureGenericArgumentsForm configureGenericArgumentsForm);
        ReplaceWithObjectParameterCommand GetReplaceWithObjectParameterCommand(IConfigureGenericArgumentsForm configureGenericArgumentsForm);
    }
}
