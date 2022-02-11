using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ObjectReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly ITypeHelper _typeHelper;

        internal ObjectReturnTypeInfo(MethodInfo mInfo, IContextProvider contextProvider)
            : base(mInfo)
        {
            _typeHelper = contextProvider.TypeHelper;
            _contextProvider = contextProvider;
        }

        #region Methods
        internal override ReturnTypeBase GetReturnType() => new ObjectReturnType(_typeHelper.ToId(this.MInfo.ReturnType), _contextProvider);
        #endregion Methods
    }
}
