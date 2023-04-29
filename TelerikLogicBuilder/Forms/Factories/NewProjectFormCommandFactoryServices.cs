using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.Forms.Commands;
using ABIS.LogicBuilder.FlowBuilder.Forms.Factories;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class NewProjectFormCommandFactoryServices
    {
        internal static IServiceCollection AddNewProjectFormCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<INewProjectFormCommandFactory,  NewProjectFormCommandFactory>()
                .AddTransient<Func<INewProjectForm, SelectFolderCommand>>
                (
                    provider =>
                    newProjectForm => new SelectFolderCommand
                    (
                        newProjectForm
                    )
                );
        }
    }
}
