using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ListOfLiteralsReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly ITypeHelper _typeHelper;

        internal ListOfLiteralsReturnTypeInfo(IEnumHelper enumHelper, IReturnTypeFactory returnTypeFactory, ITypeHelper typeHelper, MethodInfo mInfo) : base(mInfo)
        {
            _enumHelper = enumHelper;
            _returnTypeFactory = returnTypeFactory;
            _typeHelper = typeHelper;
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
        internal override ReturnTypeBase GetReturnType() => _returnTypeFactory.GetListOfLiteralsReturnType(this.LiteralFunctionReturnType, this.ListType);
        #endregion Methods
    }
}
