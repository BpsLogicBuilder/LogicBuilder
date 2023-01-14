using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectParameter;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers.Factories
{
    internal class ParameterControlValidatorFactory : IParameterControlValidatorFactory
    {
        private readonly Func<IConfigureGenericListParameterControl, IGenericListParameterControlValidator> _getGenericListParameterControlValidator;
        private readonly Func<IConfigureGenericParameterControl, IGenericParameterControlValidator> _getGenericParameterControlValidator;
        private readonly Func<IConfigureLiteralListParameterControl, ILiteralListParameterControlValidator> _getLiteralListParameterControlValidator;
        private readonly Func<IConfigureLiteralParameterControl, ILiteralParameterControlValidator> _getLiteralParameterControlValidator;
        private readonly Func<IConfigureObjectListParameterControl, IObjectListParameterControlValidator> _getObjectListParameterControlValidator;
        private readonly Func<IConfigureObjectParameterControl, IObjectParameterControlValidator> _getObjectParameterControlValidator;

        public ParameterControlValidatorFactory(
            Func<IConfigureGenericListParameterControl, IGenericListParameterControlValidator> getGenericListParameterControlValidator,
            Func<IConfigureGenericParameterControl, IGenericParameterControlValidator> getGenericParameterControlValidator,
            Func<IConfigureLiteralListParameterControl, ILiteralListParameterControlValidator> getLiteralListParameterControlValidator,
            Func<IConfigureLiteralParameterControl, ILiteralParameterControlValidator> getLiteralParameterControlValidator,
            Func<IConfigureObjectListParameterControl, IObjectListParameterControlValidator> getObjectListParameterControlValidator,
            Func<IConfigureObjectParameterControl, IObjectParameterControlValidator> getObjectParameterControlValidator)
        {
            _getGenericListParameterControlValidator = getGenericListParameterControlValidator;
            _getGenericParameterControlValidator = getGenericParameterControlValidator;
            _getLiteralListParameterControlValidator = getLiteralListParameterControlValidator;
            _getLiteralParameterControlValidator = getLiteralParameterControlValidator;
            _getObjectListParameterControlValidator = getObjectListParameterControlValidator;
            _getObjectParameterControlValidator = getObjectParameterControlValidator;
        }

        public IGenericListParameterControlValidator GetGenericListParameterControlValidator(IConfigureGenericListParameterControl configureGenericListParameterControl)
            => _getGenericListParameterControlValidator(configureGenericListParameterControl);

        public IGenericParameterControlValidator GetGenericParameterControlValidator(IConfigureGenericParameterControl configureeGenericParameterControl)
            => _getGenericParameterControlValidator(configureeGenericParameterControl);

        public ILiteralListParameterControlValidator GetLiteralListParameterControlValidator(IConfigureLiteralListParameterControl configureGenericListParameterControl)
            => _getLiteralListParameterControlValidator(configureGenericListParameterControl);

        public ILiteralParameterControlValidator GetLiteralParameterControlValidator(IConfigureLiteralParameterControl configureGenericListParameterControl)
            => _getLiteralParameterControlValidator(configureGenericListParameterControl);

        public IObjectListParameterControlValidator GetObjectListParameterControlValidator(IConfigureObjectListParameterControl configureGenericListParameterControl)
            => _getObjectListParameterControlValidator(configureGenericListParameterControl);

        public IObjectParameterControlValidator GetObjectParameterControlValidator(IConfigureObjectParameterControl configureGenericListParameterControl)
            => _getObjectParameterControlValidator(configureGenericListParameterControl);
    }
}
