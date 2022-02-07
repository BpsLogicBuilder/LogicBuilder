using ABIS.LogicBuilder.FlowBuilder.Configuration;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface ILoadProjectProperties
    {
        ProjectProperties Load(string fullPath);
    }
}
