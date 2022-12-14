using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.StateImageSetters
{
    internal class ConfigurationFolderStateImageSetter : IConfigurationFolderStateImageSetter
    {
        private readonly ICompareImages _compareImages;

        public ConfigurationFolderStateImageSetter(ICompareImages compareImages)
        {
            _compareImages = compareImages;
        }

        public void SetImage(StateImageRadTreeNode? treeNode)
        {
            if (treeNode == null)
                return;

            treeNode.StateImage = treeNode.Nodes
            .OfType<StateImageRadTreeNode>()
                .Any(n => _compareImages.AreEqual(n.StateImage, Properties.Resources.Error))
                    ? Properties.Resources.Error
                    : Properties.Resources.CheckMark;

            SetImage((StateImageRadTreeNode)treeNode.Parent);
        }
    }
}
