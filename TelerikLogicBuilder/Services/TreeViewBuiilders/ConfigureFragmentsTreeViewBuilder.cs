using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class ConfigureFragmentsTreeViewBuilder : IConfigureFragmentsTreeViewBuilder
    {
        private readonly IImageListService _imageListService;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsTreeViewBuilder(
            IImageListService imageListService,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _imageListService = imageListService;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public void Build(RadTreeView treeView, XmlDocument xmlDocument)
        {
            treeView.ShowRootLines = false;
            treeView.ImageList = _imageListService.ImageList;
            treeView.TreeViewElement.ShowNodeToolTips = true;
            treeView.Nodes.Clear();

            treeView.BeginUpdate();
            string rootNodeXPath = $"/{XmlDataConstants.FOLDERELEMENT}";
            RadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                Text = Strings.fragmentsRootNodeText,
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

        private void AddChildNodes(XmlElement folderElement, RadTreeNode treeNode, bool root = false)
        {
            _xmlDocumentHelpers.GetChildElements
            (
                folderElement,
                e => e.Name == XmlDataConstants.FRAGMENTELEMENT,
                en => en.OrderBy(i => i.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
            )
            .ForEach
            (
                fragmentElement =>
                {
                    RadTreeNode childTreeNode = _treeViewService.AddChildTreeNode
                    (
                        treeNode,
                        fragmentElement.Name,
                        XmlDataConstants.NAMEATTRIBUTE,
                        fragmentElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                        ImageIndexes.FILEIMAGEINDEX,
                        Strings.fragmentNodeDescription
                    );

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
                    RadTreeNode childFolderTreeNode = _treeViewService.CreateChildTreeNode
                    (
                        treeNode,
                        childFolderElement.Name,
                        XmlDataConstants.NAMEATTRIBUTE,
                        childFolderElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                        ImageIndexes.CLOSEDFOLDERIMAGEINDEX
                    );

                    treeNode.Nodes.Add(childFolderTreeNode);
                    AddChildNodes(childFolderElement, childFolderTreeNode);
                    if (root) _treeViewService.MakeVisible(childFolderTreeNode);
                    if (configureFragmentsForm.ExpandedNodes.ContainsKey(childFolderTreeNode.Name))
                        childFolderTreeNode.Expand();
                }
            );
        }
    }
}
