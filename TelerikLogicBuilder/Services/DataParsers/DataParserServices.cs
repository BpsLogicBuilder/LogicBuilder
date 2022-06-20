using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Services.DataParsers;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DataParserServices
    {
        internal static IServiceCollection AddDataParsers(this IServiceCollection services)
            => services
                .AddSingleton<IAssertFunctionDataParser, AssertFunctionDataParser>()
                .AddSingleton<IConditionsDataParser, ConditionsDataParser>()
                .AddSingleton<IConnectorDataParser, ConnectorDataParser>()
                .AddSingleton<IConstructorDataParser, ConstructorDataParser>()
                .AddSingleton<IDecisionDataParser, DecisionDataParser>()
                .AddSingleton<IDecisionsDataParser, DecisionsDataParser>()
                .AddSingleton<IDiagramErrorSourceDataParser, DiagramErrorSourceDataParser>()
                .AddSingleton<IFunctionDataParser, FunctionDataParser>()
                .AddSingleton<IFunctionsDataParser, FunctionsDataParser>()
                .AddSingleton<IJumpDataParser, JumpDataParser>()
                .AddSingleton<ILiteralListDataParser, LiteralListDataParser>()
                .AddSingleton<ILiteralListParameterDataParser, LiteralListParameterDataParser>()
                .AddSingleton<ILiteralListVariableDataParser, LiteralListVariableDataParser>()
                .AddSingleton<IMetaObjectDataParser, MetaObjectDataParser>()
                .AddSingleton<IModuleDataParser, ModuleDataParser>()
                .AddSingleton<IObjectDataParser, ObjectDataParser>()
                .AddSingleton<IObjectListDataParser, ObjectListDataParser>()
                .AddSingleton<IObjectListParameterDataParser, ObjectListParameterDataParser>()
                .AddSingleton<IObjectListVariableDataParser, ObjectListVariableDataParser>()
                .AddSingleton<IObjectParameterDataParser, ObjectParameterDataParser>()
                .AddSingleton<IObjectVariableDataParser, ObjectVariableDataParser>()
                .AddSingleton<IPriorityDataParser, PriorityDataParser>()
                .AddSingleton<IRetractFunctionDataParser, RetractFunctionDataParser>()
                .AddSingleton<ITableErrorSourceDataParser, TableErrorSourceDataParser>()
                .AddSingleton<IVariableDataParser, VariableDataParser>()
                .AddSingleton<IVariableValueDataParser, VariableValueDataParser>();
    }
}
