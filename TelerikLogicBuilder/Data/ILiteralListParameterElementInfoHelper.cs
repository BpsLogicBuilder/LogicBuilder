using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal interface ILiteralListParameterElementInfoHelper
    {
        LiteralListParameterElementInfo GetDefaultLiteralListElementInfo();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="literalListParameter"></param>
        /// <param name="parameterSourceClassName">
        /// The corresponding LiteralListControl (unlike LiteralListParameterRichTextBoxControl) will not 
        /// have access to the other parameters (editControlsSet) therefore the parameterSourceClassName must
        /// included here if applicable.
        /// </param>
        /// <returns></returns>
        LiteralListParameterElementInfo GetLiteralListElementInfo(ListOfLiteralsParameter literalListParameter, string parameterSourceClassName);
        LiteralListParameterElementInfo GetLiteralListElementInfo(LiteralListData listData);
    }
}
