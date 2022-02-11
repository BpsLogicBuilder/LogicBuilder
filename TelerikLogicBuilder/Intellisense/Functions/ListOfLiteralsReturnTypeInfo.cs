using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ListOfLiteralsReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly ITypeHelper _typeHelper;
        private readonly IEnumHelper _enumHelper;

        internal ListOfLiteralsReturnTypeInfo(MethodInfo mInfo, IContextProvider contextProvider) : base(mInfo)
        {
            _enumHelper = contextProvider.EnumHelper;
            _typeHelper = contextProvider.TypeHelper;
            _contextProvider = contextProvider;
        }

        /// <summary>
        /// LiteralFunctionReturnType for underlying return type
        /// </summary>
        internal LiteralFunctionReturnType LiteralFunctionReturnType => _enumHelper.GetLiteralFunctionReturnType(_typeHelper.GetUndelyingTypeForValidList(MInfo.ReturnType));

        /// <summary>
        /// List Type
        /// </summary>
        internal ListType ListType => _enumHelper.GetListType(this.MInfo.ReturnType);

        #region Methods
        internal override ReturnTypeBase GetReturnType() => new ListOfLiteralsReturnType(this.LiteralFunctionReturnType, this.ListType, _contextProvider);
        #endregion Methods
    }
}
