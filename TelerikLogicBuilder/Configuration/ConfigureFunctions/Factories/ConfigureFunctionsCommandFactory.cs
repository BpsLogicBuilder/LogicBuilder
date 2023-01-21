using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories
{
    internal class ConfigureFunctionsCommandFactory : IConfigureFunctionsCommandFactory
    {
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddBinaryOperatorCommand> _getConfigureFunctionsAddBinaryOperatorCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddClassMembersCommand> _getConfigureFunctionsAddClassMembersCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddDialogFunctionCommand> _getConfigureFunctionsAddDialogFunctionCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddFolderCommand> _getConfigureFunctionsAddFolderCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddGenericParameterCommand> _getConfigureFunctionsAddGenericParameterCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddListOfGenericsParameterCommand> _getConfigureFunctionsAddListOfGenericsParameterCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddListOfLiteralsParameterCommand> _getConfigureFunctionsAddListOfLiteralsParameterCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddListOfObjectsParameterCommand> _getConfigureFunctionsAddListOfObjectsParameterCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddLiteralParameterCommand> _getConfigureFunctionsAddLiteralParameterCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddObjectParameterCommand> _getConfigureFunctionsAddObjectParameterCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsAddStandardFunctionCommand> _getConfigureFunctionsAddStandardFunctionCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsCopyXmlCommand> _getConfigureFunctionsCopyXmlCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsCutCommand> _getConfigureFunctionsCutCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsDeleteCommand> _getConfigureFunctionsDeleteCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsHelperCommand> _getConfigureFunctionsHelperCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsImportCommand> _getConfigureFunctionsImportCommand;
        private readonly Func<IConfigureFunctionsForm, ConfigureFunctionsPasteCommand> _getConfigureFunctionsPasteCommand;

        public ConfigureFunctionsCommandFactory(
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddBinaryOperatorCommand> getConfigureFunctionsAddBinaryOperatorCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddClassMembersCommand> getConfigureFunctionsAddClassMembersCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddDialogFunctionCommand> getConfigureFunctionsAddDialogFunctionCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddFolderCommand> getConfigureFunctionsAddFolderCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddGenericParameterCommand> getConfigureFunctionsAddGenericParameterCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddListOfGenericsParameterCommand> getConfigureFunctionsAddListOfGenericsParameterCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddListOfLiteralsParameterCommand> getConfigureFunctionsAddListOfLiteralsParameterCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddListOfObjectsParameterCommand> getConfigureFunctionsAddListOfObjectsParameterCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddLiteralParameterCommand> getConfigureFunctionsAddLiteralParameterCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddObjectParameterCommand> getConfigureFunctionsAddObjectParameterCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsAddStandardFunctionCommand> getConfigureFunctionsAddStandardFunctionCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsCopyXmlCommand> getConfigureFunctionsCopyXmlCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsCutCommand> getConfigureFunctionsCutCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsDeleteCommand> getConfigureFunctionsDeleteCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsHelperCommand> getConfigureFunctionsHelperCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsImportCommand> getConfigureFunctionsImportCommand,
            Func<IConfigureFunctionsForm, ConfigureFunctionsPasteCommand> getConfigureFunctionsPasteCommand)
        {
            _getConfigureFunctionsAddBinaryOperatorCommand = getConfigureFunctionsAddBinaryOperatorCommand;
            _getConfigureFunctionsAddClassMembersCommand = getConfigureFunctionsAddClassMembersCommand;
            _getConfigureFunctionsAddDialogFunctionCommand = getConfigureFunctionsAddDialogFunctionCommand;
            _getConfigureFunctionsAddFolderCommand = getConfigureFunctionsAddFolderCommand;
            _getConfigureFunctionsAddGenericParameterCommand = getConfigureFunctionsAddGenericParameterCommand;
            _getConfigureFunctionsAddListOfGenericsParameterCommand = getConfigureFunctionsAddListOfGenericsParameterCommand;
            _getConfigureFunctionsAddListOfLiteralsParameterCommand = getConfigureFunctionsAddListOfLiteralsParameterCommand;
            _getConfigureFunctionsAddListOfObjectsParameterCommand = getConfigureFunctionsAddListOfObjectsParameterCommand;
            _getConfigureFunctionsAddLiteralParameterCommand = getConfigureFunctionsAddLiteralParameterCommand;
            _getConfigureFunctionsAddObjectParameterCommand = getConfigureFunctionsAddObjectParameterCommand;
            _getConfigureFunctionsAddStandardFunctionCommand = getConfigureFunctionsAddStandardFunctionCommand;
            _getConfigureFunctionsCopyXmlCommand = getConfigureFunctionsCopyXmlCommand;
            _getConfigureFunctionsCutCommand = getConfigureFunctionsCutCommand;
            _getConfigureFunctionsDeleteCommand = getConfigureFunctionsDeleteCommand;
            _getConfigureFunctionsHelperCommand = getConfigureFunctionsHelperCommand;
            _getConfigureFunctionsImportCommand = getConfigureFunctionsImportCommand;
            _getConfigureFunctionsPasteCommand = getConfigureFunctionsPasteCommand;
        }

        public ConfigureFunctionsAddBinaryOperatorCommand GetConfigureFunctionsAddBinaryOperatorCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddBinaryOperatorCommand(configureFunctionsForm);

        public ConfigureFunctionsAddClassMembersCommand GetConfigureFunctionsAddClassMembersCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddClassMembersCommand(configureFunctionsForm);

        public ConfigureFunctionsAddDialogFunctionCommand GetConfigureFunctionsAddDialogFunctionCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddDialogFunctionCommand(configureFunctionsForm);

        public ConfigureFunctionsAddFolderCommand GetConfigureFunctionsAddFolderCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddFolderCommand(configureFunctionsForm);

        public ConfigureFunctionsAddGenericParameterCommand GetConfigureFunctionsAddGenericParameterCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddGenericParameterCommand(configureFunctionsForm);

        public ConfigureFunctionsAddListOfGenericsParameterCommand GetConfigureFunctionsAddListOfGenericsParameterCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddListOfGenericsParameterCommand(configureFunctionsForm);

        public ConfigureFunctionsAddListOfLiteralsParameterCommand GetConfigureFunctionsAddListOfLiteralsParameterCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddListOfLiteralsParameterCommand(configureFunctionsForm);

        public ConfigureFunctionsAddListOfObjectsParameterCommand GetConfigureFunctionsAddListOfObjectsParameterCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddListOfObjectsParameterCommand(configureFunctionsForm);

        public ConfigureFunctionsAddLiteralParameterCommand GetConfigureFunctionsAddLiteralParameterCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddLiteralParameterCommand(configureFunctionsForm);

        public ConfigureFunctionsAddObjectParameterCommand GetConfigureFunctionsAddObjectParameterCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddObjectParameterCommand(configureFunctionsForm);

        public ConfigureFunctionsAddStandardFunctionCommand GetConfigureFunctionsAddStandardFunctionCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsAddStandardFunctionCommand(configureFunctionsForm);

        public ConfigureFunctionsCopyXmlCommand GetConfigureFunctionsCopyXmlCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsCopyXmlCommand(configureFunctionsForm);

        public ConfigureFunctionsCutCommand GetConfigureFunctionsCutCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsCutCommand(configureFunctionsForm);

        public ConfigureFunctionsDeleteCommand GetConfigureFunctionsDeleteCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsDeleteCommand(configureFunctionsForm);

        public ConfigureFunctionsHelperCommand GetConfigureFunctionsHelperCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsHelperCommand(configureFunctionsForm);

        public ConfigureFunctionsImportCommand GetConfigureFunctionsImportCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsImportCommand(configureFunctionsForm);

        public ConfigureFunctionsPasteCommand GetConfigureFunctionsPasteCommand(IConfigureFunctionsForm configureFunctionsForm)
            => _getConfigureFunctionsPasteCommand(configureFunctionsForm);
    }
}
