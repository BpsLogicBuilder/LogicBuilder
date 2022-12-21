using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectVariable;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers.Factories
{
    internal class VariableControlValidatorFactory : IVariableControlValidatorFactory
    {
        private readonly Func<IConfigureLiteralListVariableControl, ILiteralListVariableControlsValidator> _getLiteralListVariableControlsValidator;
        private readonly Func<IConfigureLiteralVariableControl, ILiteralVariableControlsValidator> _getLiteralVariableControlsValidator;
        private readonly Func<IConfigureObjectListVariableControl, IObjectListVariableControlsValidator> _getObjectListVariableControlsValidator;
        private readonly Func<IConfigureObjectVariableControl, IObjectVariableControlsValidator> _getObjectVariableControlsValidator;
        private readonly Func<IConfigureVariableControl, IVariableControlsValidator> _getVariableControlsValidator;

        public VariableControlValidatorFactory(
            Func<IConfigureLiteralListVariableControl, ILiteralListVariableControlsValidator> getLiteralListVariableControlsValidator,
            Func<IConfigureLiteralVariableControl, ILiteralVariableControlsValidator> getLiteralVariableControlsValidator,
            Func<IConfigureObjectListVariableControl, IObjectListVariableControlsValidator> getObjectListVariableControlsValidator,
            Func<IConfigureObjectVariableControl, IObjectVariableControlsValidator> getObjectVariableControlsValidator,
            Func<IConfigureVariableControl, IVariableControlsValidator> getVariableControlsValidator)
        {
            _getLiteralListVariableControlsValidator = getLiteralListVariableControlsValidator;
            _getLiteralVariableControlsValidator = getLiteralVariableControlsValidator;
            _getObjectListVariableControlsValidator = getObjectListVariableControlsValidator;
            _getObjectVariableControlsValidator = getObjectVariableControlsValidator;
            _getVariableControlsValidator = getVariableControlsValidator;
        }

        public ILiteralListVariableControlsValidator GetLiteralListVariableControlsValidator(IConfigureLiteralListVariableControl configureLiteralListVariableControl)
            => _getLiteralListVariableControlsValidator(configureLiteralListVariableControl);

        public ILiteralVariableControlsValidator GetLiteralVariableControlsValidator(IConfigureLiteralVariableControl configureLiteralVariableControl)
            => _getLiteralVariableControlsValidator(configureLiteralVariableControl);

        public IObjectListVariableControlsValidator GetObjectListVariableControlsValidator(IConfigureObjectListVariableControl configureObjectListVariableControl)
            => _getObjectListVariableControlsValidator(configureObjectListVariableControl);

        public IObjectVariableControlsValidator GetObjectVariableControlsValidator(IConfigureObjectVariableControl configureObjectVariableControl)
            => _getObjectVariableControlsValidator(configureObjectVariableControl);

        public IVariableControlsValidator GetVariableControlsValidator(IConfigureVariableControl configureVariableControl)
            => _getVariableControlsValidator(configureVariableControl);
    }
}
