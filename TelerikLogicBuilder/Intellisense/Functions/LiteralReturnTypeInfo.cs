using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class LiteralReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly IEnumHelper _enumHelper;

        internal LiteralReturnTypeInfo(MethodInfo mInfo, IContextProvider contextProvider) : base(mInfo)
        {
            _enumHelper = contextProvider.EnumHelper;
            this._contextProvider = contextProvider;
        }

        internal LiteralFunctionReturnType LiteralFunctionReturnType => _enumHelper.GetLiteralFunctionReturnType(MInfo.ReturnType);

        #region Methods
        internal override ReturnTypeBase GetReturnType() => new LiteralReturnType(this.LiteralFunctionReturnType, _contextProvider);
        #endregion Methods
    }
}
