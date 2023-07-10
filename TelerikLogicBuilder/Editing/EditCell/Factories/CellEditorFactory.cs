using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditCell.Factories
{
    internal class CellEditorFactory : ICellEditorFactory
    {
        public ICellEditor GetCellEditor(int column)
            => column switch
            {
                TableColumns.ACTIONCOLUMNINDEX => Program.ServiceProvider.GetRequiredService<IFunctionsCellEditor>(),
                TableColumns.CONDITIONCOLUMNINDEX => Program.ServiceProvider.GetRequiredService<IConditionsCellEditor>(),
                TableColumns.PRIORITYCOLUMNINDEX => Program.ServiceProvider.GetRequiredService<IPriorityCellEditor>(),
                _ => throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{076C7466-109F-4DEA-9C6B-E6971D5F3186}")),
            };
    }
}
