using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditConstructorCommandFactoryServices
    {
        internal static IServiceCollection AddEditConstructorCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<IEditConstructorCommandFactory, EditConstructorCommandFactory>()
                .AddTransient<Func<IEditConstructorForm, SelectConstructorCommand>>
                (
                    provider =>
                    editConstructorForm => new SelectConstructorCommand
                    (
                        editConstructorForm
                    )
                );
        }
    }
}
