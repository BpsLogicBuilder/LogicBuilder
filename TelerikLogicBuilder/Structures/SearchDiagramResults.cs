using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class SearchDiagramResults
    {
        public SearchDiagramResults(int shapeCount, IList<ResultMessage> resultMessages)
        {
            ShapeCount = shapeCount;
            ResultMessages = resultMessages;
        }

        public int ShapeCount { get; }
        public IList<ResultMessage> ResultMessages { get; }
    }
}
