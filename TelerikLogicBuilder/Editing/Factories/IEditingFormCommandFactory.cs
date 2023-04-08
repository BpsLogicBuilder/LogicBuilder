using ABIS.LogicBuilder.FlowBuilder.Editing.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingFormCommandFactory
    {
        AddXMLToFragmentsConfigurationCommand GetAddXMLToFragmentsConfigurationCommand(IDataGraphEditingHost dataGraphEditingHost);
        CopyXmlToClipboardCommand GetCopyXmlToClipboardCommand(IDataGraphEditingHost dataGraphEditingHost);
    }
}
