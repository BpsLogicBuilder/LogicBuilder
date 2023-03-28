using ABIS.LogicBuilder.FlowBuilder.Editing.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingFormCommandFactory
    {
        AddXMLToFragmentsConfigurationCommand GetAddXMLToFragmentsConfigurationCommand(IDataGraphEditingForm dataGraphEditingForm);
        CopyXmlToClipboardCommand GetCopyXmlToClipboardCommand(IDataGraphEditingForm dataGraphEditingForm);
    }
}
