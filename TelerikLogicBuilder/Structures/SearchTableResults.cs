using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class SearchTableResults
    {
        public SearchTableResults(int cellCount, IList<ResultMessage> resultMessages)
        {
            CellCount = cellCount;
            ResultMessages = resultMessages;
        }

        public int CellCount { get; }
        public IList<ResultMessage> ResultMessages { get; }
    }
}
