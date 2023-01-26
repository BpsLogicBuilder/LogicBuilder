using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories
{
    internal interface IConfigureFragmentsCommandFactory
    {
        ConfigureFragmentsAddFolderCommand GetConfigureFragmentsAddFolderCommand(IConfigureFragmentsForm configureFragmentsForm);
        ConfigureFragmentsAddFragmentCommand GetConfigureFragmentsAddFragmentCommand(IConfigureFragmentsForm configureFragmentsForm);
        ConfigureFragmentsCopyXmlCommand GetConfigureFragmentsCopyXmlCommand(IConfigureFragmentsForm configureFragmentsForm);
        ConfigureFragmentsCutCommand GetConfigureFragmentsCutCommand(IConfigureFragmentsForm configureFragmentsForm);
        ConfigureFragmentsDeleteCommand GetConfigureFragmentsDeleteCommand(IConfigureFragmentsForm configureFragmentsForm);
        ConfigureFragmentsImportCommand GetConfigureFragmentsImportCommand(IConfigureFragmentsForm configureFragmentsForm);
        ConfigureFragmentsPasteCommand GetConfigureFragmentsPasteCommand(IConfigureFragmentsForm configureFragmentsForm);
    }
}
