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
                .AddTransient<Func<IDataGraphEditingForm, AddXMLToFragmentsConfigurationCommand>>
                (
                    provider =>
                    dataGraphEditingForm => new AddXMLToFragmentsConfigurationCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IFragmentItemFactory>(),
                        provider.GetRequiredService<IFragmentListInitializer>(),
                        provider.GetRequiredService<ILoadFragments>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<IUpdateFragments>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        dataGraphEditingForm
                    )
                )
                .AddTransient<Func<IDataGraphEditingForm, CopyXmlToClipboardCommand>>
                (
                    provider =>
                    dataGraphEditingForm => new CopyXmlToClipboardCommand
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        dataGraphEditingForm
                    )
                )
                .AddTransient<IEditingFormCommandFactory, EditingFormCommandFactory>();
        }
    }
}
