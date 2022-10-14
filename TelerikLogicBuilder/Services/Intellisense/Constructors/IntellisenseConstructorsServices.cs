using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseConstructorsServices
    {
        internal static IServiceCollection AddIntellisenseConstructors(this IServiceCollection services)
            => services
                .AddSingleton<IConstructorManager, ConstructorManager>()
                .AddSingleton<IConstructorXmlParser, ConstructorXmlParser>()
                .AddSingleton<IExistingConstructorFinder, ExistingConstructorFinder>()
                .AddIntellisenseConstructorFactories();
    }
}
