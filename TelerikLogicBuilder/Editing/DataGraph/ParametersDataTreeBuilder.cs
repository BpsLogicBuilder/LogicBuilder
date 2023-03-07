using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph
{
    internal class ParametersDataTreeBuilder : IParametersDataTreeBuilder
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IDataGraphTreeViewHelper _dataGraphTreeViewHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly IImageListService _imageListService;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly ILiteralListParameterElementInfoHelper _literalListElementInfoHelper;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IObjectListParameterElementInfoHelper _objectListElementInfoHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditingForm editingForm;

        public ParametersDataTreeBuilder(
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IDataGraphTreeViewHelper dataGraphTreeViewHelper,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IGetValidConfigurationFromData getValidConfigurationFromData,
            IImageListService imageListService,
            ILiteralListDataParser literalListDataParser,
            ILiteralListParameterElementInfoHelper literalListElementInfoHelper,
            IObjectListDataParser objectListDataParser,
            IObjectListParameterElementInfoHelper objectListElementInfoHelper,
            ITreeViewService treeViewService,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditingForm editingForm)
        {
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _dataGraphTreeViewHelper = dataGraphTreeViewHelper;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _imageListService = imageListService;
            _literalListDataParser = literalListDataParser;
            _literalListElementInfoHelper = literalListElementInfoHelper;
            _objectListDataParser = objectListDataParser;
            _objectListElementInfoHelper = objectListElementInfoHelper;
            _treeViewService = treeViewService;
            _typeLoadHelper = typeLoadHelper;
            _variableDataParser = variableDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editingForm = editingForm;
        }

        private const string CONSTRUCTOR_ROOT_XPATH = $"/{XmlDataConstants.CONSTRUCTORELEMENT}";
        private const string FUNCTION_ROOT_XPATH = $"/{XmlDataConstants.FUNCTIONELEMENT}";
        private const string LITERALLIST_ROOT_XPATH = $"/{XmlDataConstants.LITERALLISTELEMENT}";
        private const string OBJECTLIST_ROOT_XPATH = $"/{XmlDataConstants.OBJECTLISTELEMENT}";

        private static readonly HashSet<string> RootXPaths = new()
        {
            CONSTRUCTOR_ROOT_XPATH,
            FUNCTION_ROOT_XPATH,
            LITERALLIST_ROOT_XPATH,
            OBJECTLIST_ROOT_XPATH
        };

        public void CreateConstructorTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType)
        {
            treeView.BeginUpdate();
            treeView.ImageList = _imageListService.ImageList;
            treeView.TreeViewElement.ShowNodeToolTips = true;
            treeView.ShowRootLines = true;
            treeView.Nodes.Clear();
            XmlElement constructorElement = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, CONSTRUCTOR_ROOT_XPATH);
            if (!_configurationService.ConstructorList.Constructors.TryGetValue(constructorElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), out Constructor? constructor))
                return;

            GetConstructorChildren
            (
                constructorElement,
                _dataGraphTreeViewHelper.AddRootConstructorTreeNode(treeView, constructorElement, rootAssignedToType, constructor.ToString()),
                true
            );

            treeView.EndUpdate();
        }

        public void CreateFunctionTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType)
        {
            treeView.BeginUpdate();
            treeView.ImageList = _imageListService.ImageList;
            treeView.TreeViewElement.ShowNodeToolTips = true;
            treeView.ShowRootLines = true;
            treeView.Nodes.Clear();
            XmlElement functionElement = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, FUNCTION_ROOT_XPATH);
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), out Function? function))
                return;

            GetFunctionChildren
            (
                functionElement,
                _dataGraphTreeViewHelper.AddRootFunctionTreeNode(treeView, functionElement, rootAssignedToType, function.ToString()),
                true
            );

            treeView.EndUpdate();
        }

        public void CreateLiteralListTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType, LiteralListParameterElementInfo literalListInfo)
        {
            treeView.BeginUpdate();
            treeView.ImageList = _imageListService.ImageList;
            treeView.TreeViewElement.ShowNodeToolTips = true;
            treeView.ShowRootLines = true;
            treeView.Nodes.Clear();

            XmlElement literalListElement = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, LITERALLIST_ROOT_XPATH);
            LiteralListData literalListData = _literalListDataParser.Parse(literalListElement, literalListInfo, editingForm);
            GetLiteralListChildren
            (
                literalListElement,
                _dataGraphTreeViewHelper.AddRootLiteralListTreeNode
                (
                    treeView, 
                    literalListElement, 
                    rootAssignedToType, 
                    literalListInfo, 
                    literalListData.VisibleText, 
                    literalListData.VisibleText
                ),
                literalListData,
                true
            );

            treeView.EndUpdate();
        }

        public void CreateObjectListTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType, ObjectListParameterElementInfo objectListInfo)
        {
            treeView.BeginUpdate();
            treeView.ImageList = _imageListService.ImageList;
            treeView.TreeViewElement.ShowNodeToolTips = true;
            treeView.ShowRootLines = true;
            treeView.Nodes.Clear();

            XmlElement objectListElement = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, OBJECTLIST_ROOT_XPATH);
            ObjectListData objectListData = _objectListDataParser.Parse(objectListElement, objectListInfo, editingForm);
            GetObjectListChildren
            (
                objectListElement,
                _dataGraphTreeViewHelper.AddRootObjectListTreeNode
                (
                    treeView,
                    objectListElement,
                    rootAssignedToType,
                    objectListInfo,
                    objectListData.VisibleText,
                    objectListData.VisibleText
                ),
                objectListData,
                true
            );

            treeView.EndUpdate();
        }

        public void RefreshTreeNode(RadTreeView treeView, XmlDocument xmlDocument, ParametersDataTreeNode node)
        {
            bool root = RootXPaths.Contains(node.Name);
            treeView.BeginUpdate();
            node.Nodes.Clear();
            XmlElement xmlElement = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, node.Name);

            switch (node)
            {
                case ConstructorElementTreeNode constructorElementTreeNode:
                    if (!_configurationService.ConstructorList.Constructors.TryGetValue(xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), out Constructor? constructor))
                        return;

                    node.ToolTipText = constructor.ToString();

                    GetConstructorChildren
                    (
                        xmlElement,
                        constructorElementTreeNode,
                        root
                    );
                    break;
                case FunctionElementTreeNode functionElementTreeNode:
                    if (!_configurationService.FunctionList.Functions.TryGetValue(xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), out Function? function))
                        return;

                    node.ToolTipText = function.ToString();

                    GetFunctionChildren
                    (
                        xmlElement,
                        functionElementTreeNode,
                        root
                    );
                    break;
                case LiteralListElementTreeNode literalListElementTreeNode:
                    LiteralListData literalListData = _literalListDataParser.Parse(xmlElement, literalListElementTreeNode.ListInfo, editingForm);
                    if (!literalListElementTreeNode.ListInfo.HasParameter)//if there's a parameter the ListInfo comes from the configured parameter.  Do not update.
                    {//Otherwise it is derived from the possibly updated LiteralListData
                        literalListElementTreeNode.ListInfo = _literalListElementInfoHelper.GetLiteralListElementInfo(literalListData);
                    }

                    literalListElementTreeNode.Text = literalListData.VisibleText;

                    GetLiteralListChildren
                    (
                        xmlElement,
                        literalListElementTreeNode,
                        literalListData,
                        root
                    );
                    break;
                case ObjectListElementTreeNode objectListElementTreeNode:
                    ObjectListData objectListData = _objectListDataParser.Parse(xmlElement, objectListElementTreeNode.ListInfo, editingForm);
                    if (!objectListElementTreeNode.ListInfo.HasParameter)//if there's a parameter the ListInfo comes from the configured parameter.  Do not update.
                    {//Otherwise it is derived from the possibly updated ObjectListData
                        objectListElementTreeNode.ListInfo = _objectListElementInfoHelper.GetObjectListElementInfo(objectListData);
                    }

                    objectListElementTreeNode.Text = objectListData.VisibleText;

                    GetObjectListChildren
                    (
                        xmlElement,
                        objectListElementTreeNode,
                        objectListData,
                        root
                    );
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{560FFF1F-A83D-40B9-8BDB-12DD6076506B}");
            }

            treeView.EndUpdate();
        }

        #region Private Methods
        private void AddConstructorNode(ParametersDataTreeNode parentTreeNode, XmlElement constructorElement)
        {
            ConstructorData constructorData = _constructorDataParser.Parse(constructorElement);
            if (!_getValidConfigurationFromData.TryGetConstructor(constructorData, editingForm.Application, out Constructor? constructor))
                return;

            InitConstructorNode
            (
                _dataGraphTreeViewHelper.AddConstructorTreeNode
                (
                    parentTreeNode,
                    constructorElement,
                    constructor.ToString()
                )
            );

            void InitConstructorNode(ConstructorElementTreeNode treeNode)
            {
                GetConstructorChildren(constructorElement, treeNode, false);
                if (editingForm.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddFunctionNode(ParametersDataTreeNode parentTreeNode, XmlElement functionElement)
        {
            FunctionData functionData = _functionDataParser.Parse(functionElement);
            if (!_getValidConfigurationFromData.TryGetFunction(functionData, editingForm.Application, out Function? function))
                return;

            InitFunctionNode
            (
                _dataGraphTreeViewHelper.AddFunctionTreeNode
                (
                    parentTreeNode,
                    functionElement,
                    function.ToString()
                )
            );

            void InitFunctionNode(FunctionElementTreeNode treeNode)
            {
                GetFunctionChildren(functionElement, treeNode, false);
                if (editingForm.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddLiteralListNode(ParametersDataTreeNode parentTreeNode, XmlElement literalListElement, LiteralListParameterElementInfo literalListElementInfo)
        {
            LiteralListData literalListData = _literalListDataParser.Parse(literalListElement, literalListElementInfo, editingForm);

            InitLiteralListMode
            (
                _dataGraphTreeViewHelper.AddLiteralListTreeNode
                (
                    parentTreeNode,
                    literalListElement,
                    literalListElementInfo,
                    literalListData.VisibleText,
                    literalListData.VisibleText
                )
            );

            void InitLiteralListMode(LiteralListElementTreeNode treeNode)
            {
                GetLiteralListChildren(literalListElement, treeNode, literalListData, false);
                if (editingForm.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddObjectListNode(ParametersDataTreeNode parentTreeNode, XmlElement objectListElement, ObjectListParameterElementInfo objectListElementInfo)
        {
            ObjectListData objectListData = _objectListDataParser.Parse(objectListElement, objectListElementInfo, editingForm);

            InitObjectListMode
            (
                _dataGraphTreeViewHelper.AddObjectListTreeNode
                (
                    parentTreeNode,
                    objectListElement,
                    objectListElementInfo,
                    objectListData.VisibleText,
                    objectListData.VisibleText
                )
            );

            void InitObjectListMode(ObjectListElementTreeNode treeNode)
            {
                GetObjectListChildren(objectListElement, treeNode, objectListData, false);
                if (editingForm.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddVariableNode(ParametersDataTreeNode parentTreeNode, XmlElement variableElement)
        {
            VariableData variableData = _variableDataParser.Parse(variableElement);
            if (!_getValidConfigurationFromData.TryGetVariable(variableData, editingForm.Application, out VariableBase? variable))
                return;

            _dataGraphTreeViewHelper.AddVariableTreeNode(parentTreeNode, variableElement, variable.Name);
        }

        private void GetConstructorChildren(XmlElement constructorElement, ConstructorElementTreeNode constructorElementTreeNode, bool root)
        {
            ConstructorData constructorData = _constructorDataParser.Parse(constructorElement);
            if (!_getValidConfigurationFromData.TryGetConstructor(constructorData, editingForm.Application, out Constructor? constructor))
                return;

            GetParameters
            (
                constructorElementTreeNode,
                root,
                constructor.Parameters.ToDictionary(p => p.Name),
                _xmlDocumentHelpers.GetChildElements
                (
                    _xmlDocumentHelpers.GetSingleChildElement(constructorElement, e => e.Name == XmlDataConstants.PARAMETERSELEMENT)
                )
                .ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            );
        }

        private void GetFunctionChildren(XmlElement functionElement, FunctionElementTreeNode functionElementTreeNode, bool root)
        {
            FunctionData functionData = _functionDataParser.Parse(functionElement);
            if (!_getValidConfigurationFromData.TryGetFunction(functionData, editingForm.Application, out Function? function))
                return;

            GetParameters
            (
                functionElementTreeNode,
                root,
                function.Parameters.ToDictionary(p => p.Name),
                _xmlDocumentHelpers.GetChildElements
                (
                    _xmlDocumentHelpers.GetSingleChildElement(functionElement, e => e.Name == XmlDataConstants.PARAMETERSELEMENT)
                )
                .ToDictionary(e => e.GetAttribute(XmlDataConstants.NAMEATTRIBUTE))
            );
        }

        private void GetLiteralListChildren(XmlElement literalListElement, LiteralListElementTreeNode literalListElementTreeNode, LiteralListData literalListData, bool root)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{486E32CC-829C-45D5-9E9F-7D8A8FE4D9B9}");

            Type literalType = _enumHelper.GetSystemType(literalListData.LiteralType);

            for (int i = 0; i < literalListData.ChildElements.Count; i++)
            {
                var literalElementTreeNode = _dataGraphTreeViewHelper.AddLiteralTreeNode
                (
                    literalListElementTreeNode, 
                    literalListData.ChildElements[i],//<literal>
                    literalType,
                    _xmlDocumentHelpers.GetVisibleText(literalListData.ChildElements[i]),
                    i
                );

                if (root)
                    _treeViewService.MakeVisible(literalElementTreeNode);
            }
        }

        private void GetLiteralListParameterChildren(XmlElement parameterElement, ParametersDataTreeNode parameterTreeNode, LiteralListParameterElementInfo literalListElementInfo)
        {
            GetChildren(_xmlDocumentHelpers.GetSingleChildElement(parameterElement));
            void GetChildren(XmlElement childOfParameterElement)//constructor,function,variable,literalList,objectList
            {
                switch (childOfParameterElement.Name)
                {
                    case XmlDataConstants.CONSTRUCTORELEMENT:
                        AddConstructorNode(parameterTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.FUNCTIONELEMENT:
                        AddFunctionNode(parameterTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.VARIABLEELEMENT:
                        AddVariableNode(parameterTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.LITERALLISTELEMENT:
                        AddLiteralListNode
                        (
                            parameterTreeNode,
                            childOfParameterElement,
                            literalListElementInfo
                        );
                        break;
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        AddObjectListNode
                        (
                            parameterTreeNode,
                            childOfParameterElement,
                            _objectListElementInfoHelper.GetObjectListElementInfo(_objectListDataParser.Parse(childOfParameterElement))
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{3F1D685C-4E50-4FEE-975E-D4C1BCE15C05}");
                }
            }
        }

        private void GetObjectListChildren(XmlElement objectListElement, ObjectListElementTreeNode objectListElementTreeNode, ObjectListData objectListData, bool root)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{43CFC2BC-9DCE-4DEA-97A0-9C0D638355A7}");

            if (!_typeLoadHelper.TryGetSystemType(objectListData.ObjectType, editingForm.Application, out Type? objectType))
                return;

            for (int i = 0; i < objectListData.ChildElements.Count; i++)
            {
                var objectElementTreeNode = _dataGraphTreeViewHelper.AddObjectTreeNode
                (
                    objectListElementTreeNode,
                    objectListData.ChildElements[i],//<object>
                    objectType,
                    GetVisibleTextForObjectType(objectListData.ChildElements[i]),
                    i
                );

                AddObjectChild(objectElementTreeNode, _xmlDocumentHelpers.GetSingleChildElement(objectListData.ChildElements[i]));

                if (root)
                    _treeViewService.MakeVisible(objectElementTreeNode);
            }

            void AddObjectChild(ObjectElementTreeNode objectTreeNode, XmlElement childElement)
            {
                switch (childElement.Name)
                {
                    case XmlDataConstants.CONSTRUCTORELEMENT:
                        AddConstructorNode(objectTreeNode, childElement);
                        break;
                    case XmlDataConstants.FUNCTIONELEMENT:
                        AddFunctionNode(objectTreeNode, childElement);
                        break;
                    case XmlDataConstants.VARIABLEELEMENT:
                        AddVariableNode(objectTreeNode, childElement);
                        break;
                    case XmlDataConstants.LITERALLISTELEMENT:
                        AddLiteralListNode
                        (
                            objectTreeNode, 
                            childElement,
                            _literalListElementInfoHelper.GetLiteralListElementInfo(_literalListDataParser.Parse(childElement))
                        );
                        break;
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        AddObjectListNode
                        (
                            objectTreeNode, 
                            childElement,
                            //child of <object> is an objectList - can't use objectListElementTreeNode.ListInfo here (belongs to the parent of <object>)
                            _objectListElementInfoHelper.GetObjectListElementInfo(_objectListDataParser.Parse(childElement))
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{B659D66A-2DE0-416D-9EDD-85081F23531C}");
                }
            }

            string GetVisibleTextForObjectType(XmlElement parameterElement)
                => _xmlDocumentHelpers.GetSingleChildElement(parameterElement).GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE);
        }

        private void GetObjectListParameterChildren(XmlElement parameterElement, ParametersDataTreeNode parameterTreeNode, ObjectListParameterElementInfo objectListElementInfo)
        {
            GetChildren(_xmlDocumentHelpers.GetSingleChildElement(parameterElement));
            void GetChildren(XmlElement childOfParameterElement)//constructor,function,variable,literalList,objectList
            {
                switch (childOfParameterElement.Name)
                {
                    case XmlDataConstants.CONSTRUCTORELEMENT:
                        AddConstructorNode(parameterTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.FUNCTIONELEMENT:
                        AddFunctionNode(parameterTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.VARIABLEELEMENT:
                        AddVariableNode(parameterTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.LITERALLISTELEMENT:
                        AddLiteralListNode
                        (
                            parameterTreeNode,
                            childOfParameterElement,
                            _literalListElementInfoHelper.GetLiteralListElementInfo(_literalListDataParser.Parse(childOfParameterElement))
                        );
                        break;
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        AddObjectListNode
                        (
                            parameterTreeNode,
                            childOfParameterElement,
                            objectListElementInfo
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{01B4C6B0-6D3C-492C-97E7-17A6A683C3B8}");
                }
            }
        }

        private void GetObjectParameterChildren(XmlElement parameterElement, ParametersDataTreeNode parameterTreeNode)
        {
            GetChildren(_xmlDocumentHelpers.GetSingleChildElement(parameterElement));
            void GetChildren(XmlElement childOfParameterElement)//constructor,function,variable,literalList,objectList
            {
                switch (childOfParameterElement.Name)
                {
                    case XmlDataConstants.CONSTRUCTORELEMENT:
                        AddConstructorNode(parameterTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.FUNCTIONELEMENT:
                        AddFunctionNode(parameterTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.VARIABLEELEMENT:
                        AddVariableNode(parameterTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.LITERALLISTELEMENT:
                        AddLiteralListNode
                        (
                            parameterTreeNode, 
                            childOfParameterElement, 
                            _literalListElementInfoHelper.GetLiteralListElementInfo(_literalListDataParser.Parse(childOfParameterElement))
                        );
                        break;
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        AddObjectListNode
                        (
                            parameterTreeNode, 
                            childOfParameterElement, 
                            _objectListElementInfoHelper.GetObjectListElementInfo(_objectListDataParser.Parse(childOfParameterElement))
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{5785F852-19B2-45AA-A78C-1DBCA779F73D}");
                }
            }
        }

        private void GetParameters(ParametersDataTreeNode parentNode, bool root, Dictionary<string, ParameterBase> parameters, Dictionary<string, XmlElement> parameterElements)
        {
            foreach(var pair in parameterElements) 
            {
                XmlElement parameterElement = pair.Value;
                if (!parameters.TryGetValue(parameterElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), out ParameterBase? parameter))
                    return;

                if (!_typeLoadHelper.TryGetSystemType(parameter, editingForm.Application, out Type? parameterType))
                    return;

                GetParameterChildren
                (
                    AddParameterNode(parameter, parameterElement, parameterType), 
                    parameter, 
                    parameterElement
                );
            }

            ParametersDataTreeNode AddParameterNode(ParameterBase parameter, 
                XmlElement parameterElement, //literalParameter, objectParameter, literalListParameter, objectListParameter
                Type parameterType)
            {
                return parameter switch
                {
                    LiteralParameter literalParameter => _dataGraphTreeViewHelper.AddLiteralParameterTreeNode(parentNode, parameterElement, parameterType, _xmlDocumentHelpers.GetVisibleText(parameterElement)),
                    ObjectParameter objectParameter => _dataGraphTreeViewHelper.AddObjectParameterTreeNode(parentNode, parameterElement, parameterType, GetVisibleTextForObjectType(parameterElement)),
                    ListOfLiteralsParameter listOfLiteralsParameter => _dataGraphTreeViewHelper.AddLiteralListParameterTreeNode(parentNode, parameterElement, parameterType, GetVisibleTextForObjectType(parameterElement)),
                    ListOfObjectsParameter listOfObjectsParameter => _dataGraphTreeViewHelper.AddObjectListParameterTreeNode(parentNode, parameterElement, parameterType, GetVisibleTextForObjectType(parameterElement)),
                    _ => throw _exceptionHelper.CriticalException("{C3BC6CDB-E4CB-4AF2-A069-F8AB21DB5F2D}"),
                };
            }

            string GetVisibleTextForObjectType(XmlElement parameterElement) 
                => _xmlDocumentHelpers.GetSingleChildElement(parameterElement).GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE);

            void GetParameterChildren(ParametersDataTreeNode parameterTreeNode, ParameterBase parameter, XmlElement parameterElement)
            {
                if (root)
                    _treeViewService.MakeVisible(parameterTreeNode);

                switch (parameter)
                {
                    case ObjectParameter:
                        GetObjectParameterChildren(parameterElement, parameterTreeNode);
                        break;
                    case ListOfObjectsParameter listOfObjectsParameter:
                        GetObjectListParameterChildren(parameterElement, parameterTreeNode, _objectListElementInfoHelper.GetObjectListElementInfo(listOfObjectsParameter));
                        break;
                    case ListOfLiteralsParameter listOfLiteralsParameter:
                        parameterElements.TryGetValue(listOfLiteralsParameter.PropertySourceParameter, out XmlElement? parameterSourceElement);
                        GetLiteralListParameterChildren
                        (
                            parameterElement, 
                            parameterTreeNode, 
                            _literalListElementInfoHelper.GetLiteralListElementInfo
                            (
                                listOfLiteralsParameter, 
                                parameterSourceElement?.InnerText ?? string.Empty// The corresponding LiteralListControl (unlike LiteralListParameterRichTextBoxControl) will not 
                            )                                       //have access to the other parameters (editControlsSet) therefore the parameterSourceClassName must
                        );                                          //included here if applicable.
                        break;
                    default:
                        break;
                }

                if (editingForm.ExpandedNodes.ContainsKey(parameterTreeNode.Name))
                    parameterTreeNode.Expand();
            }
        }
        #endregion Private Methods
    }
}
