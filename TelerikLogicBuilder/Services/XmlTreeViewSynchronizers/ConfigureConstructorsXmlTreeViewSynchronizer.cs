using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
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
    internal class ConfigureConstructorsXmlTreeViewSynchronizer : IConfigureConstructorsXmlTreeViewSynchronizer
    {
        private readonly IConfigurationFormXmlTreeViewSynchronizer _configurationFormXmlTreeViewSynchronizer;
        private readonly IConfigureConstructorsStateImageSetter _configureConstructorsStateImageSetter;
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IConstructorsFormTreeNodeComparer _constructorsFormTreeNodeComparer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsXmlTreeViewSynchronizer(
            IConfigureConstructorsStateImageSetter configureConstructorsStateImageSetter,
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IConstructorsFormTreeNodeComparer constructorsFormTreeNodeComparer,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _configurationFormXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigurationFormXmlTreeViewSynchronizer
            (
                configureConstructorsForm,
                constructorsFormTreeNodeComparer
            );
            _configureConstructorsStateImageSetter = configureConstructorsStateImageSetter;
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _constructorsFormTreeNodeComparer = constructorsFormTreeNodeComparer;
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public StateImageRadTreeNode AddConstructorNode(StateImageRadTreeNode destinationFolderTreeNode, XmlElement newXmlConstructorNode)
        {
            StateImageRadTreeNode newConstructorNode = AddConstructor(destinationFolderTreeNode, newXmlConstructorNode);
            configureConstructorsForm.SelectTreeNode(newConstructorNode);
            return newConstructorNode;
        }

        public void AddConstructorNodes(StateImageRadTreeNode destinationFolderTreeNode, IList<XmlElement> newXmlConstructorNodes)
        {
            string beforeXml = configureConstructorsForm.XmlDocument.OuterXml;
            try
            {
                AddConstructors();
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                configureConstructorsForm.SelectTreeNode
                (
                    _treeViewService.GetTreeNodeByName
                    (
                        configureConstructorsForm.TreeView, 
                        destinationFolderTreeNode.Name
                    )
                );

                throw;
            }

            void AddConstructors()
            {
                List<StateImageRadTreeNode> newTreeNodes = new();
                foreach (XmlElement constructorNode in newXmlConstructorNodes)
                    newTreeNodes.Add(AddConstructor(destinationFolderTreeNode, constructorNode, false));

                configureConstructorsForm.ValidateXmlDocument();

                foreach (StateImageRadTreeNode? newTreeNode in newTreeNodes)
                {
                    if (newTreeNode != null) newTreeNode.Selected = true;
                }

                if (!destinationFolderTreeNode.Expanded && destinationFolderTreeNode.Nodes.Count > 0)
                    destinationFolderTreeNode.Expand();
            }
        }

        public StateImageRadTreeNode AddFolder(StateImageRadTreeNode destinationFolderTreeNode, string folderName)
        {
            StateImageRadTreeNode newFolderNode = _configurationFormXmlTreeViewSynchronizer.AddFolder(destinationFolderTreeNode, folderName);
            configureConstructorsForm.SelectTreeNode(newFolderNode);
            return newFolderNode;
        }

        public StateImageRadTreeNode AddParameterNode(StateImageRadTreeNode destinationConstructorTreeNode, XmlElement newXmlParameterNode)
        {
            StateImageRadTreeNode newParameterNode = _configurationFormXmlTreeViewSynchronizer.AddParameterNode(destinationConstructorTreeNode, newXmlParameterNode);
            configureConstructorsForm.SelectTreeNode(newParameterNode);
            return newParameterNode;
        }

        public void DeleteNode(StateImageRadTreeNode treeNode)
            => _configurationFormXmlTreeViewSynchronizer.DeleteFolderOrConfiguredItem(treeNode);

        public void DeleteParameterNode(StateImageRadTreeNode treeNode)
            => _configurationFormXmlTreeViewSynchronizer.DeleteParameterNode(treeNode);

        public StateImageRadTreeNode? MoveConstructorNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedConstructorNode = _configurationFormXmlTreeViewSynchronizer.MoveConfiguredItem(destinationFolderTreeNode, movingTreeNode);
            if (movedConstructorNode != null)
                configureConstructorsForm.SelectTreeNode(movedConstructorNode);

            return movedConstructorNode;
        }

        public StateImageRadTreeNode? MoveFolderNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedFolderNode = _configurationFormXmlTreeViewSynchronizer.MoveFolderNode(destinationFolderTreeNode, movingTreeNode);
            if (movedFolderNode != null)
                configureConstructorsForm.SelectTreeNode(movedFolderNode);

            return movedFolderNode;
        }

        public void MoveFoldersConstructorsAndParameters(StateImageRadTreeNode destinationTreeNode, IList<StateImageRadTreeNode> movingTreeNodes)
        {
            string beforeXml = configureConstructorsForm.XmlDocument.OuterXml;
            try
            {
                if (_treeViewService.IsFolderNode(destinationTreeNode))
                    ValidateXmlAndSelectNewItems(MoveItemsToFolder());
                else if (_treeViewService.IsConstructorNode(destinationTreeNode))
                    ValidateXmlAndSelectNewItems(MoveItemsToConstructor());
                else if (_treeViewService.IsParameterNode(destinationTreeNode))
                    ValidateXmlAndSelectNewItems(MoveItemsBeforeParameter());
                else
                    throw _exceptionHelper.CriticalException("{FB316E4E-0CC1-49D8-AEE4-3E59082EFF40}");
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                _treeViewService.SelectTreeNodes
                (
                    configureConstructorsForm.TreeView,
                    movingTreeNodes.Select(n => n.Name).ToArray()
                );

                throw;
            }

            List<StateImageRadTreeNode?> MoveItemsToFolder() 
                => movingTreeNodes.Aggregate
                (
                    new List<StateImageRadTreeNode?>(),
                    (list, movingItem) =>
                    {
                        if (_treeViewService.IsFolderNode(movingItem))
                        {
                            list.Add(_configurationFormXmlTreeViewSynchronizer.MoveFolderNode(destinationTreeNode, movingItem, false));
                        }
                        else if (_treeViewService.IsConstructorNode(movingItem))
                        {
                            list.Add
                            (
                                _configurationFormXmlTreeViewSynchronizer.MoveConfiguredItem
                                (
                                    destinationTreeNode,
                                    movingItem,
                                    false
                                )
                            );
                        }

                        return list;
                    }
                );

            List<StateImageRadTreeNode?> MoveItemsToConstructor() 
                => movingTreeNodes.Aggregate
                (
                    new List<StateImageRadTreeNode?>(),
                    (list, movingItem) =>
                    {
                        if (_treeViewService.IsParameterNode(movingItem))
                            list.Add(_configurationFormXmlTreeViewSynchronizer.MoveParameterToConfiguredItem(destinationTreeNode, movingItem, false));

                        return list;
                    }
                );

            List<StateImageRadTreeNode?> MoveItemsBeforeParameter() 
                => movingTreeNodes.Aggregate
                (
                    new List<StateImageRadTreeNode?>(),
                    (list, movingItem) =>
                    {
                        if (_treeViewService.IsParameterNode(movingItem))
                            list.Add(_configurationFormXmlTreeViewSynchronizer.MoveParameterBeforeParameter(destinationTreeNode, movingItem, false));

                        return list;
                    }
                );

            void ValidateXmlAndSelectNewItems(List<StateImageRadTreeNode?> movedItems)
            {
                configureConstructorsForm.ValidateXmlDocument();

                foreach (StateImageRadTreeNode? movedItem in movedItems)
                {
                    if (movedItem != null) movedItem.Selected = true;
                }

                StateImageRadTreeNode? firstItem = movedItems.FirstOrDefault(m => m != null);
                if (firstItem != null)
                    configureConstructorsForm.SelectTreeNode(firstItem);
            }
        }

        public StateImageRadTreeNode? MoveParameterBeforeParameter(StateImageRadTreeNode destinationParameterTreeNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedParameterNode = _configurationFormXmlTreeViewSynchronizer.MoveParameterBeforeParameter(destinationParameterTreeNode, movingTreeNode);
            if (movedParameterNode != null)
                configureConstructorsForm.SelectTreeNode(movedParameterNode);

            return movedParameterNode;
        }

        public StateImageRadTreeNode? MoveParameterToConstructor(StateImageRadTreeNode destinationTreeConstructorNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedParameterNode = _configurationFormXmlTreeViewSynchronizer.MoveParameterToConfiguredItem(destinationTreeConstructorNode, movingTreeNode);
            if (movedParameterNode != null)
                configureConstructorsForm.SelectTreeNode(movedParameterNode);

            return movedParameterNode;
        }

        public void ReplaceConstructorNode(StateImageRadTreeNode existingTreeNode, XmlElement newXmlConstructorNode)
        {
            if (!_treeViewService.IsConstructorNode(existingTreeNode))
                throw _exceptionHelper.CriticalException("{C690B840-F884-4A9E-B661-F8A24A418F9F}");

            if (newXmlConstructorNode.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                throw _exceptionHelper.CriticalException("{B6ED42EA-1C3B-45D1-897A-BD05E0DD51E5}");

            StateImageRadTreeNode? parentNode = (StateImageRadTreeNode?)existingTreeNode.Parent;
            if (parentNode == null)
                throw _exceptionHelper.CriticalException("{80A6604D-D34A-4B63-BB0D-6EB503B6CAFE}");

            string beforeXml = configureConstructorsForm.XmlDocument.OuterXml;
            try
            {
                _configurationFormXmlTreeViewSynchronizer.DeleteFolderOrConfiguredItem(existingTreeNode);
                StateImageRadTreeNode newConstructorNode = AddConstructor(parentNode, newXmlConstructorNode);
                configureConstructorsForm.SelectTreeNode(newConstructorNode);
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                configureConstructorsForm.SelectTreeNode
                (
                    _treeViewService.GetTreeNodeByName
                    (
                        configureConstructorsForm.TreeView,
                        existingTreeNode.Name
                    )
                );

                throw;
            }
        }

        private StateImageRadTreeNode AddConstructor(StateImageRadTreeNode destinationFolderTreeNode, XmlElement newXmlConstructorNode, bool validate = true)
        {
            StateImageRadTreeNode newTreeNode = _treeViewService.CreateChildTreeNode
            (
                destinationFolderTreeNode,
                newXmlConstructorNode.Name,
                XmlDataConstants.NAMEATTRIBUTE,
                newXmlConstructorNode.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                ImageIndexes.CONSTRUCTORIMAGEINDEX,
                Strings.constructorNodeDescription
            );

            _xmlDocumentHelpers.SelectSingleElement
            (
                configureConstructorsForm.XmlDocument,
                destinationFolderTreeNode.Name
            )
            .PrependChild(newXmlConstructorNode);

            if (validate)
                configureConstructorsForm.ValidateXmlDocument();

            if (_treeViewService.IsRootNode(destinationFolderTreeNode))
                _treeViewService.MakeVisible(newTreeNode);

            InsertTreeNode(destinationFolderTreeNode, newTreeNode);
            SetConstructorStateImage(newXmlConstructorNode, newTreeNode);

            //add parameters to the tree view.
            _xmlDocumentHelpers.GetChildElements
            (
                newXmlConstructorNode,
                e => e.Name == XmlDataConstants.PARAMETERSELEMENT,
                e => e.SelectMany(o => o.ChildNodes.OfType<XmlElement>())
            )
            .ForEach
            (
                parameterElement =>
                {
                    StateImageRadTreeNode parameterTreeNode = _treeViewService.AddChildTreeNode
                    (
                        newTreeNode,
                        $"{XmlDataConstants.PARAMETERSELEMENT}/{parameterElement.Name}",
                        XmlDataConstants.NAMEATTRIBUTE,
                        parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                        _xmlDocumentHelpers.GetImageIndex(parameterElement),
                        _xmlDocumentHelpers.GetParameterTreeNodeDescription(parameterElement)
                    );

                    _configureParametersStateImageSetter.SetImage(parameterElement, parameterTreeNode, configureConstructorsForm.Application);
                }
            );

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
                    _constructorsFormTreeNodeComparer
                ),
                newTreeNode
            );
        }

        private void ResetFormOnError(string beforeXml, string exception)
        {
            configureConstructorsForm.ReloadXmlDocument(beforeXml);
            configureConstructorsForm.RebuildTreeView();
            configureConstructorsForm.SetErrorMessage(exception);
        }

        private void SetConstructorStateImage(XmlElement constructorElement, StateImageRadTreeNode treeNode)
        {
            _configureConstructorsStateImageSetter.SetImage
            (
                constructorElement,
                treeNode,
                configureConstructorsForm.Application
            );
        }
    }
}
