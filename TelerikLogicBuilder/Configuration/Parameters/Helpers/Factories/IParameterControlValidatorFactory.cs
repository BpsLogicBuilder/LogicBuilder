using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureGenericParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectListParameter;
using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureObjectParameter;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers.Factories
{
    internal interface IParameterControlValidatorFactory
    {
        IGenericListParameterControlValidator GetGenericListParameterControlValidator(IConfigureGenericListParameterControl configureGenericListParameterControl);
        IGenericParameterControlValidator GetGenericParameterControlValidator(IConfigureGenericParameterControl configureeGenericParameterControl);
        ILiteralListParameterControlValidator GetLiteralListParameterControlValidator(IConfigureLiteralListParameterControl configureGenericListParameterControl);
        ILiteralParameterControlValidator GetLiteralParameterControlValidator(IConfigureLiteralParameterControl configureGenericListParameterControl);
        IObjectListParameterControlValidator GetObjectListParameterControlValidator(IConfigureObjectListParameterControl configureGenericListParameterControl);
        IObjectParameterControlValidator GetObjectParameterControlValidator(IConfigureObjectParameterControl configureGenericListParameterControl);
    }
}
