using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ConfigurationFormXmlTreeViewSynchronizer : IConfigurationFormXmlTreeViewSynchronizer
    {
        private readonly IConfigurationFolderStateImageSetter _configurationFolderStateImageSetter;
        private readonly IConfigureConstructorsStateImageSetter _configureConstructorsStateImageSetter;
        private readonly IConfigureFunctionsStateImageSetter _configureFunctionsStateImageSetter;
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigurationForm configurationForm;
        private readonly IComparer<RadTreeNode> treeNodeComparer;

        public ConfigurationFormXmlTreeViewSynchronizer(
            IConfigurationFolderStateImageSetter configurationFolderStateImageSetter,
            IConfigureConstructorsStateImageSetter configureConstructorsStateImageSetter,
            IConfigureFunctionsStateImageSetter configureFunctionsStateImageSetter,
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigurationForm configurationForm,
            IComparer<RadTreeNode> treeNodeComparer)
        {
            _configurationFolderStateImageSetter = configurationFolderStateImageSetter;
            _configureConstructorsStateImageSetter = configureConstructorsStateImageSetter;
            _configureFunctionsStateImageSetter = configureFunctionsStateImageSetter;
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configurationForm = configurationForm;
            this.treeNodeComparer = treeNodeComparer;
        }

        public StateImageRadTreeNode AddFolder(StateImageRadTreeNode destinationFolderTreeNode, string folderName)
        {
            _xmlDocumentHelpers.SelectSingleElement
            (
                configurationForm.XmlDocument,
                destinationFolderTreeNode.Name
            )
            .AppendChild
            (
                _xmlDocumentHelpers.MakeElement
                (
                    configurationForm.XmlDocument,
                    XmlDataConstants.FOLDERELEMENT,
                    string.Empty,
                    new Dictionary<string, string> 
                    { 
                        [XmlDataConstants.NAMEATTRIBUTE] = folderName 
                    }
                )
            );

            //we never add multiple folders so always validate
            configurationForm.ValidateXmlDocument();

            StateImageRadTreeNode newTreeNode = _treeViewService.CreateChildFolderTreeNode
            (
                destinationFolderTreeNode,
                XmlDataConstants.FOLDERELEMENT,
                ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                folderName
            );

            if (_treeViewService.IsRootNode(destinationFolderTreeNode))
                _treeViewService.MakeVisible(newTreeNode);

            InsertTreeNode(destinationFolderTreeNode, newTreeNode);
            _configurationFolderStateImageSetter.SetImage(newTreeNode);

            return newTreeNode;
        }

        public StateImageRadTreeNode AddParameterNode(StateImageRadTreeNode destinationTreeNode, XmlElement newXmlParameterNode)
        {
            if (!_treeViewService.IsConstructorNode(destinationTreeNode)
                && !_treeViewService.IsMethodNode(destinationTreeNode))
            {
                throw _exceptionHelper.CriticalException("{04804962-6167-4D85-8E4C-04339C6E9BFB}");
            }

            _xmlDocumentHelpers.SelectSingleElement
            (
                configurationForm.XmlDocument,
                $"{destinationTreeNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}"
            )
            .AppendChild(newXmlParameterNode);

            //we never add multiple parameters so always validate
            configurationForm.ValidateXmlDocument();

            StateImageRadTreeNode newTreeNode = _treeViewService.AddChildTreeNode
            (
                destinationTreeNode,
                $"{XmlDataConstants.PARAMETERSELEMENT}/{newXmlParameterNode.Name}",
                XmlDataConstants.NAMEATTRIBUTE,
                newXmlParameterNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                _xmlDocumentHelpers.GetImageIndex(newXmlParameterNode),
                _xmlDocumentHelpers.GetParameterTreeNodeDescription(newXmlParameterNode)
            );

            _configureParametersStateImageSetter.SetImage(newXmlParameterNode, newTreeNode, configurationForm.Application);
            SetParameterParentStateImage(destinationTreeNode);
            _treeViewService.MakeVisible(newTreeNode);

            return newTreeNode;
        }

        public void DeleteFolderOrConfiguredItem(StateImageRadTreeNode treeNode, bool validate = true)
        {
            if (!_treeViewService.IsFolderNode(treeNode)
                && !_treeViewService.IsConstructorNode(treeNode)
                && !_treeViewService.IsMethodNode(treeNode)
                && !_treeViewService.IsVariableTypeNode(treeNode))
            {
                throw _exceptionHelper.CriticalException("{07633C74-CE73-4F96-8712-FD55DB17042D}");
            }

            StateImageRadTreeNode? previousParent = (StateImageRadTreeNode?)treeNode.Parent;
            if (previousParent == null)
                throw _exceptionHelper.CriticalException("{AA830475-19AE-433C-BE20-61FC912E4BDB}");

            DeleteNode(treeNode, previousParent, validate);

            _configurationFolderStateImageSetter.SetImage(previousParent);
        }

        public void DeleteParameterNode(StateImageRadTreeNode treeNode)
        {
            if (!_treeViewService.IsParameterNode(treeNode))
                throw _exceptionHelper.CriticalException("{F3FFF3DE-C24C-40A9-A49B-CE613B4540DE}");

            StateImageRadTreeNode? previousParent = (StateImageRadTreeNode?)treeNode.Parent;
            if (previousParent == null)
                throw _exceptionHelper.CriticalException("{6C6128E4-620C-4997-B69D-F45107F45985}");

            DeleteNode(treeNode, previousParent, true);
            SetParameterParentStateImage(previousParent);
        }

        public StateImageRadTreeNode? MoveConfiguredItem(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode, bool validate = true)
        {
            StateImageRadTreeNode previousParent = (StateImageRadTreeNode)(movingTreeNode.Parent ?? throw _exceptionHelper.CriticalException("{E66B0790-818F-4437-9D73-4478FE85FCA0}"));

            if (!_treeViewService.IsConstructorNode(movingTreeNode)
                && !_treeViewService.IsMethodNode(movingTreeNode)
                && !_treeViewService.IsVariableTypeNode(movingTreeNode))
            {
                throw _exceptionHelper.CriticalException("{BC8F6485-507E-499E-AA71-6E92C9F0849C}");
            }

            if (destinationFolderTreeNode == previousParent)
                return null;

            return MoveItem
            (
                _xmlDocumentHelpers.SelectSingleElement(configurationForm.XmlDocument, destinationFolderTreeNode.Name),
                _xmlDocumentHelpers.SelectSingleElement(configurationForm.XmlDocument, movingTreeNode.Name),
                (StateImageRadTreeNode)movingTreeNode.Parent
            );

            StateImageRadTreeNode MoveItem(XmlElement destinationFolderXmlNode, XmlElement movingXmlNode, StateImageRadTreeNode currentParentTreeNode)
            {
                movingXmlNode.ParentNode!.RemoveChild(movingXmlNode);/*XPath selection by tree node name will have a parent*/
                destinationFolderXmlNode.PrependChild(movingXmlNode);

                if (validate)
                    configurationForm.ValidateXmlDocument();

                configurationForm.TreeView.SelectedNode = null;/*prevent selection of new node upon remove.*/
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

                _configurationFolderStateImageSetter.SetImage(destinationFolderTreeNode);
                _configurationFolderStateImageSetter.SetImage(previousParent);

                return movingTreeNode;
            }
        }

        public StateImageRadTreeNode? MoveFolderNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode, bool validate = true)
        {
            if (!_treeViewService.IsFolderNode(movingTreeNode))
                throw _exceptionHelper.CriticalException("{89DC81C3-EA13-4296-977C-D1DA7D006574}");

            StateImageRadTreeNode previousParent = (StateImageRadTreeNode)(movingTreeNode.Parent ?? throw _exceptionHelper.CriticalException("{E66B0790-818F-4437-9D73-4478FE85FCA0}"));
            if (destinationFolderTreeNode == previousParent)
                return null;
            if (destinationFolderTreeNode.Name.StartsWith(movingTreeNode.Name))
                return null;

            return MoveFolder
            (
                _xmlDocumentHelpers.SelectSingleElement(configurationForm.XmlDocument, destinationFolderTreeNode.Name),
                _xmlDocumentHelpers.SelectSingleElement(configurationForm.XmlDocument, movingTreeNode.Name),
                (StateImageRadTreeNode)movingTreeNode.Parent
            );

            StateImageRadTreeNode MoveFolder(XmlElement destinationFolderXmlNode, XmlElement movingXmlNode, StateImageRadTreeNode currentParentTreeNode)
            {
                movingXmlNode.ParentNode!.RemoveChild(movingXmlNode);/*XPath selection by tree node name will have a parent*/
                destinationFolderXmlNode.AppendChild(movingXmlNode);

                if (validate)
                    configurationForm.ValidateXmlDocument();

                configurationForm.TreeView.SelectedNode = null;/*prevent selection of new node upon remove.*/
                movingTreeNode.Remove();
                if (currentParentTreeNode.Nodes.Count == 0)
                    currentParentTreeNode.Collapse();

                if (movingTreeNode.Expanded)
                    configurationForm.ExpandedNodes.Remove(movingTreeNode.Name);

                _treeViewService.RenameChildFolderTreeNode
                (
                    destinationFolderTreeNode,
                    movingTreeNode,
                    movingXmlNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                );

                if (movingTreeNode.Expanded)
                    configurationForm.ExpandedNodes.Add(movingTreeNode.Name, movingTreeNode.Text);

                configurationForm.RenameChildNodes(movingTreeNode);

                InsertTreeNode(destinationFolderTreeNode, movingTreeNode);
                if (destinationFolderTreeNode.Expanded && destinationFolderTreeNode.Nodes.Count == 1)//Make sure After_Expand  (not necessary when there was an existing node - we also need to collapse after the single new node has been added otherwise collapse has no affect)
                    destinationFolderTreeNode.Collapse();//is triggered on the destination parent node. We don't want to set the node image here because images maybe different for extended types
                if (!destinationFolderTreeNode.Expanded)
                    destinationFolderTreeNode.Expand();

                _configurationFolderStateImageSetter.SetImage(destinationFolderTreeNode);
                _configurationFolderStateImageSetter.SetImage(previousParent);

                return movingTreeNode;
            }
        }

        public StateImageRadTreeNode? MoveParameterBeforeParameter(StateImageRadTreeNode destinationParameterTreeNode, StateImageRadTreeNode movingTreeNode, bool validate = true)
        {
            if (movingTreeNode == destinationParameterTreeNode)
                return null;

            if (!_treeViewService.IsParameterNode(destinationParameterTreeNode))
                return null;

            return MoveParameter
            (
                _xmlDocumentHelpers.SelectSingleElement(configurationForm.XmlDocument, movingTreeNode.Name),
                _xmlDocumentHelpers.SelectSingleElement(configurationForm.XmlDocument, destinationParameterTreeNode.Name),
                (StateImageRadTreeNode)(destinationParameterTreeNode.Parent ?? throw _exceptionHelper.CriticalException("{493DDBBF-3AED-4179-9B0B-C901A8885F7F}")),
                (StateImageRadTreeNode)(movingTreeNode.Parent ?? throw _exceptionHelper.CriticalException("{CE5A350F-D961-4820-8F7A-89FD081B157F}"))
            );

            StateImageRadTreeNode MoveParameter(XmlElement movingXmlParameterNode, XmlElement destinationXmlParameterNode, StateImageRadTreeNode destParentTreeNode, StateImageRadTreeNode previousParent)
            {
                destinationXmlParameterNode.ParentNode!.InsertBefore
                (
                    movingXmlParameterNode.ParentNode!.RemoveChild(movingXmlParameterNode),
                    destinationXmlParameterNode
                );

                if (validate)
                    configurationForm.ValidateXmlDocument();

                configurationForm.TreeView.SelectedNode = null;/*prevent selection of new node upon remove.*/
                movingTreeNode.Remove();
                _treeViewService.RenameChildTreeNode
                (
                    destParentTreeNode,
                    movingTreeNode,
                    $"{XmlDataConstants.PARAMETERSELEMENT}/{movingXmlParameterNode.Name}",
                    XmlDataConstants.NAMEATTRIBUTE,
                    movingXmlParameterNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                );

                destParentTreeNode.Nodes.Insert(destinationParameterTreeNode.Index, movingTreeNode);
                _configureParametersStateImageSetter.SetImage(movingXmlParameterNode, movingTreeNode, configurationForm.Application);
                SetParameterParentStateImage(destParentTreeNode);
                if (previousParent != destParentTreeNode)
                    SetParameterParentStateImage(previousParent);

                return movingTreeNode;
            }
        }

        public StateImageRadTreeNode? MoveParameterToConfiguredItem(StateImageRadTreeNode destinationTreeNode, StateImageRadTreeNode movingTreeNode, bool validate = true)
        {
            if (movingTreeNode.Parent == destinationTreeNode)
                return null;

            if (!_treeViewService.IsConstructorNode(destinationTreeNode)
                && !_treeViewService.IsMethodNode(destinationTreeNode))
                return null;

            return MoveParameter
            (
                _xmlDocumentHelpers.SelectSingleElement(configurationForm.XmlDocument, movingTreeNode.Name),
                _xmlDocumentHelpers.SelectSingleElement(configurationForm.XmlDocument, $"{destinationTreeNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}")
            );

            StateImageRadTreeNode MoveParameter(XmlElement movingXmlParameterNode, XmlElement destinationXmlConfiguredItemNode)
            {
                destinationXmlConfiguredItemNode.AppendChild(movingXmlParameterNode.ParentNode!.RemoveChild(movingXmlParameterNode));
                if (validate)
                    configurationForm.ValidateXmlDocument();

                configurationForm.TreeView.SelectedNode = null;/*prevent selection of new node upon remove.*/
                movingTreeNode.Remove();
                _treeViewService.RenameChildTreeNode
                (
                    destinationTreeNode,
                    movingTreeNode,
                    $"{XmlDataConstants.PARAMETERSELEMENT}/{movingXmlParameterNode.Name}",
                    XmlDataConstants.NAMEATTRIBUTE,
                    movingXmlParameterNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                );
                destinationTreeNode.Nodes.Add(movingTreeNode);
                _configureParametersStateImageSetter.SetImage(movingXmlParameterNode, movingTreeNode, configurationForm.Application);
                SetParameterParentStateImage(destinationTreeNode);

                return movingTreeNode;
            }
        }

        private void DeleteNode(StateImageRadTreeNode treeNode, StateImageRadTreeNode previousParent, bool validate = true)
        {
            XmlNode xmlNodeToDelete = _xmlDocumentHelpers.SelectSingleElement
            (
                configurationForm.XmlDocument,
                treeNode.Name
            );
            (
                xmlNodeToDelete.ParentNode ?? throw _exceptionHelper.CriticalException("{81D466E0-7ECA-4F51-A2C3-A93072D3BB68}")
            )
            .RemoveChild(xmlNodeToDelete);

            if (validate)
                configurationForm.ValidateXmlDocument();

            RadTreeNode closestNode = _treeViewService.GetClosestNodeForSelectionAfterDelete(treeNode);
            configurationForm.TreeView.SelectedNode = null;/*ensures no action SelectedNodeChanging*/
            treeNode.Remove();
            if (previousParent.Nodes.Count == 0)
                previousParent.Collapse();

            configurationForm.SelectTreeNode(closestNode);
        }

        private void InsertTreeNode(RadTreeNode destinationFolderTreeNode, RadTreeNode newTreeNode) 
            => destinationFolderTreeNode.Nodes.Insert
            (
                _treeViewService.GetInsertPosition
                (
                    destinationFolderTreeNode.Nodes.ToArray(),
                    newTreeNode,
                    this.treeNodeComparer
                ),
                newTreeNode
            );

        private void SetParameterParentStateImage(StateImageRadTreeNode parentTreeNode)
        {
            if (_treeViewService.IsConstructorNode(parentTreeNode))
                SetParentConstructorStateImage(parentTreeNode);
            else if (_treeViewService.IsMethodNode(parentTreeNode))
                SetParentFunctionStateImage(parentTreeNode);
            else
                throw _exceptionHelper.CriticalException("{FE150552-7C00-4656-837A-4C1532A26BE0}");
        }

        private void SetParentFunctionStateImage(StateImageRadTreeNode parentTreeNode) 
            => _configureFunctionsStateImageSetter.SetImage
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    configurationForm.XmlDocument,
                    parentTreeNode.Name
                ),
                parentTreeNode,
                configurationForm.Application
            );

        private void SetParentConstructorStateImage(StateImageRadTreeNode parentTreeNode) 
            => _configureConstructorsStateImageSetter.SetImage
            (
                _xmlDocumentHelpers.SelectSingleElement
                (
                    configurationForm.XmlDocument,
                    parentTreeNode.Name
                ),
                parentTreeNode,
                configurationForm.Application
            );
    }
}
