using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditGenericArgumentsCommandFactoryServices
    {
        internal static IServiceCollection AddEditGenericArgumentsCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditGenericArgumentsControl, AddGenericArgumentCommand>>
                (
                    provider =>
                    editGenericArgumentsControl => new AddGenericArgumentCommand
                    (
                        editGenericArgumentsControl
                    )
                )
                .AddTransient<IEditGenericArgumentsCommandFactory, EditGenericArgumentsCommandFactory>()
                .AddTransient<Func<IEditGenericArgumentsControl, UpdateGenericArgumentCommand>>
                (
                    provider =>
                    editGenericArgumentsControl => new UpdateGenericArgumentCommand
                    (
                        editGenericArgumentsControl
                    )
                );
        }
    }
}
