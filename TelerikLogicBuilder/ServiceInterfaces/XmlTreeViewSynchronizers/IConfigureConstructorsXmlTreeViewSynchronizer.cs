using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers
{
    internal interface IConfigureConstructorsXmlTreeViewSynchronizer
    {
        /// <summary>
        /// Creates a new constructor tree node from the corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="newXmlConstructorNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode AddConstructorNode(StateImageRadTreeNode destinationFolderTreeNode, XmlElement newXmlConstructorNode);

        /// <summary>
        /// Creates a list of new constructor tree nodes from the corresponding XML nodes.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="newXmlConstructorNodes"></param>
        void AddConstructorNodes(StateImageRadTreeNode destinationFolderTreeNode, IList<XmlElement> newXmlConstructorNodes);

        /// <summary>
        /// Creates a new tree folder and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        StateImageRadTreeNode AddFolder(StateImageRadTreeNode destinationFolderTreeNode, string folderName);

        /// <summary>
        /// Creates a new parameter tree node from the corresponding XML node.
        /// </summary>
        /// <param name="destinationConstructorTreeNode"></param>
        /// <param name="newXmlParameterNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode AddParameterNode(StateImageRadTreeNode destinationConstructorTreeNode, XmlElement newXmlParameterNode);

        /// <summary>
        /// Deletes a folder or constructor tree-node and the corresponding XML node.
        /// </summary>
        /// <param name="treeNode"></param>
        void DeleteNode(StateImageRadTreeNode treeNode);

        /// <summary>
        /// Deletes a constructur's parameter tree-node and the corresponding XML node.
        /// </summary>
        /// <param name="treeNode"></param>
        void DeleteParameterNode(StateImageRadTreeNode treeNode);

        /// <summary>
        /// Moves a constructur tree-node and the corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveConstructorNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode);

        /// <summary>
        /// Moves a folder node and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveFolderNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode);

        /// <summary>
        /// Moves a list of nodes to the destination folder
        /// </summary>
        /// <param name="destinationTreeNode"></param>
        /// <param name="movingTreeNodes"></param>
        void MoveFoldersConstructorsAndParameters(StateImageRadTreeNode destinationTreeNode, IList<StateImageRadTreeNode> movingTreeNodes);

        /// <summary>
        /// Moves a parameter node and corresponding XML node in front of another parameter node (used for changing the order or moving the parameter to a different constructor).
        /// </summary>
        /// <param name="destinationParameterTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveParameterBeforeParameter(StateImageRadTreeNode destinationParameterTreeNode, StateImageRadTreeNode movingTreeNode);

        /// <summary>
        /// Moves a parameter node and corresponding XML node to a different contructor
        /// </summary>
        /// <param name="destinationTreeConstructorNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveParameterToConstructor(StateImageRadTreeNode destinationTreeConstructorNode, StateImageRadTreeNode movingTreeNode);

        /// <summary>
        /// Replaces an existing treenode with a new one matching the new XML element.
        /// </summary>
        /// <param name="existingTreeNode"></param>
        /// <param name="newXmlConstructorNode"></param>
        void ReplaceConstructorNode(StateImageRadTreeNode existingTreeNode, XmlElement newXmlConstructorNode);
    }
}
