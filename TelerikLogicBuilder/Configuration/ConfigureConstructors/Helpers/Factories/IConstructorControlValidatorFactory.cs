using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers.Factories
{
    internal interface IConstructorControlValidatorFactory
    {
        IConstructorControlsValidator GetConstructorControlsValidator(IConfigureConstructorControl configureConstructorControl);
    }
}
