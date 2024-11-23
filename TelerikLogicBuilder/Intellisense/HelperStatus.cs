using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal class HelperStatus(
        Application application,
        LinkedList<BaseTreeNode> path,
        ReferenceCategories referenceCategory,
        string className)
    {
        public Application Application { get; set; } = application;
        public LinkedList<BaseTreeNode> Path { get; set; } = path;
        public ReferenceCategories ReferenceCategory { get; set; } = referenceCategory;
        public string ClassName { get; set; } = className;
    }
}
