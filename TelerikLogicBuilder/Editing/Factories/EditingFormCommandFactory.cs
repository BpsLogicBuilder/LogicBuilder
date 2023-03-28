using ABIS.LogicBuilder.FlowBuilder.Editing.Commands;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormCommandFactory : IEditingFormCommandFactory
    {
        private readonly Func<IDataGraphEditingForm, AddXMLToFragmentsConfigurationCommand> _getAddXMLToFragmentsConfigurationCommand;
        private readonly Func<IDataGraphEditingForm, CopyXmlToClipboardCommand> _getCopyXmlToClipboardCommand;

        public EditingFormCommandFactory(Func<IDataGraphEditingForm, AddXMLToFragmentsConfigurationCommand> getAddXMLToFragmentsConfigurationCommand, Func<IDataGraphEditingForm, CopyXmlToClipboardCommand> getCopyXmlToClipboardCommand)
        {
            _getAddXMLToFragmentsConfigurationCommand = getAddXMLToFragmentsConfigurationCommand;
            _getCopyXmlToClipboardCommand = getCopyXmlToClipboardCommand;
        }

        public AddXMLToFragmentsConfigurationCommand GetAddXMLToFragmentsConfigurationCommand(IDataGraphEditingForm dataGraphEditingForm)
            => _getAddXMLToFragmentsConfigurationCommand(dataGraphEditingForm);

        public CopyXmlToClipboardCommand GetCopyXmlToClipboardCommand(IDataGraphEditingForm dataGraphEditingForm)
            => _getCopyXmlToClipboardCommand(dataGraphEditingForm);
    }
}
