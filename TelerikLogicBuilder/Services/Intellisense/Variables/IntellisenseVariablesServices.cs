using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseVariablesServices
    {
        internal static IServiceCollection AddIntellisenseVariables(this IServiceCollection services)
            => services
                .AddSingleton<IVariableHelper, VariableHelper>()
                .AddSingleton<IVariablesManager, VariablesManager>()
                .AddSingleton<IVariablesNodeInfoManager, VariablesNodeInfoManager>()
                .AddSingleton<IVariablesXmlParser, VariablesXmlParser>()
                .AddSingleton<IVariableValidationHelper, VariableValidationHelper>()
                .AddIntellisenseVariableFactories();
    }
}
