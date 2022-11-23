namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories
{
    internal interface IConfigureProjectPropertiesControlFactory
    {
        IApplicationControl GetApplicationControl(IConfigureProjectPropertiesForm configureProjectProperties);
    }
}
