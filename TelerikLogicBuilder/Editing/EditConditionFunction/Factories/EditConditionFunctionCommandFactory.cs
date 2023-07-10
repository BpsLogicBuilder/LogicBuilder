using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunction.Factories
{
    internal class EditConditionFunctionCommandFactory : IEditConditionFunctionCommandFactory
    {
        public SelectConditionFunctionCommand GetSelectConditionFunctionCommand(IEditConditionFunctionControl editConditionFunctionControl)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                editConditionFunctionControl
            );
    }
}
