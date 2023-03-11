using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListVariableElementInfoHelper : ILiteralListVariableElementInfoHelper
    {
        private readonly IEnumHelper _enumHelper;

        public LiteralListVariableElementInfoHelper(
            IEnumHelper enumHelper)
        {
            _enumHelper = enumHelper;
        }

        public LiteralListVariableElementInfo GetDefaultLiteralListElementInfo()
            => new
            (
                _enumHelper.GetTypeDescription
                (
                    ListType.GenericList,
                    _enumHelper.GetVisibleEnumText(LiteralVariableType.String)
                ),
                ListType.GenericList,
                LiteralVariableType.String,
                ListVariableInputStyle.ListForm,
                LiteralVariableInputStyle.SingleLineTextBox,
                new List<string>(),
                new List<string>(),
                string.Empty
            );

        public LiteralListVariableElementInfo GetLiteralListElementInfo(ListOfLiteralsVariable literalListVariable) 
            => new
            (
                literalListVariable.Name,
                literalListVariable.ListType,
                literalListVariable.LiteralType,
                literalListVariable.Control,
                literalListVariable.ElementControl,
                literalListVariable.Domain,
                literalListVariable.DefaultValue,
                literalListVariable.Comments,
                literalListVariable
            );

        public LiteralListVariableElementInfo GetLiteralListElementInfo(LiteralListData listData)
            => new
            (
                _enumHelper.GetTypeDescription
                (
                    listData.ListType,
                    _enumHelper.GetVisibleEnumText(listData.LiteralType)
                ),
                listData.ListType,
                _enumHelper.GetLiteralVariableType(listData.LiteralType),
                ListVariableInputStyle.ListForm,
                LiteralVariableInputStyle.SingleLineTextBox,
                new List<string>(),
                new List<string>(),
                string.Empty
            );
    }
}
