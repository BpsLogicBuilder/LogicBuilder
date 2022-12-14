using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
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
    internal class ConfigureFunctionsXmlTreeViewSynchronizer : IConfigureFunctionsXmlTreeViewSynchronizer
    {
        private readonly IConfigurationFormXmlTreeViewSynchronizer _configurationFormXmlTreeViewSynchronizer;
        private readonly IConfigureFunctionsStateImageSetter _configureFunctionsStateImageSetter;
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionsFormTreeNodeComparer _functionsFormTreeNodeComparer;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsXmlTreeViewSynchronizer(
            IConfigureFunctionsStateImageSetter configureFunctionsStateImageSetter,
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IExceptionHelper exceptionHelper,
            IFunctionsFormTreeNodeComparer functionsFormTreeNodeComparer,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _configurationFormXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigurationFormXmlTreeViewSynchronizer
            (
                configureFunctionsForm,
                functionsFormTreeNodeComparer
            );
            _configureFunctionsStateImageSetter = configureFunctionsStateImageSetter;
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _exceptionHelper = exceptionHelper;
            _functionsFormTreeNodeComparer = functionsFormTreeNodeComparer;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public StateImageRadTreeNode AddFolder(StateImageRadTreeNode destinationFolderTreeNode, string folderName)
        {
            StateImageRadTreeNode newFolderNode = _configurationFormXmlTreeViewSynchronizer.AddFolder(destinationFolderTreeNode, folderName);
            configureFunctionsForm.TreeView.SelectedNode = newFolderNode;
            return newFolderNode;
        }

        public StateImageRadTreeNode AddFunctionNode(StateImageRadTreeNode destinationFolderTreeNode, XmlElement newXmlFunctionNode)
        {
            StateImageRadTreeNode newFunctionNode = AddFunction(destinationFolderTreeNode, newXmlFunctionNode);
            configureFunctionsForm.TreeView.SelectedNode = newFunctionNode;
            return newFunctionNode;
        }

        public void AddFunctionNodes(StateImageRadTreeNode destinationFolderTreeNode, IList<XmlElement> newXmlFunctionNodes)
        {
            string beforeXml = configureFunctionsForm.XmlDocument.OuterXml;
            try
            {
                AddFunctions();
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                _treeViewService.SelectTreeNode(configureFunctionsForm.TreeView, destinationFolderTreeNode.Name);
            }

            void AddFunctions()
            {
                List<StateImageRadTreeNode> newTreeNodes = new();
                foreach (XmlElement constructorNode in newXmlFunctionNodes)
                    newTreeNodes.Add(AddFunction(destinationFolderTreeNode, constructorNode, false));

                configureFunctionsForm.ValidateXmlDocument();

                foreach (StateImageRadTreeNode? newTreeNode in newTreeNodes)
                {
                    if (newTreeNode != null) newTreeNode.Selected = true;
                }
            }
        }

        public StateImageRadTreeNode AddParameterNode(StateImageRadTreeNode destinationFunctionTreeNode, XmlElement newXmlParameterNode)
        {
            StateImageRadTreeNode newParameterNode = _configurationFormXmlTreeViewSynchronizer.AddParameterNode(destinationFunctionTreeNode, newXmlParameterNode);
            configureFunctionsForm.TreeView.SelectedNode = newParameterNode;
            return newParameterNode;
        }

        public void DeleteNode(StateImageRadTreeNode treeNode)
            => _configurationFormXmlTreeViewSynchronizer.DeleteFolderOrConfiguredItem(treeNode);

        public void DeleteParameterNode(StateImageRadTreeNode treeNode)
            => _configurationFormXmlTreeViewSynchronizer.DeleteParameterNode(treeNode);

        public StateImageRadTreeNode? MoveFolderNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedFolderNode = _configurationFormXmlTreeViewSynchronizer.MoveFolderNode(destinationFolderTreeNode, movingTreeNode);
            if (movedFolderNode != null)
                configureFunctionsForm.TreeView.SelectedNode = movedFolderNode;

            return movedFolderNode;
        }

        public void MoveFoldersFunctionsAndParameters(StateImageRadTreeNode destinationTreeNode, IList<StateImageRadTreeNode> movingTreeNodes)
        {
            string beforeXml = configureFunctionsForm.XmlDocument.OuterXml;
            try
            {
                if (_treeViewService.IsFolderNode(destinationTreeNode))
                    ValidateXmlAndSelectNewItems(MoveItemsToFolder());
                else if (_treeViewService.IsMethodNode(destinationTreeNode))
                    ValidateXmlAndSelectNewItems(MoveItemsToFunction());
                else if (_treeViewService.IsParameterNode(destinationTreeNode))
                    ValidateXmlAndSelectNewItems(MoveItemsBeforeParameter());
                else
                    throw _exceptionHelper.CriticalException("{85FC3433-F147-4871-973B-905E11F23629}");
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                _treeViewService.SelectTreeNodes
                (
                    configureFunctionsForm.TreeView,
                    movingTreeNodes.Select(n => n.Name).ToArray()
                );
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
                        else if (_treeViewService.IsMethodNode(movingItem))
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

            List<StateImageRadTreeNode?> MoveItemsToFunction()
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
                configureFunctionsForm.ValidateXmlDocument();

                foreach (StateImageRadTreeNode? movedItem in movedItems)
                {
                    if (movedItem != null) movedItem.Selected = true;
                }
            }
        }

        public StateImageRadTreeNode? MoveFunctionNode(StateImageRadTreeNode destinationFolderTreeNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedFunctionNode = _configurationFormXmlTreeViewSynchronizer.MoveConfiguredItem(destinationFolderTreeNode, movingTreeNode);
            if (movedFunctionNode != null)
                configureFunctionsForm.TreeView.SelectedNode = movedFunctionNode;

            return movedFunctionNode;
        }

        public StateImageRadTreeNode? MoveParameterBeforeParameter(StateImageRadTreeNode destinationParameterTreeNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedParameterNode = _configurationFormXmlTreeViewSynchronizer.MoveParameterBeforeParameter(destinationParameterTreeNode, movingTreeNode);
            if (movedParameterNode != null)
                configureFunctionsForm.TreeView.SelectedNode = movedParameterNode;

            return movedParameterNode;
        }

        public StateImageRadTreeNode? MoveParameterToFunction(StateImageRadTreeNode destinationTreeFunctionNode, StateImageRadTreeNode movingTreeNode)
        {
            StateImageRadTreeNode? movedParameterNode = _configurationFormXmlTreeViewSynchronizer.MoveParameterToConfiguredItem(destinationTreeFunctionNode, movingTreeNode);
            if (movedParameterNode != null)
                configureFunctionsForm.TreeView.SelectedNode = movedParameterNode;

            return movedParameterNode;
        }

        public void ReplaceFunctionNode(StateImageRadTreeNode existingTreeNode, XmlElement newXmlFunctionNode)
        {
            if (!_treeViewService.IsMethodNode(existingTreeNode))
                throw _exceptionHelper.CriticalException("{17CD3E64-CBBB-4FD9-8D46-DF5D0516C8B3}");

            if (newXmlFunctionNode.Name != XmlDataConstants.FUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{9B8A931C-1E95-4CA6-ABE6-294A32868B1C}");

            StateImageRadTreeNode? parentNode = (StateImageRadTreeNode?)existingTreeNode.Parent;
            if (parentNode == null)
                throw _exceptionHelper.CriticalException("{C129570A-D13B-45F5-A9CA-603CFEF516D1}");

            string beforeXml = configureFunctionsForm.XmlDocument.OuterXml;
            try
            {
                _configurationFormXmlTreeViewSynchronizer.DeleteFolderOrConfiguredItem(existingTreeNode);
                StateImageRadTreeNode newVariableNode = AddFunction(parentNode, newXmlFunctionNode);
                configureFunctionsForm.TreeView.SelectedNode = newVariableNode;
            }
            catch (LogicBuilderException ex)
            {
                ResetFormOnError(beforeXml, ex.Message);
                _treeViewService.SelectTreeNode(configureFunctionsForm.TreeView, existingTreeNode.Name);
            }
        }

        private StateImageRadTreeNode AddFunction(StateImageRadTreeNode destinationFolderTreeNode, XmlElement newXmlFunctionNode, bool validate = true)
        {
            StateImageRadTreeNode newTreeNode = _treeViewService.CreateChildTreeNode
            (
                destinationFolderTreeNode,
                newXmlFunctionNode.Name,
                XmlDataConstants.NAMEATTRIBUTE,
                newXmlFunctionNode.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                ImageIndexes.METHODIMAGEINDEX,
                Strings.constructorNodeDescription
            );

            _xmlDocumentHelpers.SelectSingleElement
            (
                configureFunctionsForm.XmlDocument,
                destinationFolderTreeNode.Name
            )
            .PrependChild(newXmlFunctionNode);

            if (validate)
                configureFunctionsForm.ValidateXmlDocument();

            if (_treeViewService.IsRootNode(destinationFolderTreeNode))
                _treeViewService.MakeVisible(newTreeNode);

            InsertTreeNode(destinationFolderTreeNode, newTreeNode);
            SetFunctionStateImage(newXmlFunctionNode, newTreeNode);

            //add parameters to the tree view.
            _xmlDocumentHelpers.GetChildElements
            (
                newXmlFunctionNode,
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
                        parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                        _xmlDocumentHelpers.GetImageIndex(parameterElement),
                        _xmlDocumentHelpers.GetParameterTreeNodeDescription(parameterElement)
                    );

                    _configureParametersStateImageSetter.SetImage(parameterElement, parameterTreeNode, configureFunctionsForm.Application);
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
                    _functionsFormTreeNodeComparer
                ),
                newTreeNode
            );
        }

        private void ResetFormOnError(string beforeXml, string exception)
        {
            configureFunctionsForm.ReloadXmlDocument(beforeXml);
            configureFunctionsForm.RebuildTreeView();
            configureFunctionsForm.SetErrorMessage(exception);
        }

        private void SetFunctionStateImage(XmlElement functionElement, StateImageRadTreeNode treeNode)
        {
            _configureFunctionsStateImageSetter.SetImage
            (
                functionElement,
                treeNode,
                configureFunctionsForm.Application
            );
        }
    }
}
