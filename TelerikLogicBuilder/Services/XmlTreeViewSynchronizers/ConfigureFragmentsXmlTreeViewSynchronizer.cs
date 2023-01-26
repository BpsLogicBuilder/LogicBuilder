using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ConfigureFragmentsXmlTreeViewSynchronizer : IConfigureFragmentsXmlTreeViewSynchronizer
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFragmentsFormTreeNodeComparer _fragmentsFormTreeNodeComparer;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsXmlTreeViewSynchronizer(
            IExceptionHelper exceptionHelper,
            IFragmentsFormTreeNodeComparer fragmentsFormTreeNodeComparer,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _exceptionHelper = exceptionHelper;
            _fragmentsFormTreeNodeComparer = fragmentsFormTreeNodeComparer;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public RadTreeNode AddFolder(RadTreeNode destinationFolderTreeNode, string folderName)
        {
            _xmlDocumentHelpers.SelectSingleElement
            (
                configureFragmentsForm.XmlDocument,
                destinationFolderTreeNode.Name
            )
            .AppendChild
            (
                _xmlDocumentHelpers.MakeElement
                (
                    configureFragmentsForm.XmlDocument,
                    XmlDataConstants.FOLDERELEMENT,
                    string.Empty,
                    new Dictionary<string, string>
                    {
                        [XmlDataConstants.NAMEATTRIBUTE] = folderName
                    }
                )
            );

            //we never add multiple folders so always validate
            configureFragmentsForm.ValidateXmlDocument();

            RadTreeNode newTreeNode = _treeViewService.CreateChildFolderTreeNode
            (
                destinationFolderTreeNode,
                XmlDataConstants.FOLDERELEMENT,
                ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                folderName
            );

            if (_treeViewService.IsRootNode(destinationFolderTreeNode))
                _treeViewService.MakeVisible(newTreeNode);

            InsertTreeNode(destinationFolderTreeNode, newTreeNode);
            configureFragmentsForm.SelectTreeNode(newTreeNode);

            return newTreeNode;
        }

        public RadTreeNode AddFragmentNode(RadTreeNode destinationFolderTreeNode, XmlElement newXmlFragmentNode)
        {
            RadTreeNode newFragmentNode = AddFragment(destinationFolderTreeNode, newXmlFragmentNode);
            configureFragmentsForm.SelectTreeNode(newFragmentNode);
            return newFragmentNode;
        }

        public void AddFragmentNodes(RadTreeNode destinationFolderTreeNode, IList<XmlElement> newXmlFragmentNodes)
        {
            string beforeXml = configureFragmentsForm.XmlDocument.OuterXml;
            try
            {
                AddFragments();
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                configureFragmentsForm.SelectTreeNode
                (
                    _treeViewService.GetTreeNodeByName
                    (
                        configureFragmentsForm.TreeView,
                        destinationFolderTreeNode.Name
                    )
                );

                throw;
            }

            void AddFragments()
            {
                List<RadTreeNode> newTreeNodes = new();
                foreach (XmlElement FragmentNode in newXmlFragmentNodes)
                    newTreeNodes.Add(AddFragment(destinationFolderTreeNode, FragmentNode, false));

                configureFragmentsForm.ValidateXmlDocument();

                foreach (RadTreeNode? newTreeNode in newTreeNodes)
                {
                    if (newTreeNode != null) newTreeNode.Selected = true;
                }

                if (!destinationFolderTreeNode.Expanded && destinationFolderTreeNode.Nodes.Count > 0)
                    destinationFolderTreeNode.Expand();
            }
        }

        public void DeleteNode(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsFolderNode(treeNode)
                && !_treeViewService.IsFileNode(treeNode))
            {
                throw _exceptionHelper.CriticalException("{2ABA428B-5D2C-4546-989E-A0BE528BF909}");
            }

            RadTreeNode? previousParent = treeNode.Parent;
            if (previousParent == null)
                throw _exceptionHelper.CriticalException("{60181DBC-07DC-40FB-99A7-D7FC503C05A7}");

            DeleteNode(treeNode, previousParent);
        }

        public RadTreeNode? MoveFolderNode(RadTreeNode destinationFolderTreeNode, RadTreeNode movingTreeNode, bool validate = true)
        {
            if (!_treeViewService.IsFolderNode(movingTreeNode))
                throw _exceptionHelper.CriticalException("{C1C68B05-D3F2-417A-9632-572CC5B7C926}");

            RadTreeNode previousParent = movingTreeNode.Parent ?? throw _exceptionHelper.CriticalException("{025B14C5-17D5-4FA8-A699-E85CA321E88A}");
            if (destinationFolderTreeNode == previousParent)
                return null;
            if (destinationFolderTreeNode.Name.StartsWith(movingTreeNode.Name))
                return null;

            return MoveFolder
            (
                _xmlDocumentHelpers.SelectSingleElement(configureFragmentsForm.XmlDocument, destinationFolderTreeNode.Name),
                _xmlDocumentHelpers.SelectSingleElement(configureFragmentsForm.XmlDocument, movingTreeNode.Name),
                movingTreeNode.Parent
            );

            RadTreeNode MoveFolder(XmlElement destinationFolderXmlNode, XmlElement movingXmlNode, RadTreeNode currentParentTreeNode)
            {
                movingXmlNode.ParentNode!.RemoveChild(movingXmlNode);/*XPath selection by tree node name will have a parent*/
                destinationFolderXmlNode.AppendChild(movingXmlNode);

                if (validate)
                    configureFragmentsForm.ValidateXmlDocument();

                configureFragmentsForm.TreeView.SelectedNode = null;/*prevent selection of new node upon remove.*/
                movingTreeNode.Remove();
                if (currentParentTreeNode.Nodes.Count == 0)
                    currentParentTreeNode.Collapse();

                if (movingTreeNode.Expanded)
                    configureFragmentsForm.ExpandedNodes.Remove(movingTreeNode.Name);

                _treeViewService.RenameChildFolderTreeNode
                (
                    destinationFolderTreeNode,
                    movingTreeNode,
                    movingXmlNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                );

                if (movingTreeNode.Expanded)
                    configureFragmentsForm.ExpandedNodes.Add(movingTreeNode.Name, movingTreeNode.Text);

                configureFragmentsForm.RenameChildNodes(movingTreeNode);

                InsertTreeNode(destinationFolderTreeNode, movingTreeNode);
                if (destinationFolderTreeNode.Expanded && destinationFolderTreeNode.Nodes.Count == 1)//Make sure After_Expand  (not necessary when there was an existing node - we also need to collapse after the single new node has been added otherwise collapse has no affect)
                    destinationFolderTreeNode.Collapse();//is triggered on the destination parent node. We don't want to set the node image here because images maybe different for extended types
                if (!destinationFolderTreeNode.Expanded)
                    destinationFolderTreeNode.Expand();

                return movingTreeNode;
            }
        }

        public void MoveFoldersAndFragments(RadTreeNode destinationFolderTreeNode, IList<RadTreeNode> movingTreeNodes)
        {
            if (!_treeViewService.IsFolderNode(destinationFolderTreeNode))
                return;

            string beforeXml = configureFragmentsForm.XmlDocument.OuterXml;
            try
            {
                MoveItems();
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                _treeViewService.SelectTreeNodes
                (
                    configureFragmentsForm.TreeView,
                    movingTreeNodes.Select(n => n.Name).ToArray()
                );

                throw;
            }

            void MoveItems()
            {
                List<RadTreeNode?> movedItems = movingTreeNodes.Aggregate
                (
                    new List<RadTreeNode?>(),
                    (list, movingItem) =>
                    {
                        if (_treeViewService.IsFolderNode(movingItem))
                        {
                            list.Add(MoveFolderNode(destinationFolderTreeNode, movingItem, false));
                        }
                        else if (_treeViewService.IsFileNode(movingItem))
                        {
                            list.Add
                            (
                                MoveFragmentNode
                                (
                                    destinationFolderTreeNode,
                                    movingItem,
                                    false
                                )
                            );
                        }

                        return list;
                    }
                );

                configureFragmentsForm.ValidateXmlDocument();

                foreach (RadTreeNode? movedItem in movedItems)
                {
                    if (movedItem != null) movedItem.Selected = true;
                }

                RadTreeNode? firstItem = movedItems.FirstOrDefault(m => m != null);
                if (firstItem != null)
                    configureFragmentsForm.SelectTreeNode(firstItem);
            }
        }

        public RadTreeNode? MoveFragmentNode(RadTreeNode destinationFolderTreeNode, RadTreeNode movingTreeNode, bool validate = true)
        {
            RadTreeNode previousParent = movingTreeNode.Parent ?? throw _exceptionHelper.CriticalException("{8ED2E233-726F-4659-A214-A631E8B3EA63}");

            if (!_treeViewService.IsFileNode(movingTreeNode))
                throw _exceptionHelper.CriticalException("{8D49E59E-6E77-4D0D-95CF-0214EA753864}");

            if (destinationFolderTreeNode == previousParent)
                return null;

            return MoveItem
            (
                _xmlDocumentHelpers.SelectSingleElement(configureFragmentsForm.XmlDocument, destinationFolderTreeNode.Name),
                _xmlDocumentHelpers.SelectSingleElement(configureFragmentsForm.XmlDocument, movingTreeNode.Name),
                movingTreeNode.Parent
            );

            RadTreeNode MoveItem(XmlElement destinationFolderXmlNode, XmlElement movingXmlNode, RadTreeNode currentParentTreeNode)
            {
                movingXmlNode.ParentNode!.RemoveChild(movingXmlNode);/*XPath selection by tree node name will have a parent*/
                destinationFolderXmlNode.PrependChild(movingXmlNode);

                if (validate)
                    configureFragmentsForm.ValidateXmlDocument();

                configureFragmentsForm.TreeView.SelectedNode = null;/*prevent selection of new node upon remove.*/
                movingTreeNode.Remove();
                if (currentParentTreeNode.Nodes.Count == 0)
                    currentParentTreeNode.Collapse();

                _treeViewService.RenameChildTreeNode
                (
                    destinationFolderTreeNode,
                    movingTreeNode,
                    movingXmlNode.Name,
                    XmlDataConstants.NAMEATTRIBUTE,
                    movingXmlNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                );

                InsertTreeNode(destinationFolderTreeNode, movingTreeNode);
                if (destinationFolderTreeNode.Expanded && destinationFolderTreeNode.Nodes.Count == 1)//Make sure After_Expand  (not necessary when there was an existing node - we also need to collapse after the single new node has been added otherwise collapse has no affect)
                    destinationFolderTreeNode.Collapse();//is triggered on the destination parent node. We don't want to set the node image here because images maybe different for extended types
                if (!destinationFolderTreeNode.Expanded)
                    destinationFolderTreeNode.Expand();

                return movingTreeNode;
            }
        }

        public void ReplaceFragmentNode(RadTreeNode existingTreeNode, XmlElement newXmlFragmentNode)
        {
            if (!_treeViewService.IsFileNode(existingTreeNode))
                throw _exceptionHelper.CriticalException("{1BA68A6A-316D-4BE2-9DB1-9B575C15F7DD}");

            if (newXmlFragmentNode.Name != XmlDataConstants.FRAGMENTELEMENT)
                throw _exceptionHelper.CriticalException("{AA1DEA30-69C4-4AFF-A21B-A336C22759C6}");

            RadTreeNode? parentNode = existingTreeNode.Parent;
            if (parentNode == null)
                throw _exceptionHelper.CriticalException("{E92FACC9-8474-4607-AA02-FDCE4C1FC2F1}");

            string beforeXml = configureFragmentsForm.XmlDocument.OuterXml;
            try
            {
                DeleteNode(existingTreeNode);
                RadTreeNode newFragmentNode = AddFragment(parentNode, newXmlFragmentNode);
                configureFragmentsForm.SelectTreeNode(newFragmentNode);
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                configureFragmentsForm.SelectTreeNode
                (
                    _treeViewService.GetTreeNodeByName
                    (
                        configureFragmentsForm.TreeView,
                        existingTreeNode.Name
                    )
                );

                throw;
            }
        }

        private RadTreeNode AddFragment(RadTreeNode destinationFolderTreeNode, XmlElement newXmlFragmentNode, bool validate = true)
        {
            RadTreeNode newTreeNode = _treeViewService.CreateChildTreeNode
            (
                destinationFolderTreeNode,
                newXmlFragmentNode.Name,
                XmlDataConstants.NAMEATTRIBUTE,
                newXmlFragmentNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                ImageIndexes.FILEIMAGEINDEX,
                Strings.fragmentNodeDescription
            );

            _xmlDocumentHelpers.SelectSingleElement
            (
                configureFragmentsForm.XmlDocument,
                destinationFolderTreeNode.Name
            )
            .PrependChild(newXmlFragmentNode);

            if (validate)
                configureFragmentsForm.ValidateXmlDocument();

            if (_treeViewService.IsRootNode(destinationFolderTreeNode))
                _treeViewService.MakeVisible(newTreeNode);

            InsertTreeNode(destinationFolderTreeNode, newTreeNode);

            return newTreeNode;
        }

        private void DeleteNode(RadTreeNode treeNode, RadTreeNode previousParent, bool validate = true)
        {
            XmlNode xmlNodeToDelete = _xmlDocumentHelpers.SelectSingleElement
            (
                configureFragmentsForm.XmlDocument,
                treeNode.Name
            );
            (
                xmlNodeToDelete.ParentNode ?? throw _exceptionHelper.CriticalException("{E55F32C9-6BC0-4C0F-AD55-FFABB6CEB0F7}")
            )
            .RemoveChild(xmlNodeToDelete);

            if (validate)
                configureFragmentsForm.ValidateXmlDocument();

            RadTreeNode closestNode = _treeViewService.GetClosestNodeForSelectionAfterDelete(treeNode);
            configureFragmentsForm.TreeView.SelectedNode = null;/*ensures no action SelectedNodeChanging*/
            treeNode.Remove();
            if (previousParent.Nodes.Count == 0)
                previousParent.Collapse();

            configureFragmentsForm.SelectTreeNode(closestNode);
        }

        private void InsertTreeNode(RadTreeNode destinationFolderTreeNode, RadTreeNode newTreeNode)
            => destinationFolderTreeNode.Nodes.Insert
            (
                _treeViewService.GetInsertPosition
                (
                    destinationFolderTreeNode.Nodes.ToArray(),
                    newTreeNode,
                    _fragmentsFormTreeNodeComparer
                ),
                newTreeNode
            );

        private void ResetFormOnError(string beforeXml, string exception)
        {
            configureFragmentsForm.ReloadXmlDocument(beforeXml);
            configureFragmentsForm.RebuildTreeView();
            configureFragmentsForm.SetErrorMessage(exception);
        }
    }
}
