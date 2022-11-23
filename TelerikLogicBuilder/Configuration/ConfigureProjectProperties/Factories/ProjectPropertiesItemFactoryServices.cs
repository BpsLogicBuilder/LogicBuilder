using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal static class ProjectPropertiesItemFactoryServices
    {
        internal static IServiceCollection AddProjectPropertiesItemFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<string, string, string, string, RuntimeType, List<string>, string, string, string, List<string>, string, string, string, string, List<string>, WebApiDeployment, Application>>
                (
                    provider =>
                    (name, nickname, activityAssembly, activityAssemblyPath, runtime, loadAssemblyPaths, activityClass, applicationExcecutable, applicationExcecutablePath, startupArguments, resourceFile, resourceFileDeploymentPath, rulesFile, rulesDeploymentPath, modules, webApiDeployment) => new Application
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name,
                        nickname,
                        activityAssembly,
                        activityAssemblyPath,
                        runtime,
                        loadAssemblyPaths,
                        activityClass,
                        applicationExcecutable,
                        applicationExcecutablePath,
                        startupArguments,
                        resourceFile,
                        resourceFileDeploymentPath,
                        rulesFile,
                        rulesDeploymentPath,
                        modules,
                        webApiDeployment
                    )
                )
                .AddTransient<IProjectPropertiesItemFactory, ProjectPropertiesItemFactory>()
                .AddTransient<Func<string, string, Dictionary<string, Application>, HashSet<string>, ProjectProperties>>
                (
                    provider =>
                    (projectName, projectPath, applicationList, connectorObjectTypes) => new ProjectProperties
                    (
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        projectName,
                        projectPath,
                        applicationList,
                        connectorObjectTypes
                    )
                );
        }
    }
}
