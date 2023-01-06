using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseServices
    {
        internal static IServiceCollection AddIntellisense(this IServiceCollection services)
            => services
                .AddSingleton<IIntellisenseHelper, IntellisenseHelper>()
                .AddIntellisenseConstructors()
                .AddIntellisenseCustomConfigurationFactories()
                .AddIntellisenseFunctions()
                .AddIntellisenseGenericArguments()
                .AddIntellisenseHelperStatusBuilderServices()
                .AddIntellisenseParameters()
                .AddIntellisenseFactories()
                .AddIntellisenseTreeNodeFactories()
                .AddIntellisenseVariables();
    }
}
