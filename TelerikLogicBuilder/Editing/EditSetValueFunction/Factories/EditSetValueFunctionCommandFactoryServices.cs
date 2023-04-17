using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditSetValueFunctionCommandFactoryServices
    {
        internal static IServiceCollection AddEditSetValueFunctionCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditSetValueFunctionCommandFactory, EditSetValueFunctionCommandFactory>()
                .AddTransient<Func<HelperButtonDropDownList, SelectVariableCommand>>
                (
                    provider =>
                    helperButtonDropDownList => new SelectVariableCommand
                    (
                        helperButtonDropDownList
                    )
                );
        }
    }
}
