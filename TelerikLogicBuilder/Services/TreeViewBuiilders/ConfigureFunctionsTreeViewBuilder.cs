using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
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
    internal class ConfigureFunctionsTreeViewBuilder : IConfigureFunctionsTreeViewBuilder
    {
        private readonly IConfigureFunctionsStateImageSetter _configureFunctionsStateImageSetter;
        private readonly IConfigureParametersStateImageSetter _configureParametersStateImageSetter;
        private readonly IImageListService _imageListService;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsTreeViewBuilder(
            IConfigureFunctionsStateImageSetter configureFunctionsStateImageSetter,
            IConfigureParametersStateImageSetter configureParametersStateImageSetter,
            IImageListService imageListService,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _configureFunctionsStateImageSetter = configureFunctionsStateImageSetter;
            _configureParametersStateImageSetter = configureParametersStateImageSetter;
            _imageListService = imageListService;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public void Build(RadTreeView treeView, XmlDocument xmlDocument)
        {
            treeView.ShowRootLines = false;
            treeView.ImageList = _imageListService.ImageList;
            treeView.Nodes.Clear();

            treeView.BeginUpdate();
            string rootNodeXPath = $"/forms/form[@name='{XmlDataConstants.FUNCTIONSFORMROOTNODENAME}']/folder[@name='{XmlDataConstants.FUNCTIONSROOTFOLDERNAMEATTRIBUTE}']";
            StateImageRadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.CLOSEDFOLDERIMAGEINDEX,
                Text = Strings.functionsRootFolderText,
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
                        parameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                        _xmlDocumentHelpers.GetImageIndex(parameterElement),
                        _xmlDocumentHelpers.GetParameterTreeNodeDescription(parameterElement)
                    );

                    _configureParametersStateImageSetter.SetImage(parameterElement, childTreeNode, configureFunctionsForm.Application);
                }
            );

            _xmlDocumentHelpers.GetChildElements
            (
                parentElement,
                e => e.Name == XmlDataConstants.FUNCTIONELEMENT,
                en => en.OrderBy(e => e.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
            )
            .ForEach
            (
                functionElement =>
                {
                    //<function><returnType><literal><literalType>Void</literalType></literal></returnType></function>
                    XmlElement returnTypeElement = functionElement.ChildNodes.OfType<XmlElement>()
                                                    .Single(e => e.Name == XmlDataConstants.RETURNTYPEELEMENT)//returnType
                                                    .ChildNodes.OfType<XmlElement>()
                                                    .Single();//literal/object/generic/literalList/objectList/genericList

                    StateImageRadTreeNode childTreeNode = _treeViewService.AddChildTreeNode
                    (
                        treeNode,
                        functionElement.Name,
                        XmlDataConstants.NAMEATTRIBUTE,
                        functionElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                        ImageIndexes.METHODIMAGEINDEX,
                        _xmlDocumentHelpers.GetFunctionTreeNodeDescription(returnTypeElement)
                    );

                    _configureFunctionsStateImageSetter.SetImage(functionElement, childTreeNode, configureFunctionsForm.Application);
                    AddChildNodes(functionElement, childTreeNode);
                    if (root) _treeViewService.MakeVisible(childTreeNode);
                    if (configureFunctionsForm.ExpandedNodes.ContainsKey(childTreeNode.Name))
                        treeNode.Expand();
                }
            );

            _xmlDocumentHelpers.GetChildElements
            (
                parentElement,
                e => e.Name == XmlDataConstants.FOLDERELEMENT,
                en => en.OrderBy(e => e.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value)
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
                        folderElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value
                    );

                    childTreeNode.StateImage = Properties.Resources.CheckMark;
                    treeNode.Nodes.Add(childTreeNode);
                    AddChildNodes(folderElement, childTreeNode);
                    if (root) _treeViewService.MakeVisible(childTreeNode);
                    if (configureFunctionsForm.ExpandedNodes.ContainsKey(childTreeNode.Name))
                        treeNode.Expand();
                }
            );
        }
    }
}
