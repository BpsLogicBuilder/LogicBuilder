using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class GenericReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IReturnTypeFactory _returnTypeFactory;

        internal GenericReturnTypeInfo(IReturnTypeFactory returnTypeFactory, MethodInfo mInfo) : base(mInfo)
        {
            _returnTypeFactory = returnTypeFactory;
        }

        #region Methods
        internal override ReturnTypeBase GetReturnType() => _returnTypeFactory.GetGenericReturnType(this.MInfo.ReturnType.Name);
        #endregion Methods
    }
}
