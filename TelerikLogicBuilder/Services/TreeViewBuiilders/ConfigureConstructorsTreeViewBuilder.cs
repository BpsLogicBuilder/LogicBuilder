using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class ConfigureConstructorsTreeViewBuilder : IConfigureConstructorsTreeViewBuilder
    {
        private readonly IConfigureConstructorsStateImageSetter _configureConstructorsStateImageSetter;
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IImageListService _imageListService;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsTreeViewBuilder(
            IConfigureConstructorsStateImageSetter configureConstructorsStateImageSetter,
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IImageListService imageListService,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _configureConstructorsStateImageSetter = configureConstructorsStateImageSetter;
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _imageListService = imageListService;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public void Build(RadTreeView treeView, XmlDocument xmlDocument)
        {
            treeView.ShowRootLines = false;
            treeView.ImageList = _imageListService.ImageList;
            treeView.Nodes.Clear();

            treeView.BeginUpdate();
            string rootNodeXPath = $"/{XmlDataConstants.FORMELEMENT}";
            StateImageRadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                Text = Strings.constructorsRootFolderText,
                Name = rootNodeXPath
            };

            treeView.Nodes.Add(rootNode);
            AddChildNodes
            (
                _xmlDocumentHelpers.SelectSingleElement(xmlDocument, rootNodeXPath),
                rootNode,
                true
            );

            treeView.BeginUpdate();
            treeView.EndUpdate();
        }

        private void AddChildNodes(XmlElement parentElement, StateImageRadTreeNode treeNode, bool root = false)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                parentElement,
                e => e.Name == XmlDataConstants.PARAMETERSELEMENT,
                e => e.SelectMany(o => o.ChildNodes.OfType<XmlElement>())//don't sort parameters
            )
            .ForEach
            (
                parameterElement =>
                {
                    StateImageRadTreeNode childTreeNode = _treeViewService.AddChildTreeNode
                    (
                        treeNode,
                        $"{XmlDataConstants.PARAMETERSELEMENT}/{parameterElement.Name}",
                        XmlDataConstants.NAMEATTRIBUTE,
                        parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                        _xmlDocumentHelpers.GetImageIndex(parameterElement),
                        _xmlDocumentHelpers.GetParameterTreeNodeDescription(parameterElement)
                    );

                    _configureParametersStateImageSetter.SetImage(parameterElement, childTreeNode, configureConstructorsForm.Application);
                }
            );

            _xmlDocumentHelpers.GetChildElements
            (
                parentElement,
                e => e.Name == XmlDataConstants.CONSTRUCTORELEMENT,
                en => en.OrderBy(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            )
            .ForEach
            (
                constructorElement =>
                {
                    StateImageRadTreeNode childTreeNode = _treeViewService.AddChildTreeNode
                    (
                        treeNode,
                        constructorElement.Name,
                        XmlDataConstants.NAMEATTRIBUTE,
                        constructorElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                        ImageIndexes.CONSTRUCTORIMAGEINDEX,
                        Strings.constructorNodeDescription
                    );

                    _configureConstructorsStateImageSetter.SetImage(constructorElement, childTreeNode, configureConstructorsForm.Application);
                    AddChildNodes(constructorElement, childTreeNode);
                    if (root) _treeViewService.MakeVisible(childTreeNode);
                    if (configureConstructorsForm.ExpandedNodes.ContainsKey(childTreeNode.Name))
                        treeNode.Expand();
                }
            );

            _xmlDocumentHelpers.GetChildElements
            (
                parentElement,
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                en => en.OrderBy(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            )
            .ForEach
            (
                folderElement =>
                {
                    StateImageRadTreeNode childTreeNode = _treeViewService.CreateChildFolderTreeNode
                    (
                        treeNode,
                        folderElement.Name,
                        ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                        folderElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
                    );

                    childTreeNode.StateImage = Properties.Resources.CheckMark;
                    treeNode.Nodes.Add(childTreeNode);
                    AddChildNodes(folderElement, childTreeNode);
                    if (root) _treeViewService.MakeVisible(childTreeNode);
                    if (configureConstructorsForm.ExpandedNodes.ContainsKey(childTreeNode.Name))
                        treeNode.Expand();
                }
            );
        }
    }
}
