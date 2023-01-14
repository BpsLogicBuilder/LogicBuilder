using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Factories
{
    internal class ConfigureConstructorsCommandFactory : IConfigureConstructorsCommandFactory
    {
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsAddConstructorCommand> _getConfigureConstructorsAddConstructorCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsAddFolderCommand> _getConfigureConstructorsAddFolderCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsAddGenericParameterCommand> _getConfigureConstructorsAddGenericParameterCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsAddListOfGenericsParameterCommand> _getConfigureConstructorsAddListOfGenericsParameterCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsAddListOfLiteralsParameterCommand> _getConfigureConstructorsAddListOfLiteralsParameterCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsAddListOfObjectsParameterCommand> _getConfigureConstructorsAddListOfObjectsParameterCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsAddLiteralParameterCommand> _getConfigureConstructorsAddLiteralParameterCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsAddObjectParameterCommand> _getConfigureConstructorsAddObjectParameterCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsCopyXmlCommand> _getConfigureConstructorsCopyXmlCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsCutCommand> _getConfigureConstructorsCutCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsDeleteCommand> _getConfigureConstructorsDeleteCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsHelperCommand> _getConfigureConstructorsHelperCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsImportCommand> _getConfigureConstructorsImportCommand;
        private readonly Func<IConfigureConstructorsForm, ConfigureConstructorsPasteCommand> _getConfigureConstructorsPasteCommand;

        public ConfigureConstructorsCommandFactory(
            Func<IConfigureConstructorsForm, ConfigureConstructorsAddConstructorCommand> getConfigureConstructorsAddConstructorCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsAddFolderCommand> getConfigureConstructorsAddFolderCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsAddGenericParameterCommand> getConfigureConstructorsAddGenericParameterCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsAddListOfGenericsParameterCommand> getConfigureConstructorsAddListOfGenericsParameterCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsAddListOfLiteralsParameterCommand> getConfigureConstructorsAddListOfLiteralsParameterCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsAddListOfObjectsParameterCommand> getConfigureConstructorsAddListOfObjectsParameterCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsAddLiteralParameterCommand> getConfigureConstructorsAddLiteralParameterCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsAddObjectParameterCommand> getConfigureConstructorsAddObjectParameterCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsCopyXmlCommand> getConfigureConstructorsCopyXmlCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsCutCommand> getConfigureConstructorsCutCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsDeleteCommand> getConfigureConstructorsDeleteCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsHelperCommand> getConfigureConstructorsHelperCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsImportCommand> getConfigureConstructorsImportCommand,
            Func<IConfigureConstructorsForm, ConfigureConstructorsPasteCommand> getConfigureConstructorsPasteCommand)
        {
            _getConfigureConstructorsAddConstructorCommand = getConfigureConstructorsAddConstructorCommand;
            _getConfigureConstructorsAddFolderCommand = getConfigureConstructorsAddFolderCommand;
            _getConfigureConstructorsAddGenericParameterCommand = getConfigureConstructorsAddGenericParameterCommand;
            _getConfigureConstructorsAddListOfGenericsParameterCommand = getConfigureConstructorsAddListOfGenericsParameterCommand;
            _getConfigureConstructorsAddListOfLiteralsParameterCommand = getConfigureConstructorsAddListOfLiteralsParameterCommand;
            _getConfigureConstructorsAddListOfObjectsParameterCommand = getConfigureConstructorsAddListOfObjectsParameterCommand;
            _getConfigureConstructorsAddLiteralParameterCommand = getConfigureConstructorsAddLiteralParameterCommand;
            _getConfigureConstructorsAddObjectParameterCommand = getConfigureConstructorsAddObjectParameterCommand;
            _getConfigureConstructorsCopyXmlCommand = getConfigureConstructorsCopyXmlCommand;
            _getConfigureConstructorsCutCommand = getConfigureConstructorsCutCommand;
            _getConfigureConstructorsDeleteCommand = getConfigureConstructorsDeleteCommand;
            _getConfigureConstructorsHelperCommand = getConfigureConstructorsHelperCommand;
            _getConfigureConstructorsImportCommand = getConfigureConstructorsImportCommand;
            _getConfigureConstructorsPasteCommand = getConfigureConstructorsPasteCommand;
        }

        public ConfigureConstructorsAddConstructorCommand GetConfigureConstructorsAddConstructorCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsAddConstructorCommand(configureConstructorsForm);

        public ConfigureConstructorsAddFolderCommand GetConfigureConstructorsAddFolderCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsAddFolderCommand(configureConstructorsForm);

        public ConfigureConstructorsAddGenericParameterCommand GetConfigureConstructorsAddGenericParameterCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsAddGenericParameterCommand(configureConstructorsForm);

        public ConfigureConstructorsAddListOfGenericsParameterCommand GetConfigureConstructorsAddListOfGenericsParameterCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsAddListOfGenericsParameterCommand(configureConstructorsForm);

        public ConfigureConstructorsAddListOfLiteralsParameterCommand GetConfigureConstructorsAddListOfLiteralsParameterCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsAddListOfLiteralsParameterCommand(configureConstructorsForm);

        public ConfigureConstructorsAddListOfObjectsParameterCommand GetConfigureConstructorsAddListOfObjectsParameterCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsAddListOfObjectsParameterCommand(configureConstructorsForm);

        public ConfigureConstructorsAddLiteralParameterCommand GetConfigureConstructorsAddLiteralParameterCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsAddLiteralParameterCommand(configureConstructorsForm);

        public ConfigureConstructorsAddObjectParameterCommand GetConfigureConstructorsAddObjectParameterCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsAddObjectParameterCommand(configureConstructorsForm);

        public ConfigureConstructorsCopyXmlCommand GetConfigureConstructorsCopyXmlCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsCopyXmlCommand(configureConstructorsForm);

        public ConfigureConstructorsCutCommand GetConfigureConstructorsCutCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsCutCommand(configureConstructorsForm);

        public ConfigureConstructorsDeleteCommand GetConfigureConstructorsDeleteCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsDeleteCommand(configureConstructorsForm);

        public ConfigureConstructorsHelperCommand GetConfigureConstructorsHelperCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsHelperCommand(configureConstructorsForm);

        public ConfigureConstructorsImportCommand GetConfigureConstructorsImportCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsImportCommand(configureConstructorsForm);

        public ConfigureConstructorsPasteCommand GetConfigureConstructorsPasteCommand(IConfigureConstructorsForm configureConstructorsForm)
            => _getConfigureConstructorsPasteCommand(configureConstructorsForm);
    }
}
