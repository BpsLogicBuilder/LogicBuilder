using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ListOfGenericsReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly ITypeHelper _typeHelper;
        private readonly IEnumHelper _enumHelper;

        internal ListOfGenericsReturnTypeInfo(MethodInfo mInfo, IContextProvider contextProvider)
            : base(mInfo)
        {
            _enumHelper = contextProvider.EnumHelper;
            _typeHelper = contextProvider.TypeHelper;
            _contextProvider = contextProvider;
        }

        #region Fields
        #endregion Fields


        private string GenericParameterName => _typeHelper.GetUndelyingTypeForValidList(MInfo.ReturnType).Name;
        private ListType ListType => _enumHelper.GetListType(this.MInfo.ReturnType);

        #region Methods
        internal override ReturnTypeBase GetReturnType() => new ListOfGenericsReturnType(this.GenericParameterName, this.ListType, _contextProvider);
        #endregion Methods
    }
}
