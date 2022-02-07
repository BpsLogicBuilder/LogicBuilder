using ABIS.LogicBuilder.FlowBuilder.Configuration;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors
{
    internal class ConstructorHelperStatus
    {
        internal ConstructorHelperStatus(Application Application, RadTreeNode Node, string ClassName)
        {
            this.Application = Application;
            this.Node = Node;
            this.ClassName = ClassName;
        }

        #region Properties
        internal Application Application { get; set; }
        internal RadTreeNode Node { get; set; }
        internal string ClassName { get; set; }
        #endregion Properties
    }
}
