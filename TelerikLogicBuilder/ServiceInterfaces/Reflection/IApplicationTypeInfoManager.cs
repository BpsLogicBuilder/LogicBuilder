using ABIS.LogicBuilder.FlowBuilder.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection
{
    internal interface IApplicationTypeInfoManager
    {
        void ClearApplications();
        ApplicationTypeInfo GetApplicationTypeInfo(string applicationName);
        bool HasApplications { get; }
    }
}
