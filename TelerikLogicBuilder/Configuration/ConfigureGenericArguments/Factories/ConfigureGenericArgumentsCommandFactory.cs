using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.Factories
{
    internal class ConfigureGenericArgumentsCommandFactory : IConfigureGenericArgumentsCommandFactory
    {
        private readonly Func<IConfigureGenericArgumentsForm, ReplaceWithListOfLiteralsParameterCommand> _getReplaceWithListOfLiteralsParameterCommand;
        private readonly Func<IConfigureGenericArgumentsForm, ReplaceWithListOfObjectsParameterCommand> _getReplaceWithListOfObjectsParameterCommand;
        private readonly Func<IConfigureGenericArgumentsForm, ReplaceWithLiteralParameterCommand> _getReplaceWithLiteralParameterCommand;
        private readonly Func<IConfigureGenericArgumentsForm, ReplaceWithObjectParameterCommand> _getReplaceWithObjectParameterCommand;

        public ConfigureGenericArgumentsCommandFactory(
            Func<IConfigureGenericArgumentsForm, ReplaceWithListOfLiteralsParameterCommand> getReplaceWithListOfLiteralsParameterCommand,
            Func<IConfigureGenericArgumentsForm, ReplaceWithListOfObjectsParameterCommand> getReplaceWithListOfObjectsParameterCommand,
            Func<IConfigureGenericArgumentsForm, ReplaceWithLiteralParameterCommand> getReplaceWithLiteralParameterCommand,
            Func<IConfigureGenericArgumentsForm, ReplaceWithObjectParameterCommand> getReplaceWithObjectParameterCommand)
        {
            _getReplaceWithListOfLiteralsParameterCommand = getReplaceWithListOfLiteralsParameterCommand;
            _getReplaceWithListOfObjectsParameterCommand = getReplaceWithListOfObjectsParameterCommand;
            _getReplaceWithLiteralParameterCommand = getReplaceWithLiteralParameterCommand;
            _getReplaceWithObjectParameterCommand = getReplaceWithObjectParameterCommand;
        }

        public ReplaceWithListOfLiteralsParameterCommand GetReplaceWithListOfLiteralsParameterCommand(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getReplaceWithListOfLiteralsParameterCommand(configureGenericArgumentsForm);

        public ReplaceWithListOfObjectsParameterCommand GetReplaceWithListOfObjectsParameterCommand(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getReplaceWithListOfObjectsParameterCommand(configureGenericArgumentsForm);

        public ReplaceWithLiteralParameterCommand GetReplaceWithLiteralParameterCommand(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getReplaceWithLiteralParameterCommand(configureGenericArgumentsForm);

        public ReplaceWithObjectParameterCommand GetReplaceWithObjectParameterCommand(IConfigureGenericArgumentsForm configureGenericArgumentsForm)
            => _getReplaceWithObjectParameterCommand(configureGenericArgumentsForm);
    }
}
