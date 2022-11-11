using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.TreeViewBuiilders;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services.TreeViewBuiilders
{
    internal class ConfigureFunctionGenericArgumentsTreeViewBuilder : IConfigureFunctionGenericArgumentsTreeViewBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IImageListService _imageListService;
        private readonly IStringHelper _stringHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConfigureFunctionGenericArgumentsTreeViewBuilder(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IImageListService imageListService,
            IStringHelper stringHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
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

            if (xmlDocument.DocumentElement?.Name != XmlDataConstants.FUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{6B2E6654-BA30-4E0E-89F2-C7866D247D48}");

            FunctionData functionData = _functionDataParser.Parse(xmlDocument.DocumentElement);

            if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out Function? function))
                throw _exceptionHelper.CriticalException("{F0A1222E-DD9A-4D5D-935B-F9F78C1F7BA0}");

            treeView.BeginUpdate();
            string rootNodeXPath = $"/{XmlDataConstants.FUNCTIONELEMENT}/{XmlDataConstants.GENERICARGUMENTSELEMENT}";
            RadTreeNode rootNode = new()
            {
                ImageIndex = ImageIndexes.TYPEIMAGEINDEX,
                Text = _stringHelper.ToShortName(function.TypeName),
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
