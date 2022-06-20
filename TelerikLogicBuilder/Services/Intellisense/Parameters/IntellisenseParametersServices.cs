using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Parameters;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseParametersServices
    {
        internal static IServiceCollection AddIntellisenseParameters(this IServiceCollection services)
            => services
                .AddSingleton<IMultipleChoiceParameterValidator, MultipleChoiceParameterValidator>()
                .AddSingleton<IParameterHelper, ParameterHelper>()
                .AddSingleton<IParametersManager, ParametersManager>()
                .AddSingleton<IParametersMatcher, ParametersMatcher>()
                .AddSingleton<IParametersXmlParser, ParametersXmlParser>();
    }
}
