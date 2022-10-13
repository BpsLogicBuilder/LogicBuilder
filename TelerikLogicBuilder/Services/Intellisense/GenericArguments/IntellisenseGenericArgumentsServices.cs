using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.GenericArguments;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseGenericArgumentsServices
    {
        internal static IServiceCollection AddIntellisenseGenericArguments(this IServiceCollection services)
            => services
                .AddSingleton<IGenericConfigXmlParser, GenericConfigXmlParser>()
                .AddIntellisenseGenericConfigFactories();
    }
}
