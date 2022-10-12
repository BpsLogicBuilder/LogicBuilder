using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ListOfObjectsReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly ITypeHelper _typeHelper;

        internal ListOfObjectsReturnTypeInfo(IEnumHelper enumHelper, IReturnTypeFactory returnTypeFactory, ITypeHelper typeHelper, MethodInfo mInfo)
            : base(mInfo)
        {
            _enumHelper = enumHelper;
            _typeHelper = typeHelper;
            _returnTypeFactory = returnTypeFactory;
        }

        internal ListType ListType => _enumHelper.GetListType(this.MInfo.ReturnType);

        #region Methods
        internal override ReturnTypeBase GetReturnType() => _returnTypeFactory.GetListOfObjectsReturnType
        (
            _typeHelper.ToId
            (
                _typeHelper.GetUndelyingTypeForValidList(this.MInfo.ReturnType)
            ), 
            this.ListType
        );
        #endregion Methods
    }
}
