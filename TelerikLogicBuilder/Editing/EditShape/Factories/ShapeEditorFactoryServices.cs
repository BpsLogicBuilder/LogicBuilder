using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditShape;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditShape.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System;
using System.Globalization;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ShapeEditorFactoryServices
    {
        internal static IServiceCollection AddShapeEditorFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IActionShapeEditor, ActionShapeEditor>()
                .AddTransient<IDialogShapeEditor, DialogShapeEditor>()
                .AddTransient<Func<string, IShapeEditor>>
                (
                    provider =>
                    masterName =>
                    {
                        return masterName switch
                        {
                            UniversalMasterName.ACTION => provider.GetRequiredService<IActionShapeEditor>(),
                            UniversalMasterName.DIALOG => provider.GetRequiredService<IDialogShapeEditor>(),
                            _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{EB5FFEF4-2266-4569-A0DA-A2C6E30574B0}")),
                        };
                    })
                .AddTransient<IShapeEditorFactory, ShapeEditorFactory>();
        }
    }
}
