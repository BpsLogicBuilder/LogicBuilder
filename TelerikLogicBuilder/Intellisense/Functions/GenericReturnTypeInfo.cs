using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class GenericReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IContextProvider _contextProvider;

        internal GenericReturnTypeInfo(MethodInfo mInfo, IContextProvider contextProvider) : base(mInfo)
        {
            _contextProvider = contextProvider;
        }

        #region Methods
        internal override ReturnTypeBase GetReturnType() => new GenericReturnType(this.MInfo.ReturnType.Name, _contextProvider);
        #endregion Methods
    }
}
