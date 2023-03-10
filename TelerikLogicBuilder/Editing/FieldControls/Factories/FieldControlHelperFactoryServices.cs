using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
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
                .AddTransient<Func<IRichInputBoxValueControl, ICreateRichInputBoxContextMenu>>
                (
                    provider =>
                    richInputBoxValueControl => new CreateRichInputBoxContextMenu
                    (
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IObjectRichTextBoxValueControl, IEditObjectVariableHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new EditObjectVariableHelper
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        richInputBoxValueControl
                    )
                )
                .AddTransient<Func<IRichInputBoxValueControl, IEditVariableHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new EditVariableHelper
                    (
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
                        provider.GetRequiredService<IVariableDataParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        objectRichTextBoxValueControl
                    )
                )
                .AddTransient<Func<IParameterRichInputBoxValueControl, IParameterRichInputBoxEventsHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new ParameterRichInputBoxEventsHelper
                    (
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
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
