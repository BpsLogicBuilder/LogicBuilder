using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers
{
    internal class ConfigureVariablesXmlTreeViewSynchronizer : IConfigureVariablesXmlTreeViewSynchronizer
    {
        private readonly IConfigurationFormXmlTreeViewSynchronizer _configurationFormXmlTreeViewSynchronizer;
        private readonly IConfigureVariablesStateImageSetter _configureVariablesStateImageSetter;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IVariablesFormTreeNodeComparer _variablesFormTreeNodeComparer;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesXmlTreeViewSynchronizer(
            IConfigureVariablesStateImageSetter configureVariablesStateImageSetter,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IVariablesFormTreeNodeComparer variablesFormTreeNodeComparer,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureVariablesForm configureVariablesForm)
        {
            _configurationFormXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigurationFormXmlTreeViewSynchronizer
            (
                configureVariablesForm,
                variablesFormTreeNodeComparer
            );
            _configureVariablesStateImageSetter = configureVariablesStateImageSetter;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _variablesFormTreeNodeComparer = variablesFormTreeNodeComparer;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureVariablesForm = configureVariablesForm;
        }

        public StateImageRadTreeNode AddFolder(StateImageRadTreeNode destinationFolderTreeNode, string folderName)
        {
            StateImageRadTreeNode newFolderNode = _configurationFormXmlTreeViewSynchronizer.AddFolder(destinationFolderTreeNode, folderName);
            configureVariablesForm.SelectTreeNode(newFolderNode);
            return newFolderNode;
        }

        public StateImageRadTreeNode AddVariableNode(StateImageRadTreeNode destinationFolderTreeNode, XmlElement newXmlVariableNode)
        {
            StateImageRadTreeNode newVariableNode = AddVariable(destinationFolderTreeNode, newXmlVariableNode);
            configureVariablesForm.SelectTreeNode(newVariableNode);
            return newVariableNode;
        }

        public void AddVariableNodes(StateImageRadTreeNode destinationFolderTreeNode, IList<XmlElement> newXmlVariableNodes)
        {
            string beforeXml = configureVariablesForm.XmlDocument.OuterXml;
            try
            {
                AddVariables();
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                configureVariablesForm.SelectTreeNode
                (
                    _treeViewService.GetTreeNodeByName
                    (
                        configureVariablesForm.TreeView,
                        destinationFolderTreeNode.Name
                    )
                );

                throw;
            }

            void AddVariables()
            {
                List<StateImageRadTreeNode> newTreeNodes = new();
                foreach (XmlElement variableNode in newXmlVariableNodes)
                    newTreeNodes.Add(AddVariable(destinationFolderTreeNode, variableNode, false));

                configureVariablesForm.ValidateXmlDocument();

                foreach (StateImageRadTreeNode? newTreeNode in newTreeNodes)
                {
                    if (newTreeNode != null) newTreeNode.Selected = true;
                }

                if (!destinationFolderTreeNode.Expanded && destinationFolderTreeNode.Nodes.Count > 0)
                    destinationFolderTreeNode.Expand();
            }
        }

        public void DeleteNode(StateImageRadTreeNode treeNode) 
            => _configurationFormXmlTreeViewSynchronizer.DeleteFolderOrConfiguredItem(treeNode);

        public StateImageRadTreeNode? MoveFolderNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedFolderNode = _configurationFormXmlTreeViewSynchronizer.MoveFolderNode(destinationFolderTreeNode, movingTreeNode);
            if (movedFolderNode != null)
                configureVariablesForm.SelectTreeNode(movedFolderNode);

            return movedFolderNode;
        }

        public void MoveFoldersAndVariables(StateImageRadTreeNode destinationFolderTreeNode, IList<StateImageRadTreeNode> movingTreeNodes)
        {
            if (!_treeViewService.IsFolderNode(destinationFolderTreeNode))
                return;

            string beforeXml = configureVariablesForm.XmlDocument.OuterXml;
            try
            {
                MoveItems();
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                _treeViewService.SelectTreeNodes
                (
                    configureVariablesForm.TreeView,
                    movingTreeNodes.Select(n => n.Name).ToArray()
                );

                throw;
            }


            void MoveItems()
            {
                List<StateImageRadTreeNode?> movedItems = movingTreeNodes.Aggregate
                (
                    new List<StateImageRadTreeNode?>(),
                    (list, movingItem) =>
                    {
                        if (_treeViewService.IsFolderNode(movingItem))
                        {
                            list.Add(_configurationFormXmlTreeViewSynchronizer.MoveFolderNode(destinationFolderTreeNode, movingItem, false));
                        }
                        else if (_treeViewService.IsVariableTypeNode(movingItem))
                        {
                            list.Add
                            (
                                _configurationFormXmlTreeViewSynchronizer.MoveConfiguredItem
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

                configureVariablesForm.ValidateXmlDocument();

                foreach (StateImageRadTreeNode? movedItem in movedItems)
                {
                    if (movedItem != null) movedItem.Selected = true;
                }

                StateImageRadTreeNode? firstItem = movedItems.FirstOrDefault(m => m != null);
                if (firstItem != null)
                    configureVariablesForm.SelectTreeNode(firstItem);
            }
        }

        public StateImageRadTreeNode? MoveVariableNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedVariableNode = _configurationFormXmlTreeViewSynchronizer.MoveConfiguredItem(destinationFolderTreeNode, movingTreeNode);
            if (movedVariableNode != null)
                configureVariablesForm.SelectTreeNode(movedVariableNode);

            return movedVariableNode;
        }

        public void ReplaceVariableNode(StateImageRadTreeNode existingTreeNode, XmlElement newXmlVariableNode)
        {
            if (!_treeViewService.IsVariableTypeNode(existingTreeNode))
                throw _exceptionHelper.CriticalException("{4BFF075A-3455-4D83-8231-2C99B95CB9C4}");

            if (
                    !new HashSet<string>
                    {
                        XmlDataConstants.LITERALVARIABLEELEMENT,
                        XmlDataConstants.OBJECTVARIABLEELEMENT,
                        XmlDataConstants.LITERALLISTVARIABLEELEMENT,
                        XmlDataConstants.OBJECTLISTVARIABLEELEMENT
                    }.Contains(newXmlVariableNode.Name)
                )
            {
                throw _exceptionHelper.CriticalException("{22071E31-2E32-45C6-A597-FEB0BBE21EFA}");
            }

            StateImageRadTreeNode? parentNode = (StateImageRadTreeNode?)existingTreeNode.Parent;
            if (parentNode == null)
                throw _exceptionHelper.CriticalException("{6CA6529D-C535-4256-A194-0DCA788BC178}");

            string beforeXml = configureVariablesForm.XmlDocument.OuterXml;
            try
            {
                _configurationFormXmlTreeViewSynchronizer.DeleteFolderOrConfiguredItem(existingTreeNode);
                StateImageRadTreeNode newVariableNode = AddVariable(parentNode, newXmlVariableNode);
                configureVariablesForm.SelectTreeNode(newVariableNode);
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                configureVariablesForm.SelectTreeNode
                (
                    _treeViewService.GetTreeNodeByName
                    (
                        configureVariablesForm.TreeView,
                        existingTreeNode.Name
                    )
                );

                throw;
            }
        }

        private void ResetFormOnError(string beforeXml, string exception)
        {
            configureVariablesForm.ReloadXmlDocument(beforeXml);
            configureVariablesForm.RebuildTreeView();
            configureVariablesForm.SetErrorMessage(exception);
        }

        private StateImageRadTreeNode AddVariable(StateImageRadTreeNode destinationFolderTreeNode, XmlElement newXmlVariableNode, bool validate = true)
        {
            StateImageRadTreeNode newTreeNode = _treeViewService.CreateChildTreeNode
            (
                destinationFolderTreeNode,
                newXmlVariableNode.Name,
                XmlDataConstants.NAMEATTRIBUTE,
                newXmlVariableNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                _xmlDocumentHelpers.GetImageIndex(newXmlVariableNode),
                _xmlDocumentHelpers.GetVariableTreeNodeDescription(newXmlVariableNode)
            );

            _xmlDocumentHelpers.SelectSingleElement
            (
                configureVariablesForm.XmlDocument,
                destinationFolderTreeNode.Name
            )
            .PrependChild(newXmlVariableNode);

            if (validate)
                configureVariablesForm.ValidateXmlDocument();

            if (_treeViewService.IsRootNode(destinationFolderTreeNode))
                _treeViewService.MakeVisible(newTreeNode);

            InsertTreeNode(destinationFolderTreeNode, newTreeNode);
            SetVariableStateImage(newXmlVariableNode, newTreeNode);

            return newTreeNode;
        }

        private void InsertTreeNode(RadTreeNode destinationFolderTreeNode, RadTreeNode newTreeNode)
        {
            destinationFolderTreeNode.Nodes.Insert
            (
                _treeViewService.GetInsertPosition
                (
                    destinationFolderTreeNode.Nodes.ToArray(),
                    newTreeNode,
                    _variablesFormTreeNodeComparer
                ),
                newTreeNode
            );
        }

        private void SetVariableStateImage(XmlElement variableElement, StateImageRadTreeNode treeNode)
        {
            _configureVariablesStateImageSetter.SetImage
            (
                variableElement,
                treeNode,
                configureVariablesForm.Application
            );
        }
    }
}
