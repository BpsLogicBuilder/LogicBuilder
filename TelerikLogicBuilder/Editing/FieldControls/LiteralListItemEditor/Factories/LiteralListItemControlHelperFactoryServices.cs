using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.LiteralListItemEditor.Factories
{
    internal static class LiteralListItemControlHelperFactoryServices
    {
        internal static IServiceCollection AddLiteralListItemControlHelperFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<ILiteralListItemControlHelperFactory, LiteralListItemControlHelperFactory>()
                .AddTransient<Func<IRichInputBoxValueControl, ILiteralListItemRichInputBoxEventsHelper>>
                (
                    provider =>
                    richInputBoxValueControl => new LiteralListItemRichInputBoxEventsHelper
                    (
                        provider.GetRequiredService<IFieldControlHelperFactory>(),
                        richInputBoxValueControl
                    )
                );
        }
    }
}
