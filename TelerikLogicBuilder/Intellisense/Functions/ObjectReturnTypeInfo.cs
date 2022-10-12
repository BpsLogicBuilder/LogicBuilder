using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ObjectReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly ITypeHelper _typeHelper;

        internal ObjectReturnTypeInfo(IReturnTypeFactory returnTypeFactory, ITypeHelper typeHelper, MethodInfo mInfo)
            : base(mInfo)
        {
            _returnTypeFactory = returnTypeFactory;
            _typeHelper = typeHelper;
        }

        #region Methods
        internal override ReturnTypeBase GetReturnType() => _returnTypeFactory.GetObjectReturnType(_typeHelper.ToId(this.MInfo.ReturnType));
        #endregion Methods
    }
}
