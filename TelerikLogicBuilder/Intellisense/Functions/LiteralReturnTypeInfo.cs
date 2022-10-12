using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class LiteralReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IEnumHelper _enumHelper; 
        private readonly IReturnTypeFactory _returnTypeFactory;

        internal LiteralReturnTypeInfo(IEnumHelper enumHelper, IReturnTypeFactory returnTypeFactory, MethodInfo mInfo) : base(mInfo)
        {
            _enumHelper = enumHelper;
            _returnTypeFactory = returnTypeFactory;
        }

        internal LiteralFunctionReturnType LiteralFunctionReturnType => _enumHelper.GetLiteralFunctionReturnType(MInfo.ReturnType);

        #region Methods
        internal override ReturnTypeBase GetReturnType() => _returnTypeFactory.GetLiteralReturnType(this.LiteralFunctionReturnType);
        #endregion Methods
    }
}
