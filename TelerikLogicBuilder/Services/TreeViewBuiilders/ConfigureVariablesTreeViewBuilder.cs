using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class ConfigureVariablesTreeViewBuilder : IConfigureVariablesTreeViewBuilder
    {
        private readonly IConfigureVariablesStateImageSetter _configureVariablesStateImageSetter;
        private readonly IImageListService _imageListService;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesTreeViewBuilder(
            IConfigureVariablesStateImageSetter configureVariablesStateImageSetter,
            IImageListService imageListService,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureVariablesForm configureVariablesForm)
        {
            _configureVariablesStateImageSetter = configureVariablesStateImageSetter;
            _imageListService = imageListService;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureVariablesForm = configureVariablesForm;
        }

        public void Build(RadTreeView treeView, XmlDocument xmlDocument)
        {
            treeView.ShowRootLines = false;
            treeView.ImageList = _imageListService.ImageList;
            treeView.TreeViewElement.ShowNodeToolTips = true;
            treeView.Nodes.Clear();

            treeView.BeginUpdate();
            string rootNodeXPath = $"/{XmlDataConstants.FOLDERELEMENT}";
            StateImageRadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                Text = Strings.variablesRootNodeText,
                Name = rootNodeXPath
            };

            treeView.Nodes.Add(rootNode);
            AddChildNodes
            (
                _xmlDocumentHelpers.SelectSingleElement(xmlDocument, rootNodeXPath),
                rootNode,
                true
            );

            treeView.EndUpdate();
        }

        private void AddChildNodes(XmlElement folderElement, StateImageRadTreeNode treeNode, bool root = false)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                folderElement,
                e => new HashSet<string> { XmlDataConstants.LITERALVARIABLEELEMENT, XmlDataConstants.OBJECTVARIABLEELEMENT, XmlDataConstants.LITERALLISTVARIABLEELEMENT, XmlDataConstants.OBJECTLISTVARIABLEELEMENT }.Contains(e.Name),
                en => en.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
            )
            .ForEach
            (
                variableElement =>
                {
                    StateImageRadTreeNode childTreeNode = _treeViewService.AddChildTreeNode
                    (
                        treeNode,
                        variableElement.Name,
                        XmlDataConstants.NAMEATTRIBUTE,
                        variableElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                        _xmlDocumentHelpers.GetImageIndex(variableElement),
                        _xmlDocumentHelpers.GetVariableTreeNodeDescription(variableElement)
                    );

                    _configureVariablesStateImageSetter.SetImage(variableElement, childTreeNode, configureVariablesForm.Application);

                    if (root) _treeViewService.MakeVisible(childTreeNode);
                }
            );

            _xmlDocumentHelpers.GetChildElements
            (
                folderElement,
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                en => en.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
            )
            .ForEach
            (
                childFolderElement =>
                {
                    StateImageRadTreeNode childFolderTreeNode = _treeViewService.CreateChildTreeNode
                    (
                        treeNode,
                        childFolderElement.Name,
                        XmlDataConstants.NAMEATTRIBUTE,
                        childFolderElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                        ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                    );

                    childFolderTreeNode.StateImage = Properties.Resources.CheckMark;
                    treeNode.Nodes.Add(childFolderTreeNode);
                    AddChildNodes(childFolderElement, childFolderTreeNode);
                    if (root) _treeViewService.MakeVisible(childFolderTreeNode);
                    if (configureVariablesForm.ExpandedNodes.ContainsKey(childFolderTreeNode.Name))
                        childFolderTreeNode.Expand();
                }
            );
        }
    }
}
