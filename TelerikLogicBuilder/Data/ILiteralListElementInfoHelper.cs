using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal interface ILiteralListElementInfoHelper
    {
        LiteralListElementInfo GetDefaultLiteralListElementInfo();
        LiteralListElementInfo GetLiteralListElementInfo(ListOfLiteralsParameter literalListParameter);
        LiteralListElementInfo GetLiteralListElementInfo(LiteralListData listData);
    }
}
