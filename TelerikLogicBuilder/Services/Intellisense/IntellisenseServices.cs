namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseServices
    {
        internal static IServiceCollection AddIntellisense(this IServiceCollection services)
            => services
                .AddIntellisenseConstructors()
                .AddIntellisenseFunctions()
                .AddIntellisenseGenericArguments()
                .AddIntellisenseParameters()
                .AddIntellisenseVariables();
    }
}
