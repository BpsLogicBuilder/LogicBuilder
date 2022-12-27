using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal abstract class BaseTreeNode : RadTreeNode
    {
        protected BaseTreeNode(
            string text,
            IVariableTreeNode? parentNode)
            : base(text)
        {
            ParentNode = parentNode;
        }

        #region Properties
        public virtual string MemberText => Text;
        public IVariableTreeNode? ParentNode { get; }
        public abstract Type MemberType { get; }
        #endregion Properties

        
    }
}
