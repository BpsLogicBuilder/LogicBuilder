using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IDiagramValidator
    {
        Task<IList<ResultMessage>> Validate();
    }
}
