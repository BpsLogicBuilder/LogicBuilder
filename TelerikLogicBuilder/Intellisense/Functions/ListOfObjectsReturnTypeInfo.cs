using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ListOfObjectsReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IContextProvider _contextProvider;
        private readonly ITypeHelper _typeHelper;
        private readonly IEnumHelper _enumHelper;

        internal ListOfObjectsReturnTypeInfo(MethodInfo mInfo, IContextProvider contextProvider)
            : base(mInfo)
        {
            _enumHelper = contextProvider.EnumHelper;
            _typeHelper = contextProvider.TypeHelper;
            _contextProvider = contextProvider;
        }

        internal ListType ListType => _enumHelper.GetListType(this.MInfo.ReturnType);

        #region Methods
        internal override ReturnTypeBase GetReturnType() => new ListOfObjectsReturnType
        (
            _typeHelper.ToId
            (
                _typeHelper.GetUndelyingTypeForValidList(this.MInfo.ReturnType)
            ), 
            this.ListType, 
            _contextProvider
        );
        #endregion Methods
    }
}
