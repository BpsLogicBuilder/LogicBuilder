using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.EditGenericArguments.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class EditGenericArgumentsControlFactoryServices
    {
        internal static IServiceCollection AddEditGenericArgumentsControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IEditGenericArgumentsForm, IEditGenericArgumentsControl>>
                (
                    provider =>
                    editGenericArgumentsForm => new EditGenericArgumentsControl
                    (
                        provider.GetRequiredService<IEditGenericArgumentsCommandFactory>(),
                        editGenericArgumentsForm
                    )
                )
                .AddTransient<IEditGenericArgumentsControlFactory, EditGenericArgumentsControlFactory>();
        }
    }
}
