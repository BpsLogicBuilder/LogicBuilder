using ABIS.LogicBuilder.FlowBuilder.Editing.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormCommandFactory : IEditingFormCommandFactory
    {
        private readonly Func<IDataGraphEditingHost, AddXMLToFragmentsConfigurationCommand> _getAddXMLToFragmentsConfigurationCommand;
        private readonly Func<IDataGraphEditingHost, CopyXmlToClipboardCommand> _getCopyXmlToClipboardCommand;

        public EditingFormCommandFactory(
            Func<IDataGraphEditingHost, AddXMLToFragmentsConfigurationCommand> getAddXMLToFragmentsConfigurationCommand,
            Func<IDataGraphEditingHost, CopyXmlToClipboardCommand> getCopyXmlToClipboardCommand)
        {
            _getAddXMLToFragmentsConfigurationCommand = getAddXMLToFragmentsConfigurationCommand;
            _getCopyXmlToClipboardCommand = getCopyXmlToClipboardCommand;
        }

        public AddXMLToFragmentsConfigurationCommand GetAddXMLToFragmentsConfigurationCommand(IDataGraphEditingHost dataGraphEditingHost)
            => _getAddXMLToFragmentsConfigurationCommand(dataGraphEditingHost);

        public CopyXmlToClipboardCommand GetCopyXmlToClipboardCommand(IDataGraphEditingHost dataGraphEditingHost)
            => _getCopyXmlToClipboardCommand(dataGraphEditingHost);
    }
}
