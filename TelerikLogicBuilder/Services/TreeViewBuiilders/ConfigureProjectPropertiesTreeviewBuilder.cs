using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class ConfigureProjectPropertiesTreeViewBuilder : IConfigureProjectPropertiesTreeViewBuilder
    {
        private readonly IImageListService _imageListService;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConfigureProjectPropertiesTreeViewBuilder(
            IImageListService imageListService,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _imageListService = imageListService;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Build(RadTreeView treeView, XmlDocument xmlDocument)
        {
            treeView.ShowRootLines = false;
            treeView.ImageList = _imageListService.ImageList;
            treeView.Nodes.Clear();

            treeView.BeginUpdate();
            string rootNodeXPath = $"/{XmlDataConstants.PROJECTPROPERTIESELEMENT}/{XmlDataConstants.APPLICATIONSELEMENT}";
            RadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.PROJECTFOLDERIMAGEINDEX,
                Text = Strings.projectPropertiesRootNodeText,
                Name = rootNodeXPath
            };

            treeView.Nodes.Add(rootNode);
            AddApplicationNodes
            (
                _xmlDocumentHelpers.SelectSingleElement(xmlDocument, rootNodeXPath),
                rootNode
            );
            treeView.EndUpdate();
        }

        private void AddApplicationNodes(XmlElement applicationsElement, RadTreeNode rootNode)
        {
            _xmlDocumentHelpers
                .GetChildElements
                (
                    applicationsElement,
                    null, 
                    en => en.OrderBy(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
                )
                .ForEach
                (
                    element =>
                    {
                        RadTreeNode childNode = _treeViewService.AddChildTreeNode
                        (
                            rootNode,
                            element.Name,
                            XmlDataConstants.NAMEATTRIBUTE,
                            element.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                            ImageIndexes.APPLICATIONIMAGEINDEX
                        );

                        _treeViewService.MakeVisible(childNode);
                    }
                );
        }
    }
}
