using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FieldControlCommandFactoryServices
    {
        internal static IServiceCollection AddFieldControlCommandFactories(this IServiceCollection services)
        {
            return services
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
                .AddTransient<Func<ILiteralParameterDomainRichInputBoxControl, SelectDomainItemCommand>>
                (
                    provider =>
                    richInputBoxValueControl => new SelectDomainItemCommand
                    (
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
