using ABIS.LogicBuilder.FlowBuilder;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditCell;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditCell.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System;
using System.Globalization;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class CellEditorFactoryServices
    {
        internal static IServiceCollection AddCellEditorFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<ICellEditorFactory, CellEditorFactory>()
                .AddTransient<IConditionsCellEditor, ConditionsCellEditor>()
                .AddTransient<IFunctionsCellEditor, FunctionsCellEditor>()
                .AddTransient<IPriorityCellEditor, PriorityCellEditor>();
        }
    }
}
