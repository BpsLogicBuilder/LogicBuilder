using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class XmlValidationServices
    {
        internal static IServiceCollection AddXmlValidation(this IServiceCollection services) 
            => services
                .AddSingleton<IXmlValidator, XmlValidator>()
                .AddXmlConfigurationValidation()
                .AddXmlDataValidation();
    }
}
