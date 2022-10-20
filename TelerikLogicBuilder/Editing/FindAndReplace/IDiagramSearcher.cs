using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface IDiagramSearcher
    { 
        Task<SearchDiagramResults> Search();
    }
}
