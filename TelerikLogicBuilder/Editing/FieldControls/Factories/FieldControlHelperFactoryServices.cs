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
                .AddTransient<Func<IRichInputBoxValueControl, IEditLiteralConstructorHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new EditLiteralConstructorHelper
                    (
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IConstructorTypeHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
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
                    richInputBoxValueControl => new EditObjectConstructorHelper
                    (
                        provider.GetRequiredService<IConstructorDataParser>(),
                        provider.GetRequiredService<IConstructorTypeHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IRefreshVisibleTextHelper>(),
                        provider.GetRequiredService<IXmlDataHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, IEditObjectVariableHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new EditObjectVariableHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<IFieldControlHelperFactory, FieldControlHelperFactory>()
                .AddTransient<Func<IObjectRichTextBoxValueControl, IObjectRichTextBoxEventsHelper>>
                (
                    provider =>
                    objectRichTextBoxValueControl => new ObjectRichTextBoxEventsHelper
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectRichTextBoxValueControl
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
                );
        }
    }
}
