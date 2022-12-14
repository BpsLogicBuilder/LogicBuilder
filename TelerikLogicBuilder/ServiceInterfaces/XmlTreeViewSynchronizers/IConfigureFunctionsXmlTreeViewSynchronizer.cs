using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers
{
    internal interface IConfigureFunctionsXmlTreeViewSynchronizer
    {
        /// <summary>
        /// Creates a new tree folder and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        StateImageRadTreeNode AddFolder(StateImageRadTreeNode destinationFolderTreeNode, string folderName);

        /// <summary>
        /// Creates a new function tree node from the corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="newXmlFunctionNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode AddFunctionNode(StateImageRadTreeNode destinationFolderTreeNode, XmlElement newXmlFunctionNode);

        /// <summary>
        /// Creates a list of new function tree nodes from the corresponding XML nodes.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="newXmlFunctionNodes"></param>
        void AddFunctionNodes(StateImageRadTreeNode destinationFolderTreeNode, IList<XmlElement> newXmlFunctionNodes);

        /// <summary>
        /// Creates a new parameter tree node from the corresponding XML node.
        /// </summary>
        /// <param name="destinationFunctionTreeNode"></param>
        /// <param name="newXmlParameterNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode AddParameterNode(StateImageRadTreeNode destinationFunctionTreeNode, XmlElement newXmlParameterNode);

        /// <summary>
        /// Deletes a folder or function tree-node and the corresponding XML node.
        /// </summary>
        /// <param name="treeNode"></param>
        void DeleteNode(StateImageRadTreeNode treeNode);

        /// <summary>
        /// Deletes a function's parameter tree-node and the corresponding XML node.
        /// </summary>
        /// <param name="treeNode"></param>
        void DeleteParameterNode(StateImageRadTreeNode treeNode);

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
        void MoveFoldersFunctionsAndParameters(StateImageRadTreeNode destinationTreeNode, IList<StateImageRadTreeNode> movingTreeNodes);

        /// <summary>
        /// Moves a function tree-node and the corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveFunctionNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode);

        /// <summary>
        /// Moves a parameter node and corresponding XML node in front of another parameter node (used for changing the order or moving the parameter to a different function).
        /// </summary>
        /// <param name="destinationParameterTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveParameterBeforeParameter(StateImageRadTreeNode destinationParameterTreeNode, StateImageRadTreeNode movingTreeNode);

        /// <summary>
        /// Moves a parameter node and corresponding XML node to a different function
        /// </summary>
        /// <param name="destinationTreeFunctionNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns></returns>
        StateImageRadTreeNode? MoveParameterToFunction(StateImageRadTreeNode destinationTreeFunctionNode, StateImageRadTreeNode movingTreeNode);

        /// <summary>
        /// Replaces an existing treenode with a new one matching the new XML element.
        /// </summary>
        /// <param name="existingTreeNode"></param>
        /// <param name="newXmlFunctionNode"></param>
        void ReplaceFunctionNode(StateImageRadTreeNode existingTreeNode, XmlElement newXmlFunctionNode);
    }
}
