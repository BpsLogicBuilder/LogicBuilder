using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface ITableValidator
    {
        Task<IList<ResultMessage>> Validate();
    }
}
