using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class FunctionHelper : IFunctionHelper
    {
        public bool IsBoolean(Function function) => (function.ReturnType is LiteralReturnType literalReturnType) && literalReturnType.ReturnType == LiteralFunctionReturnType.Boolean;

        public bool IsDialog(Function function) => function.FunctionCategory == FunctionCategories.DialogForm;

        public bool IsVoid(Function function) => (function.ReturnType is LiteralReturnType literalReturnType) && literalReturnType.ReturnType == LiteralFunctionReturnType.Void;
    }
}
