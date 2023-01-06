using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseHelperStatusBuilderServices
    {
        internal static IServiceCollection AddIntellisenseHelperStatusBuilderServices(this IServiceCollection services)
        {
            return services
                .AddSingleton<IFunctionNodeBuilder, FunctionNodeBuilder>()
                .AddSingleton<IReferenceInfoListBuilder, ReferenceInfoListBuilder>()
                .AddHelperStatusBuilderFactories();
        }
    }
}
