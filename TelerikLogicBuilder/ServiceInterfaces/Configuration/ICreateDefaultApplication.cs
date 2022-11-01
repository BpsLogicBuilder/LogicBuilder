using ABIS.LogicBuilder.FlowBuilder.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface ICreateDefaultApplication
    {
        Application Create(string applicationNameString);
    }
}
