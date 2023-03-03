using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.Services.Data;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DataServices
    {
        internal static IServiceCollection AddData(this IServiceCollection services)
            => services
                .AddSingleton<IAnyParametersHelper, AnyParametersHelper>()
                .AddSingleton<IGenericConstructorHelper, GenericConstructorHelper>()
                .AddSingleton<IGenericFunctionHelper, GenericFunctionHelper>()
                .AddSingleton<IGenericParametersHelper, GenericParametersHelper>()
                .AddSingleton<IGenericReturnTypeHelper, GenericReturnTypeHelper>()
                .AddSingleton<IGetValidConfigurationFromData, GetValidConfigurationFromData>()
                .AddSingleton<ILiteralListParameterElementInfoHelper, LiteralListParameterElementInfoHelper>()
                .AddSingleton<IObjectListElementInfoHelper, ObjectListElementInfoHelper>()
                .AddSingleton<IRefreshVisibleTextHelper, RefreshVisibleTextHelper>()
                .AddSingleton<IUpdateVisibleTextAttribute, UpdateVisibleTextAttribute>();
    }
}
