using System.Drawing;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    public class StateImageRadTreeNode : RadTreeNode
    {
        public StateImageRadTreeNode()
        {
        }

        public StateImageRadTreeNode(string text) : base(text)
        {
        }

        public StateImageRadTreeNode(string text, bool expanded) : base(text, expanded)
        {
        }

        public StateImageRadTreeNode(string text, Image image) : base(text, image)
        {
        }

        public StateImageRadTreeNode(string text, RadTreeNode[] children) : base(text, children)
        {
        }

        public StateImageRadTreeNode(string text, Image image, bool expanded) : base(text, image, expanded)
        {
        }

        public Image? StateImage { get; set; }
    }
}
