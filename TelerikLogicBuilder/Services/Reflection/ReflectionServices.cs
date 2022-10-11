using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ReflectionServices
    {
        internal static IServiceCollection AddReflection(this IServiceCollection services)
            => services
                .AddSingleton<IApplicationTypeInfoManager, ApplicationTypeInfoManager>()
                .AddSingleton<IAssemblyHelper, AssemblyHelper>()
                .AddSingleton<IAssemblyLoader, AssemblyLoader>()
                .AddSingleton<IApplicationTypeInfoHelper, ApplicationTypeInfoHelper>()
                .AddSingleton<ILoadContextSponsor, LoadContextSponsor>()
                .AddSingleton<IReflectionHelper, ReflectionHelper>();
    }
}
