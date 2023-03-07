using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListParameterElementInfoHelper : ILiteralListParameterElementInfoHelper
    {
        private readonly IEnumHelper _enumHelper;

        public LiteralListParameterElementInfoHelper(
            IEnumHelper enumHelper)
        {
            _enumHelper = enumHelper;
        }

        public LiteralListParameterElementInfo GetDefaultLiteralListElementInfo()
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

        public LiteralListParameterElementInfo GetLiteralListElementInfo(ListOfLiteralsParameter literalListParameter, string parameterSourceClassName)
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
                literalListParameter,
                parameterSourceClassName
            );

        public LiteralListParameterElementInfo GetLiteralListElementInfo(LiteralListData listData)
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
