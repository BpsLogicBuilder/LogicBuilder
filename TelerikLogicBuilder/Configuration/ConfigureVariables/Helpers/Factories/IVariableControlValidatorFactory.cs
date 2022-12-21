using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectVariable;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers.Factories
{
    internal interface IVariableControlValidatorFactory
    {
        ILiteralListVariableControlsValidator GetLiteralListVariableControlsValidator(IConfigureLiteralListVariableControl configureLiteralListVariableControl);
        ILiteralVariableControlsValidator GetLiteralVariableControlsValidator(IConfigureLiteralVariableControl configureLiteralVariableControl);
        IObjectListVariableControlsValidator GetObjectListVariableControlsValidator(IConfigureObjectListVariableControl configureObjectListVariableControl);
        IObjectVariableControlsValidator GetObjectVariableControlsValidator(IConfigureObjectVariableControl configureObjectVariableControl);
        IVariableControlsValidator GetVariableControlsValidator(IConfigureVariableControl configureVariableControl);
    }
}
