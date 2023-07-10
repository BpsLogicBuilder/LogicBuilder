using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditDialogFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditDialogFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddSingleton<IEditDialogFunctionCommandFactory, EditDialogFunctionCommandFactory>();
        }
    }
}
