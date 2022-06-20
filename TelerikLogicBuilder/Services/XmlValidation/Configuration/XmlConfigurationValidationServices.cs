using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class XmlConfigurationValidationServices
    {
        internal static IServiceCollection AddXmlConfigurationValidation(this IServiceCollection services) 
            => services
                .AddSingleton<IConnectorDataXmlValidator, ConnectorDataXmlValidator>()
                .AddSingleton<IConstructorsXmlValidator, ConstructorsXmlValidator>()
                .AddSingleton<IFunctionsXmlValidator, FunctionsXmlValidator>()
                .AddSingleton<IVariablesXmlValidator, VariablesXmlValidator>();
    }
}
