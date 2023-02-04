using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration.Initialization;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Configuration.Initialization
{
    internal class FunctionListMatcher : IFunctionListMatcher
    {
        public bool IsBoolFunction(Function function)
            => IsLiteralMatch(function, LiteralFunctionReturnType.Boolean);

        public bool IsDialogFunction(Function function) 
            => IsLiteralMatch(function, LiteralFunctionReturnType.Void)
                && function.FunctionCategory == FunctionCategories.DialogForm;

        public bool IsTableFunction(Function function) 
            => IsLiteralMatch(function, LiteralFunctionReturnType.Void)
                && function.FunctionCategory != FunctionCategories.DialogForm;

        public bool IsValueFunction(Function function) 
            => !IsLiteralMatch(function, LiteralFunctionReturnType.Void);

        public bool IsVoidFunction(Function function) 
            => IsLiteralMatch(function, LiteralFunctionReturnType.Void)
                && function.FunctionCategory != FunctionCategories.DialogForm
                && function.FunctionCategory != FunctionCategories.RuleChainingUpdate;

        static bool IsLiteralMatch(Function function, LiteralFunctionReturnType literalType)
            => function.ReturnType is LiteralReturnType literalReturnType
                && literalReturnType.ReturnType == literalType;
    }
}
