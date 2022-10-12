using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class ListOfGenericsReturnTypeInfo : ReturnTypeInfoBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IReturnTypeFactory _returnTypeFactory;
        private readonly ITypeHelper _typeHelper;
        
        internal ListOfGenericsReturnTypeInfo(IEnumHelper enumHelper, IReturnTypeFactory returnTypeFactory, ITypeHelper typeHelper, MethodInfo mInfo)
            : base(mInfo)
        {
            _enumHelper = enumHelper;
            _returnTypeFactory = returnTypeFactory;
            _typeHelper = typeHelper;
        }

        private string GenericParameterName => _typeHelper.GetUndelyingTypeForValidList(MInfo.ReturnType).Name;
        private ListType ListType => _enumHelper.GetListType(this.MInfo.ReturnType);

        #region Methods
        internal override ReturnTypeBase GetReturnType() => _returnTypeFactory.GetListOfGenericsReturnType(this.GenericParameterName, this.ListType);
        #endregion Methods
    }
}
