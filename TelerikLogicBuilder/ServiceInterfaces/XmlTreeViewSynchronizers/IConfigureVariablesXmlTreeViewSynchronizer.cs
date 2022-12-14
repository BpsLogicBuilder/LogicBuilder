using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers
{
    internal interface IConfigureVariablesXmlTreeViewSynchronizer
    {
        /// <summary>
        /// Creates a new tree folder and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="folderName"></param>
        /// <returns>The new tree node.</returns>
        StateImageRadTreeNode AddFolder(StateImageRadTreeNode destinationFolderTreeNode, string folderName);

        /// <summary>
        /// Creates a new variable tree node from the corresponding XML node.
        /// </summary>
        /// <param name="destinationParentNode"></param>
        /// <param name="newXmlVariableNode"></param>
        /// <returns>The new tree node.</returns>
        StateImageRadTreeNode AddVariableNode(StateImageRadTreeNode destinationFolderTreeNode, XmlElement newXmlVariableNode);

        /// <summary>
        /// Creates a list of new variable tree nodes from the corresponding XML nodes.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="newXmlVariableNodes"></param>
        void AddVariableNodes(StateImageRadTreeNode destinationFolderTreeNode, IList<XmlElement> newXmlVariableNodes);

        /// <summary>
        /// Deletes the tree node and the corresponding XML node.
        /// </summary>
        /// <param name="treeNode"></param>
        void DeleteNode(StateImageRadTreeNode treeNode);

        /// <summary>
        /// Moves a list of nodes to the destination folder
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNodes"></param>
        void MoveFoldersAndVariables(StateImageRadTreeNode destinationFolderTreeNode, IList<StateImageRadTreeNode> movingTreeNodes);

        /// <summary>
        /// Moves a folder node and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns>The tree node at the new location</returns>
        StateImageRadTreeNode? MoveFolderNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode);

        /// <summary>
        /// Moves a variable node and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns>The tree node at the new loacation.</returns>
        StateImageRadTreeNode? MoveVariableNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode);

        /// <summary>
        /// Replaces an existing treenode with a new one matching the new XML element.
        /// </summary>
        /// <param name="existingTreeNode"></param>
        /// <param name="newXmlVariableNode"></param>
        void ReplaceVariableNode(StateImageRadTreeNode existingTreeNode, XmlElement newXmlVariableNode);
    }
}
