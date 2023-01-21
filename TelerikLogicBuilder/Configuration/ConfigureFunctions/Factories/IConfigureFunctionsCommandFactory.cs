using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Factories
{
    internal interface IConfigureFunctionsCommandFactory
    {
        ConfigureFunctionsAddBinaryOperatorCommand GetConfigureFunctionsAddBinaryOperatorCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddClassMembersCommand GetConfigureFunctionsAddClassMembersCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddDialogFunctionCommand GetConfigureFunctionsAddDialogFunctionCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddFolderCommand GetConfigureFunctionsAddFolderCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddGenericParameterCommand GetConfigureFunctionsAddGenericParameterCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddListOfGenericsParameterCommand GetConfigureFunctionsAddListOfGenericsParameterCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddListOfLiteralsParameterCommand GetConfigureFunctionsAddListOfLiteralsParameterCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddListOfObjectsParameterCommand GetConfigureFunctionsAddListOfObjectsParameterCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddLiteralParameterCommand GetConfigureFunctionsAddLiteralParameterCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddObjectParameterCommand GetConfigureFunctionsAddObjectParameterCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsAddStandardFunctionCommand GetConfigureFunctionsAddStandardFunctionCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsCopyXmlCommand GetConfigureFunctionsCopyXmlCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsCutCommand GetConfigureFunctionsCutCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsDeleteCommand GetConfigureFunctionsDeleteCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsHelperCommand GetConfigureFunctionsHelperCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsImportCommand GetConfigureFunctionsImportCommand(IConfigureFunctionsForm configureFunctionsForm);
        ConfigureFunctionsPasteCommand GetConfigureFunctionsPasteCommand(IConfigureFunctionsForm configureFunctionsForm);
    }
}
