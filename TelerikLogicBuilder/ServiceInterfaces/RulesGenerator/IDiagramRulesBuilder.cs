using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IDiagramRulesBuilder
    {
        Task<BuildRulesResult> BuildRules();
    }
}
