using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal class HelperStatus
    {
        public HelperStatus(
            Application application,
            LinkedList<BaseTreeNode> path,
            ReferenceCategories referenceCategory,
            string className)
        {
            Application = application;
            Path = path;
            ReferenceCategory = referenceCategory;
            ClassName = className;
        }

        public Application Application { get; set; }
        public LinkedList<BaseTreeNode> Path { get; set; }
        public ReferenceCategories ReferenceCategory { get; set; }
        public string ClassName { get; set; }
    }
}
