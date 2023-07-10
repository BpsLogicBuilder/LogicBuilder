using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingFormCommandFactory : IEditingFormCommandFactory
    {
        public AddXMLToFragmentsConfigurationCommand GetAddXMLToFragmentsConfigurationCommand(IDataGraphEditingHost dataGraphEditingHost)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IFragmentItemFactory>(),
                Program.ServiceProvider.GetRequiredService<IFragmentListInitializer>(),
                Program.ServiceProvider.GetRequiredService<ILoadFragments>(),
                Program.ServiceProvider.GetRequiredService<IStringHelper>(),
                Program.ServiceProvider.GetRequiredService<IUpdateFragments>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                dataGraphEditingHost
            );

        public CopyXmlToClipboardCommand GetCopyXmlToClipboardCommand(IDataGraphEditingHost dataGraphEditingHost)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                dataGraphEditingHost
            );
    }
}
