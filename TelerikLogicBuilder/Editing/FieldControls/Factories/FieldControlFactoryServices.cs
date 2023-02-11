using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FieldControlFactoryServices
    {
        internal static IServiceCollection AddFieldControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IFieldControlFactory, FieldControlFactory>()
                .AddTransient<Func<IEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl>>
                (
                    provider =>
                    (editigControl, literalParameter) => new LiteralParameterRichInputBoxControl
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IFieldControlCommandFactory>(),
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ILayoutFieldControlButtons>(),
                        provider.GetRequiredService<IUpdateRichInputBoxXml>(),
                        provider.GetRequiredService<RichInputBox>(),
                        editigControl, 
                        literalParameter
                    )
                );
        }
    }
}
