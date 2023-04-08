using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditingFormCommandFactoryServices
    {
        internal static IServiceCollection AddEditingFormCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IDataGraphEditingHost, AddXMLToFragmentsConfigurationCommand>>
                (
                    provider =>
                    dataGraphEditingHost => new AddXMLToFragmentsConfigurationCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IFragmentItemFactory>(),
                        provider.GetRequiredService<IFragmentListInitializer>(),
                        provider.GetRequiredService<ILoadFragments>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IUpdateFragments>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        dataGraphEditingHost
                    )
                )
                .AddTransient<Func<IDataGraphEditingHost, CopyXmlToClipboardCommand>>
                (
                    provider =>
                    dataGraphEditingHost => new CopyXmlToClipboardCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingHost
                    )
                )
                .AddTransient<IEditingFormCommandFactory, EditingFormCommandFactory>();
        }
    }
}
