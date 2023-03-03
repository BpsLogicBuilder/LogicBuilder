using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal interface ILiteralListParameterElementInfoHelper
    {
        LiteralListParameterElementInfo GetDefaultLiteralListElementInfo();
        LiteralListParameterElementInfo GetLiteralListElementInfo(ListOfLiteralsParameter literalListParameter);
        LiteralListParameterElementInfo GetLiteralListElementInfo(LiteralListData listData);
    }
}
