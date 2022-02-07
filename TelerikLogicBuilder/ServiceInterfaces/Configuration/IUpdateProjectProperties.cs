using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration
{
    internal interface IUpdateProjectProperties
    {
        ProjectProperties Update(string fullPath, Dictionary<string, Application> applicationList, HashSet<string> connectorObjectTypes);
    }
}
