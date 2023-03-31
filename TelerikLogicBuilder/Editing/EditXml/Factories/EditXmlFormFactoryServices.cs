using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditXml.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
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
                .AddTransient<Func<string, Type, IEditBooleanFunctionFormXml>>
                (
                    provider =>
                    (xml, assignedTo) => new EditBooleanFunctionFormXml
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionElementValidator>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml,
                        assignedTo
                    )
                )
                .AddTransient<Func<string, IEditBuildDecisionFormXml>>
                (
                    provider =>
                    xml => new EditBuildDecisionFormXml
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IDecisionElementValidator>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml
                    )
                )
                .AddTransient<Func<string, IEditConditionsFormXml>>
                (
                    provider =>
                    xml => new EditConditionsFormXml
                    (
                        provider.GetRequiredService<IConditionsElementValidator>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml
                    )
                )
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
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml, 
                        assignedTo
                    )
                )
                .AddTransient<Func<string, IEditDecisionsFormXml>>
                (
                    provider =>
                    xml => new EditDecisionsFormXml
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IDecisionsElementValidator>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml
                    )
                )
                .AddTransient<Func<string, IEditDialogFunctionFormXml>>
                (
                    provider =>
                    xml => new EditDialogFunctionFormXml
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IFunctionsDataParser>(),
                        provider.GetRequiredService<IFunctionElementValidator>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml
                    )
                )
                .AddTransient<Func<string, IEditFunctionsFormXml>>
                (
                    provider =>
                    xml => new EditFunctionsFormXml
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IFunctionsDataParser>(),
                        provider.GetRequiredService<IFunctionsElementValidator>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml
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
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml,
                        assignedTo
                    )
                )
                .AddTransient<Func<string, Type, IEditObjectListFormXml>>
                (
                    provider =>
                    (xml, assignedTo) => new EditObjectListFormXml
                    (
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IObjectListElementValidator>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml,
                        assignedTo
                    )
                )
                .AddTransient<Func<string, IEditTableFunctionsFormXml>>
                (
                    provider =>
                    xml => new EditTableFunctionsFormXml
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IFunctionsDataParser>(),
                        provider.GetRequiredService<IFunctionsElementValidator>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
                        provider.GetRequiredService<IServiceFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        xml
                    )
                )
                .AddTransient<Func<string, Type, IEditValueFunctionFormXml>>
                (
                    provider =>
                    (xml, assignedTo) => new EditValueFunctionFormXml
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IDialogFormMessageControl>(),
                        provider.GetRequiredService<IEditXmlHelperFactory>(),
                        provider.GetRequiredService<IFormInitializer>(),
                        provider.GetRequiredService<IFunctionDataParser>(),
                        provider.GetRequiredService<IFunctionElementValidator>(),
                        provider.GetRequiredService<IFunctionHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<EditXmlRichTextBoxPanel>(),
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
