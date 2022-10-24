using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationItemFactoryServices
    {
        internal static IServiceCollection AddConfigurationItemFactories(this IServiceCollection services)
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
                .AddTransient<IConfigurationItemFactory, ConfigurationItemFactory>()
                .AddTransient<Func<string, string, Fragment>>
                (
                    provider =>
                    (name, xml) => new Fragment
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        name, 
                        xml
                    )
                )
                .AddTransient<Func<string, string, Dictionary<string, Application>, HashSet<string>, ProjectProperties>>
                (
                    provider =>
                    (projectName, projectPath, applicationList, connectorObjectTypes) => new ProjectProperties
                    (
                        provider.GetRequiredService<IFileIOHelper>(),
                        provider.GetRequiredService<IPathHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        projectName,
                        projectPath,
                        applicationList,
                        connectorObjectTypes
                    )
                )
                .AddTransient<Func<string, string, string, string, WebApiDeployment>>
                (
                    provider =>
                    (postFileDataUrl, postVariablesMetaUrl, deleteRulesUrl, deleteAllRulesUrl) => new WebApiDeployment
                    (
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        postFileDataUrl,
                        postVariablesMetaUrl,
                        deleteRulesUrl,
                        deleteAllRulesUrl
                    )
                );
        }
    }
}
