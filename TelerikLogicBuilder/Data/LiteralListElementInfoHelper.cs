using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListElementInfoHelper : ILiteralListElementInfoHelper
    {
        private readonly IEnumHelper _enumHelper;

        public LiteralListElementInfoHelper(
            IEnumHelper enumHelper)
        {
            _enumHelper = enumHelper;
        }

        public LiteralListElementInfo GetDefaultLiteralListElementInfo()
            => new
            (
                _enumHelper.GetTypeDescription
                (
                    ListType.GenericList,
                    _enumHelper.GetVisibleEnumText(LiteralParameterType.String)
                ),
                ListType.GenericList,
                LiteralParameterType.String,
                ListParameterInputStyle.ListForm,
                LiteralParameterInputStyle.SingleLineTextBox,
                new List<string>(),
                new List<string>(),
                string.Empty
            );

        public LiteralListElementInfo GetLiteralListElementInfo(ListOfLiteralsParameter literalListParameter)
            => new
            (
                literalListParameter.Name,
                literalListParameter.ListType,
                literalListParameter.LiteralType,
                literalListParameter.Control,
                literalListParameter.ElementControl,
                literalListParameter.Domain,
                literalListParameter.DefaultValues,
                literalListParameter.Comments,
                literalListParameter
            );

        public LiteralListElementInfo GetLiteralListElementInfo(LiteralListData listData)
            => new
            (
                _enumHelper.GetTypeDescription
                (
                    listData.ListType,
                    _enumHelper.GetVisibleEnumText(listData.LiteralType)
                ),
                listData.ListType,
                listData.LiteralType,
                ListParameterInputStyle.ListForm,
                LiteralParameterInputStyle.SingleLineTextBox,
                new List<string>(),
                new List<string>(),
                string.Empty
            );
    }
}
