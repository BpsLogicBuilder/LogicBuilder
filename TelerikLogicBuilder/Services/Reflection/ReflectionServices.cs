using ABIS.LogicBuilder.FlowBuilder.Reflection.Factories;
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
                .AddSingleton<IApplicationTypeInfoHelper, ApplicationTypeInfoHelper>()
                .AddSingleton<ILoadAssemblyFromPath, LoadAssemblyFromPath>()
                .AddSingleton<ILoadContextSponsor, LoadContextSponsor>()
                .AddSingleton<IReflectionFactory, ReflectionFactory>()
                .AddSingleton<IReflectionHelper, ReflectionHelper>()
                .AddReflectionFactories();
    }
}
