using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureGenericListReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureGenericReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureLiteralListReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureLiteralReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureObjectListReturnType;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.ConfigureObjectReturnType;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType.Factories
{
    internal interface IConfigureReturnTypeControlFactory
    {
        IConfigureGenericListReturnTypeControl GetConfigureGenericListReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm);
        IConfigureGenericReturnTypeControl GetConfigureGenericReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm);
        IConfigureLiteralListReturnTypeControl GetConfigureLiteralListReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm);
        IConfigureLiteralReturnTypeControl GetConfigureLiteralReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm);
        IConfigureObjectListReturnTypeControl GetConfigureObjectListReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm);
        IConfigureObjectReturnTypeControl GetConfigureObjectReturnTypeControl(IConfigureReturnTypeForm configureReturnTypeForm);
    }
}
