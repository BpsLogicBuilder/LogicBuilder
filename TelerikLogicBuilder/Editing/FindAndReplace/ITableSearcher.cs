using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface ITableSearcher
    {
        Task<SearchTableResults> Search();
    }
}
