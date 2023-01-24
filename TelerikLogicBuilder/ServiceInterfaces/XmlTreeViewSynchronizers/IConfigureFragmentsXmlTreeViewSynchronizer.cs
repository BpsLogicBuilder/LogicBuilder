using System.Collections.Generic;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers
{
    internal interface IConfigureFragmentsXmlTreeViewSynchronizer
    {
        /// <summary>
        /// Creates a new tree folder and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="folderName"></param>
        /// <returns></returns>
        RadTreeNode AddFolder(RadTreeNode destinationFolderTreeNode, string folderName);

        /// <summary>
        /// Creates a new fragment tree node from the corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="newXmlFragmentNode"></param>
        /// <returns></returns>
        RadTreeNode AddFragmentNode(RadTreeNode destinationFolderTreeNode, XmlElement newXmlFragmentNode);

        /// <summary>
        /// Creates a list of new fragment tree nodes from the corresponding XML nodes.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="newXmlFragmentNodes"></param>
        void AddFragmentNodes(RadTreeNode destinationFolderTreeNode, IList<XmlElement> newXmlFragmentNodes);

        /// <summary>
        /// Deletes the tree node and the corresponding XML node.
        /// </summary>
        /// <param name="treeNode"></param>
        void DeleteNode(RadTreeNode treeNode);

        /// <summary>
        /// Moves a list of nodes to the destination folder
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNodes"></param>
        void MoveFoldersAndFragments(RadTreeNode destinationFolderTreeNode, IList<RadTreeNode> movingTreeNodes);

        /// <summary>
        /// Moves a folder node and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns></returns>
        RadTreeNode? MoveFolderNode(RadTreeNode destinationFolderTreeNode, RadTreeNode movingTreeNode, bool validate = true);

        /// <summary>
        /// Moves a fragment node and corresponding XML node.
        /// </summary>
        /// <param name="destinationFolderTreeNode"></param>
        /// <param name="movingTreeNode"></param>
        /// <returns></returns>
        RadTreeNode? MoveFragmentNode(RadTreeNode destinationFolderTreeNode, RadTreeNode movingTreeNode, bool validate = true);

        /// <summary>
        /// Replaces an existing fragment treenode with a new one matching the new XML element.
        /// </summary>
        /// <param name="existingTreeNode"></param>
        /// <param name="newXmlFragmentNode"></param>
        void ReplaceFragmentNode(RadTreeNode existingTreeNode, XmlElement newXmlFragmentNode);
    }
}
