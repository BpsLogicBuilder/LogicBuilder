using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class ConfigureConstructorGenericArgumentsTreeViewBuilder : IConfigureConstructorGenericArgumentsTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IImageListService _imageListService;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConfigureConstructorGenericArgumentsTreeViewBuilder(
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IExceptionHelper exceptionHelper,
            IImageListService imageListService,
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _exceptionHelper = exceptionHelper;
            _imageListService = imageListService;
            _stringHelper = stringHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Build(RadTreeView treeView, XmlDocument xmlDocument)
        {
            treeView.ShowRootLines = false;
            treeView.ImageList = _imageListService.ImageList;
            treeView.Nodes.Clear();
            if (xmlDocument.DocumentElement?.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                throw _exceptionHelper.CriticalException("{7F608B92-7420-4F31-86CB-34DFA63F7EF5}");

            ConstructorData constructorData = _constructorDataParser.Parse(xmlDocument.DocumentElement);

            if (!_configurationService.ConstructorList.Constructors.TryGetValue(constructorData.Name, out Constructor? constructor))
                throw _exceptionHelper.CriticalException("{A6DF6130-9A22-4FFA-9E49-E30F0610CD4B}");

            treeView.BeginUpdate();
            string rootNodeXPath = $"/{XmlDataConstants.CONSTRUCTORELEMENT}/{XmlDataConstants.GENERICARGUMENTSELEMENT}";
            RadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.TYPEIMAGEINDEX,
                Text = _stringHelper.ToShortName(constructor.TypeName),
                Name = rootNodeXPath
            };

            treeView.Nodes.Add(rootNode);
            AddGenericParameterNodes
            (
                _xmlDocumentHelpers.SelectSingleElement(xmlDocument, rootNodeXPath),
                rootNode
            );

            treeView.EndUpdate();
        }

        private void AddGenericParameterNodes(XmlElement genericArgumentsElement, RadTreeNode rootNode)
        {
            _xmlDocumentHelpers
                .GetChildElements(genericArgumentsElement)
                .ForEach
                (
                    element =>
                    {
                        RadTreeNode childNode = _treeViewService.AddChildTreeNode
                        (
                            rootNode,
                            element.Name,
                            XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE,
                            element.GetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE),
                            _xmlDocumentHelpers.GetImageIndex(element),
                            _xmlDocumentHelpers.GetGenericArgumentTreeNodeDescription(element)
                        );

                        _treeViewService.MakeVisible(childNode);
                    }
                );
        }
    }
}
