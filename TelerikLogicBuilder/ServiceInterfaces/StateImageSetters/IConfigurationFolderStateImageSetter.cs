using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters
{
    internal interface IConfigurationFolderStateImageSetter
    {
        void SetImage(StateImageRadTreeNode? treeNode);
    }
}
