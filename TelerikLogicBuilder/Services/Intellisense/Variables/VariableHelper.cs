using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables
{
    internal class VariableHelper : IVariableHelper
    {
        private readonly IEnumHelper _enumHelper;

        public VariableHelper(IEnumHelper enumHelper)
        {
            _enumHelper = enumHelper;
        }

        public bool CanBeInteger(VariableBase variable) 
            => (variable is LiteralVariable literalVariable) && _enumHelper.CanBeInteger(literalVariable.LiteralType);
    }
}
