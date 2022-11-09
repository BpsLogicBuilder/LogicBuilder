using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls;
using ABIS.LogicBuilder.FlowBuilder.Configuration.UserControls.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ApplicationControlCommandFactoryServices
    {
        internal static IServiceCollection AddApplicationControlCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IApplicationControl, EditActivityAssemblyCommand>>
                (
                    provider =>
                    applicationControl => new EditActivityAssemblyCommand
                    (
                        provider.GetRequiredService<IPathHelper>(),
                        applicationControl
                    )
                )
                .AddTransient<Func<IApplicationControl, EditActivityAssemblyPathCommand>>
                (
                    provider =>
                    applicationControl => new EditActivityAssemblyPathCommand
                    (
                        applicationControl
                    )
                )
                .AddTransient<Func<IApplicationControl, EditExcludedModulesCommand>>
                (
                    provider =>
                    applicationControl => new EditExcludedModulesCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        applicationControl
                    )
                )
                .AddTransient<Func<IApplicationControl, EditLoadAssemblyPathsCommand>>
                (
                    provider =>
                    applicationControl => new EditLoadAssemblyPathsCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        applicationControl
                    )
                )
                .AddTransient<Func<IApplicationControl, EditResourceFilesDeploymentCommand>>
                (
                    provider =>
                    applicationControl => new EditResourceFilesDeploymentCommand
                    (
                        applicationControl
                    )
                )
                .AddTransient<Func<IApplicationControl, EditRulesDeploymentCommand>>
                (
                    provider =>
                    applicationControl => new EditRulesDeploymentCommand
                    (
                        applicationControl
                    )
                )
                .AddTransient<Func<IApplicationControl, EditWebApiDeploymentCommand>>
                (
                    provider =>
                    applicationControl => new EditWebApiDeploymentCommand
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IWebApiDeploymentXmlParser>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        applicationControl
                    )
                )
                .AddTransient<IApplicationControlCommandFactory, ApplicationControlCommandFactory>();
        }
    }
}
