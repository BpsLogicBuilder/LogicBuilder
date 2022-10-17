using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigurationServices
    {
        internal static IServiceCollection AddConfiguration(this IServiceCollection services)
            => services
                .AddSingleton<IApplicationXmlParser, ApplicationXmlParser>()
                .AddSingleton<IBuiltInFunctionsLoader, BuiltInFunctionsLoader>()
                .AddSingleton<IConfigurationService, ConfigurationService>()
                .AddSingleton<ICreateConstructors, CreateConstructors>()
                .AddSingleton<ICreateFragments, CreateFragments>()
                .AddSingleton<ICreateFunctions, CreateFunctions>()
                .AddSingleton<ICreateProjectProperties, CreateProjectProperties>()
                .AddSingleton<ICreateVariables, CreateVariables>()
                .AddSingleton<IEditConstructors, EditConstructors>()
                .AddSingleton<IEditFragments, EditFragments>()
                .AddSingleton<IEditFunctions, EditFunctions>()
                .AddSingleton<IEditProjectProperties, EditProjectProperties>()
                .AddSingleton<IEditVariables, EditVariables>()
                .AddSingleton<IFragmentXmlParser, FragmentXmlParser>()
                .AddSingleton<ILoadConstructors, LoadConstructors>()
                .AddSingleton<ILoadFragments, LoadFragments>()
                .AddSingleton<ILoadFunctions, LoadFunctions>()
                .AddSingleton<ILoadProjectProperties, LoadProjectProperties>()
                .AddSingleton<ILoadVariables, LoadVariables>()
                .AddSingleton<IProjectPropertiesXmlParser, ProjectPropertiesXmlParser>()
                .AddSingleton<IUpdateConstructors, UpdateConstructors>()
                .AddSingleton<IUpdateFragments, UpdateFragments>()
                .AddSingleton<IUpdateFunctions, UpdateFunctions>()
                .AddSingleton<IUpdateProjectProperties, UpdateProjectProperties>()
                .AddSingleton<IUpdateVariables, UpdateVariables>()
                .AddSingleton<IWebApiDeploymentXmlParser, WebApiDeploymentXmlParser>()
                .AddConfigurationInitialization()
                .AddConfigurationItemFactories();
    }
}
