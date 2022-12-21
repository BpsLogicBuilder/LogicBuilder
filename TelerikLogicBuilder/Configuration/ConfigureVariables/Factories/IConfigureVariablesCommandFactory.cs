using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Factories
{
    internal interface IConfigureVariablesCommandFactory
    {
        AddLiteralListVariableCommand GetAddLiteralListVariableCommand(IConfigureVariablesForm configureVariablesForm);
        AddLiteralVariableCommand GetAddLiteralVariableCommand(IConfigureVariablesForm configureVariablesForm);
        AddObjectListVariableCommand GetAddObjectListVariableCommand(IConfigureVariablesForm configureVariablesForm);
        AddObjectVariableCommand GetAddObjectVariableCommand(IConfigureVariablesForm configureVariablesForm);
        ConfigureVariablesAddFolderCommand GetConfigureVariablesAddFolderCommand(IConfigureVariablesForm configureVariablesForm);
        ConfigureVariablesAddMembersCommand GetConfigureVariablesAddMembersCommand(IConfigureVariablesForm configureVariablesForm);
        ConfigureVariablesCopyXmlCommand GetConfigureVariablesCopyXmlCommand(IConfigureVariablesForm configureVariablesForm);
        ConfigureVariablesCutCommand GetConfigureVariablesCutCommand(IConfigureVariablesForm configureVariablesForm);
        ConfigureVariablesDeleteCommand GetConfigureVariablesDeleteCommand(IConfigureVariablesForm configureVariablesForm);
        ConfigureVariablesHelperCommand GetConfigureVariablesHelperCommand(IConfigureVariablesForm configureVariablesForm);
        ConfigureVariablesImportCommand GetConfigureVariablesImportCommand(IConfigureVariablesForm configureVariablesForm);
        ConfigureVariablesPasteCommand GetConfigureVariablesPasteCommand(IConfigureVariablesForm configureVariablesForm);
    }
}
