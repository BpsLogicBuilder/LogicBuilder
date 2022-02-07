using ABIS.LogicBuilder.FlowBuilder.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface ICreateProjectProperties
    {
        ProjectProperties Create(string path, string projectName);
        ProjectProperties Create(string fullPath);
    }
}
