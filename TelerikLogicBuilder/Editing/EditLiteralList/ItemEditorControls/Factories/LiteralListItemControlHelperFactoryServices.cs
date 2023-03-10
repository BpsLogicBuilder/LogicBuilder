using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList.ItemEditorControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls;
using System;

namespace Microsoft.Extensions.DependencyInjection
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
                        provider.GetRequiredService<IEditingControlHelperFactory>(),
                        richInputBoxValueControl
                    )
                );
        }
    }
}
