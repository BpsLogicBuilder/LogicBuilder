using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces
{
    internal interface IModuleNamesReader
    {
        IDictionary<string, string> GetNames();
    }
}
