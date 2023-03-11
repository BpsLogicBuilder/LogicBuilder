using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal interface ILiteralListVariableElementInfoHelper
    {
        LiteralListVariableElementInfo GetDefaultLiteralListElementInfo();
        LiteralListVariableElementInfo GetLiteralListElementInfo(ListOfLiteralsVariable literalListVariable);
        LiteralListVariableElementInfo GetLiteralListElementInfo(LiteralListData listData);
    }
}
