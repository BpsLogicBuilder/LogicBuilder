using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers
{
    internal interface IConfigurationFormXmlTreeViewSynchronizer
    {
        /// <summary>
        /// Creates a new tree folder and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="folderName"></param>
        /// <returns>The new tree node.</returns>
        StateImageRadTreeNode AddFolder(StateImageRadTreeNode destinationFolderTreeNode, string folderName);

        /// <summary>
        /// Creates a new parameter tree node and corresponding XML node.
        /// </summary>
        /// <param name="destinationTreeNode"></param>
        /// <param name="newXmlParameterNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode AddParameterNode(StateImageRadTreeNode destinationTreeNode, XmlElement newXmlParameterNode);

        /// <summary>
        /// Deletes a folder, constructor, function, or variable tree-node and the corresponding XML node.
        /// </summary>
        /// <param name="treeNode"></param>
        void DeleteFolderOrConfiguredItem(StateImageRadTreeNode treeNode, bool validate = true);

        /// <summary>
        /// Deletes a function or constructur's parameter tree-node and the corresponding XML node.
        /// </summary>
        /// <param name="treeNode"></param>
        void DeleteParameterNode(StateImageRadTreeNode treeNode);

        /// <summary>
        /// Moves a constructur, function or variable tree-node and the corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveConfiguredItem(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode, bool validate = true);

        /// <summary>
        /// Moves a folder node and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns>The tree node at the new location.</returns>
        StateImageRadTreeNode? MoveFolderNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode, bool validate = true);

        /// <summary>
        /// Moves a parameter node and corresponding XML node in front of another parameter node (used for changing the order or moving the parameter to a different constructore/function).
        /// </summary>
        /// <param name="destinationParameterTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveParameterBeforeParameter(StateImageRadTreeNode destinationParameterTreeNode, StateImageRadTreeNode movingTreeNode, bool validate = true);

        /// <summary>
        /// Moves a parameter node and corresponding XML node to a different contructor/function
        /// </summary>
        /// <param name="destinationTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <param name="validate"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveParameterToConfiguredItem(StateImageRadTreeNode destinationTreeNode, StateImageRadTreeNode movingTreeNode, bool validate = true);
    }
}
