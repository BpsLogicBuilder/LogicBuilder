using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseFunctionsServices
    {
        internal static IServiceCollection AddIntellisenseFunctions(this IServiceCollection services)
            => services
                .AddSingleton<IFunctionHelper, FunctionHelper>()
                .AddSingleton<IFunctionManager, FunctionManager>()
                .AddSingleton<IFunctionNodeInfoManager, FunctionNodeInfoManager>()
                .AddSingleton<IFunctionValidationHelper, FunctionValidationHelper>()
                .AddSingleton<IFunctionXmlParser, FunctionXmlParser>()
                .AddSingleton<IReturnTypeManager, ReturnTypeManager>()
                .AddSingleton<IReturnTypeManager, ReturnTypeManager>()
                .AddSingleton<IReturnTypeXmlParser, ReturnTypeXmlParser>()
                .AddIntellisenseReturnTypeInfos()
                .AddIntellisenseReturnTypes();
    }
}
