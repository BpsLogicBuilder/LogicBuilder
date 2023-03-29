using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditXmlFormFactoryServices
    {
        internal static IServiceCollection AddEditXmlFormFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, Type, IEditConstructorFormXml>>
                (
                    provider =>
                    (xml, assignedTo) => new EditConstructorFormXml
                    (
                        provider.GetRequiredService<IConstructorElementValidator>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<RichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml, 
                        assignedTo
                    )
                )
                .AddTransient<Func<string, Type, IEditLiteralListFormXml>>
                (
                    provider =>
                    (xml, assignedTo) => new EditLiteralListFormXml
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<ILiteralListElementValidator>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<RichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml,
                        assignedTo
                    )
                )
                .AddTransient<IEditXmlFormFactory, EditXmlFormFactory>();
        }
    }
}
