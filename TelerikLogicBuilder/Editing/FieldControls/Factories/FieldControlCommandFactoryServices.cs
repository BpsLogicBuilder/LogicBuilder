using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FieldControlCommandFactoryServices
    {
        internal static IServiceCollection AddFieldControlCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConstructorGenericParametersControl, AddUpdateConstructorGenericArgumentsCommand>>
                (
                    provider =>
                    genericParametersControl => new AddUpdateConstructorGenericArgumentsCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IGenericConfigManager>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        genericParametersControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, ClearRichInputBoxTextCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new ClearRichInputBoxTextCommand
                    (
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, CopyRichInputBoxTextCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new CopyRichInputBoxTextCommand
                    (
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, CutRichInputBoxTextCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new CutRichInputBoxTextCommand
                    (
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, DeleteRichInputBoxTextCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new DeleteRichInputBoxTextCommand
                    (
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxConstructorCommand>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new EditObjectRichTextBoxConstructorCommand
                    (
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxFunctionCommand>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new EditObjectRichTextBoxFunctionCommand
                    (
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxLiteralListCommand>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new EditObjectRichTextBoxLiteralListCommand
                    (
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxObjectListCommand>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new EditObjectRichTextBoxObjectListCommand
                    (
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, EditObjectRichTextBoxVariableCommand>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new EditObjectRichTextBoxVariableCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, EditRichInputBoxConstructorCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new EditRichInputBoxConstructorCommand
                    (
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, EditRichInputBoxFunctionCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new EditRichInputBoxFunctionCommand
                    (
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, EditRichInputBoxVariableCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new EditRichInputBoxVariableCommand
                    (
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<IFieldControlCommandFactory, FieldControlCommandFactory>()
                .AddTransient<Func<IRichInputBoxValueControl, PasteRichInputBoxTextCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new PasteRichInputBoxTextCommand
                    (
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IDomainRichInputBoxValueControl, SelectDomainItemCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new SelectDomainItemCommand
                    (
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IPropertyInputRichInputBoxControl, SelectItemFromPropertyListCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new SelectItemFromPropertyListCommand
                    (
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IPropertyInputRichInputBoxControl, SelectItemFromReferencesTreeViewCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new SelectItemFromReferencesTreeViewCommand
                    (
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, ToCamelCaseRichInputBoxCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new ToCamelCaseRichInputBoxCommand
                    (
                        provider.GetRequiredService<IStringHelper>(),
                        richInputBoxValueControl
                    )
                );
        }
    }
}
