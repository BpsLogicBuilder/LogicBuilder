using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FieldControlHelperFactoryServices
    {
        internal static IServiceCollection AddFieldControlHelperFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IRichInputBoxValueControl, IConnectorTextRichInputBoxEventsHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new ConnectorTextRichInputBoxEventsHelper
                    (
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, IEditLiteralConstructorHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new EditLiteralConstructorHelper
                    (
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IConstructorTypeHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, IEditLiteralFunctionHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new EditLiteralFunctionHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, IEditLiteralVariableHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new EditLiteralVariableHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, IEditObjectConstructorHelper>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new EditObjectConstructorHelper
                    (
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IConstructorTypeHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, IEditObjectFunctionHelper>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new EditObjectFunctionHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IParameterRichTextBoxValueControl, IEditParameterLiteralListHelper>>
                (
                    provider =>
                    parameterRichTextBoxValueControl => new EditParameterLiteralListHelper
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        parameterRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IParameterRichTextBoxValueControl, IEditParameterObjectListHelper>>
                (
                    provider =>
                    parameterRichTextBoxValueControl => new EditParameterObjectListHelper
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        parameterRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, IEditObjectVariableHelper>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new EditObjectVariableHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IVariableRichTextBoxValueControl, IEditVariableLiteralListHelper>>
                (
                    provider =>
                    variableRichTextBoxValueControl => new EditVariableLiteralListHelper
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        variableRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IVariableRichTextBoxValueControl, IEditVariableObjectListHelper>>
                (
                    provider =>
                    variableRichTextBoxValueControl => new EditVariableObjectListHelper
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        variableRichTextBoxValueControl
                    )
                )
                .AddTransient<IFieldControlHelperFactory, FieldControlHelperFactory>()
                .AddTransient<Func<IRichInputBoxValueControl, ILiteralListItemRichInputBoxEventsHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new LiteralListItemRichInputBoxEventsHelper
                    (
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IParameterRichTextBoxValueControl, IParameterObjectRichTextBoxEventsHelper>>
                (
                    provider =>
                    parameterRichTextBoxValueControl => new ParameterObjectRichTextBoxEventsHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        parameterRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IParameterRichInputBoxValueControl, IParameterRichInputBoxEventsHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new ParameterRichInputBoxEventsHelper
                    (
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, IRichInputBoxEventsHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new RichInputBoxEventsHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, IUpdateObjectRichTextBoxXml>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new UpdateObjectRichTextBoxXml
                    (
                        provider.GetRequiredService<IConstructorElementValidator>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionElementValidator>(),
                        provider.GetRequiredService<ILiteralListElementValidator>(),
                        provider.GetRequiredService<IObjectListElementValidator>(),
                        provider.GetRequiredService<IVariableElementValidator>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IVariableRichTextBoxValueControl, IVariableObjectRichTextBoxEventsHelper>>
                (
                    provider =>
                    variableRichTextBoxValueControl => new VariableObjectRichTextBoxEventsHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        variableRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IVariableRichInputBoxValueControl, IVariableRichInputBoxEventsHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new VariableRichInputBoxEventsHelper
                    (
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        richInputBoxValueControl
                    )
                );
        }
    }
}
