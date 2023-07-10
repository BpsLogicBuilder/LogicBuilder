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
                .AddTransient<IConditionShapeEditor, ConditionShapeEditor>()
                .AddTransient<IConnectorShapeEditor, ConnectorShapeEditor>()
                .AddTransient<IDecisionShapeEditor, DecisionShapeEditor>()
                .AddTransient<IDialogShapeEditor, DialogShapeEditor>()
                .AddTransient<IJumpShapeEditor, JumpShapeEditor>()
                .AddTransient<IModuleShapeEditor, ModuleShapeEditor>()
                .AddSingleton<IShapeEditorFactory, ShapeEditorFactory>();
        }
    }
}
