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
                .AddTransient<Func<int, ICellEditor>>
                (
                    provider =>
                    column =>
                    {
                        return column switch
                        {
                            TableColumns.ACTIONCOLUMNINDEX => provider.GetRequiredService<IFunctionsCellEditor>(),
                            TableColumns.CONDITIONCOLUMNINDEX => provider.GetRequiredService<IConditionsCellEditor>(),
                            TableColumns.PRIORITYCOLUMNINDEX => provider.GetRequiredService<IPriorityCellEditor>(),
                            _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{076C7466-109F-4DEA-9C6B-E6971D5F3186}")),
                        };
                    }
                )
                .AddTransient<ICellEditorFactory, CellEditorFactory>()
                .AddTransient<IConditionsCellEditor, ConditionsCellEditor>()
                .AddTransient<IFunctionsCellEditor, FunctionsCellEditor>()
                .AddTransient<IPriorityCellEditor, PriorityCellEditor>();
        }
    }
}
