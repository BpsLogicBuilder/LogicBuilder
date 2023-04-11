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
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IDataGraphTreeViewHelper _dataGraphTreeViewHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IGetValidConfigurationFromData _getValidConfigurationFromData;
        private readonly IImageListService _imageListService;
        private readonly ILiteralListDataParser _literalListDataParser;
        private readonly ILiteralListParameterElementInfoHelper _literalListParameterElementInfoHelper;
        private readonly ILiteralListVariableElementInfoHelper _literalListVariableElementInfoHelper;
        private readonly IObjectListDataParser _objectListDataParser;
        private readonly IObjectListParameterElementInfoHelper _objectListParameterElementInfoHelper;
        private readonly IObjectListVariableElementInfoHelper _objectListVariableElementInfoHelper;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingHost dataGraphEditingHost;

        public ParametersDataTreeBuilder(
            IAssertFunctionDataParser assertFunctionDataParser,
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IDataGraphTreeViewHelper dataGraphTreeViewHelper,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IGetValidConfigurationFromData getValidConfigurationFromData,
            IImageListService imageListService,
            ILiteralListDataParser literalListDataParser,
            ILiteralListParameterElementInfoHelper literalListParameterElementInfoHelper,
            ILiteralListVariableElementInfoHelper literalListariableElementInfoHelper,
            IObjectListDataParser objectListDataParser,
            IObjectListParameterElementInfoHelper objectListParameterElementInfoHelper,
            IObjectListVariableElementInfoHelper objectListVariableElementInfoHelper,
            IRetractFunctionDataParser retractFunctionDataParser,
            ITreeViewService treeViewService,
            ITypeLoadHelper typeLoadHelper,
            IVariableDataParser variableDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost)
        {
            _assertFunctionDataParser = assertFunctionDataParser;
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _dataGraphTreeViewHelper = dataGraphTreeViewHelper;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _getValidConfigurationFromData = getValidConfigurationFromData;
            _imageListService = imageListService;
            _literalListDataParser = literalListDataParser;
            _literalListParameterElementInfoHelper = literalListParameterElementInfoHelper;
            _literalListVariableElementInfoHelper = literalListariableElementInfoHelper;
            _objectListDataParser = objectListDataParser;
            _objectListParameterElementInfoHelper = objectListParameterElementInfoHelper;
            _objectListVariableElementInfoHelper = objectListVariableElementInfoHelper;
            _retractFunctionDataParser = retractFunctionDataParser;
            _treeViewService = treeViewService;
            _typeLoadHelper = typeLoadHelper;
            _variableDataParser = variableDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
        }

        private const string ASSERT_FUNCTION_ROOT_XPATH = $"/{XmlDataConstants.ASSERTFUNCTIONELEMENT}";
        private const string CONSTRUCTOR_ROOT_XPATH = $"/{XmlDataConstants.CONSTRUCTORELEMENT}";
        private const string FUNCTION_ROOT_XPATH = $"/{XmlDataConstants.FUNCTIONELEMENT}";
        private const string LITERALLIST_ROOT_XPATH = $"/{XmlDataConstants.LITERALLISTELEMENT}";
        private const string OBJECTLIST_ROOT_XPATH = $"/{XmlDataConstants.OBJECTLISTELEMENT}";
        private const string RETRACT_FUNCTION_ROOT_XPATH = $"/{XmlDataConstants.RETRACTFUNCTIONELEMENT}";

        private static readonly HashSet<string> RootXPaths = new()
        {
            ASSERT_FUNCTION_ROOT_XPATH,
            CONSTRUCTOR_ROOT_XPATH,
            FUNCTION_ROOT_XPATH,
            LITERALLIST_ROOT_XPATH,
            OBJECTLIST_ROOT_XPATH,
            RETRACT_FUNCTION_ROOT_XPATH
        };

        public void CreateAssertFunctionTreeProfile(RadTreeView treeView, XmlDocument xmlDocument)
        {
            treeView.BeginUpdate();
            treeView.ImageList = _imageListService.ImageList;
            treeView.TreeViewElement.ShowNodeToolTips = true;
            treeView.ShowRootLines = true;
            treeView.Nodes.Clear();
            XmlElement functionElement = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, ASSERT_FUNCTION_ROOT_XPATH);
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), out Function? function))
                return;

            GetAssertFunctionChildren
            (
                functionElement,
                _dataGraphTreeViewHelper.AddRootAssertFunctionTreeNode(treeView, functionElement, function.Name)
            );

            treeView.EndUpdate();
        }

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
            LiteralListData literalListData = _literalListDataParser.Parse(literalListElement, literalListInfo, dataGraphEditingHost);
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
            ObjectListData objectListData = _objectListDataParser.Parse(objectListElement, objectListInfo, dataGraphEditingHost);
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

        public void CreateRetractFunctionTreeProfile(RadTreeView treeView, XmlDocument xmlDocument)
        {
            throw new NotImplementedException();
        }

        public void RefreshTreeNode(RadTreeView treeView, XmlDocument xmlDocument, ParametersDataTreeNode node)
        {
            bool root = RootXPaths.Contains(node.Name);
            treeView.BeginUpdate();
            node.Nodes.Clear();
            XmlElement xmlElement = _xmlDocumentHelpers.SelectSingleElement(xmlDocument, node.Name);

            switch (node)
            {
                case AssertFunctionElementTreeNode assertFunctionElementTreeNode:
                    if (!_configurationService.FunctionList.Functions.TryGetValue(xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), out Function? assertFunction))
                        return;

                    node.ToolTipText = assertFunction.Name;

                    GetAssertFunctionChildren
                    (
                        xmlElement,
                        assertFunctionElementTreeNode
                    );
                    break;
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
                case ParameterLiteralListElementTreeNode parameterLiteralListElementTreeNode:
                    LiteralListData literalListData = _literalListDataParser.Parse(xmlElement, parameterLiteralListElementTreeNode.ListInfo, dataGraphEditingHost);
                    if (!parameterLiteralListElementTreeNode.ListInfo.HasParameter)//if there's a parameter the ListInfo comes from the configured parameter.  Do not update.
                    {//Otherwise it is derived from the possibly updated LiteralListData
                        parameterLiteralListElementTreeNode.ListInfo = _literalListParameterElementInfoHelper.GetLiteralListElementInfo(literalListData);
                    }

                    parameterLiteralListElementTreeNode.Text = literalListData.VisibleText;

                    GetLiteralListChildren
                    (
                        xmlElement,
                        parameterLiteralListElementTreeNode,
                        literalListData,
                        root
                    );
                    break;
                case ParameterObjectListElementTreeNode objectListElementTreeNode:
                    ObjectListData objectListData = _objectListDataParser.Parse(xmlElement, objectListElementTreeNode.ListInfo, dataGraphEditingHost);
                    if (!objectListElementTreeNode.ListInfo.HasParameter)//if there's a parameter the ListInfo comes from the configured parameter.  Do not update.
                    {//Otherwise it is derived from the possibly updated ObjectListData
                        objectListElementTreeNode.ListInfo = _objectListParameterElementInfoHelper.GetObjectListElementInfo(objectListData);
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
                case RetractFunctionElementTreeNode:
                    break;
                case VariableLiteralListElementTreeNode variableLiteralListElementTreeNode:
                    LiteralListData variableLiteralListData = _literalListDataParser.Parse(xmlElement, variableLiteralListElementTreeNode.ListInfo, dataGraphEditingHost);
                    if (!variableLiteralListElementTreeNode.ListInfo.HasVariable)//if there's a variable the ListInfo comes from the configured variable.  Do not update.
                    {//Otherwise it is derived from the possibly updated LiteralListData
                        variableLiteralListElementTreeNode.ListInfo = _literalListVariableElementInfoHelper.GetLiteralListElementInfo(variableLiteralListData);
                    }

                    variableLiteralListElementTreeNode.Text = variableLiteralListData.VisibleText;

                    GetLiteralListChildren
                    (
                        xmlElement,
                        variableLiteralListElementTreeNode,
                        variableLiteralListData,
                        root
                    );
                    break;
                case VariableObjectListElementTreeNode objectVariableListElementTreeNode:
                    ObjectListData variableObjectListData = _objectListDataParser.Parse(xmlElement, objectVariableListElementTreeNode.ListInfo, dataGraphEditingHost);
                    if (!objectVariableListElementTreeNode.ListInfo.HasVariable)//if there's a variable the ListInfo comes from the configured variable.  Do not update.
                    {//Otherwise it is derived from the possibly updated ObjectListData
                        objectVariableListElementTreeNode.ListInfo = _objectListVariableElementInfoHelper.GetObjectListElementInfo(variableObjectListData);
                    }

                    objectVariableListElementTreeNode.Text = variableObjectListData.VisibleText;

                    GetObjectListChildren
                    (
                        xmlElement,
                        objectVariableListElementTreeNode,
                        variableObjectListData,
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
            if (!_getValidConfigurationFromData.TryGetConstructor(constructorData, dataGraphEditingHost.Application, out Constructor? constructor))
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
                if (dataGraphEditingHost.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddFunctionNode(ParametersDataTreeNode parentTreeNode, XmlElement functionElement)
        {
            FunctionData functionData = _functionDataParser.Parse(functionElement);
            if (!_getValidConfigurationFromData.TryGetFunction(functionData, dataGraphEditingHost.Application, out Function? function))
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
                if (dataGraphEditingHost.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddLiteralListNode(ParametersDataTreeNode parentTreeNode, XmlElement literalListElement, LiteralListParameterElementInfo literalListElementInfo)
        {
            LiteralListData literalListData = _literalListDataParser.Parse(literalListElement, literalListElementInfo, dataGraphEditingHost);

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

            void InitLiteralListMode(ParameterLiteralListElementTreeNode treeNode)
            {
                GetLiteralListChildren(literalListElement, treeNode, literalListData, false);
                if (dataGraphEditingHost.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddLiteralListNode(ParametersDataTreeNode parentTreeNode, XmlElement literalListElement, LiteralListVariableElementInfo literalListElementInfo)
        {
            LiteralListData literalListData = _literalListDataParser.Parse(literalListElement, literalListElementInfo, dataGraphEditingHost);

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

            void InitLiteralListMode(VariableLiteralListElementTreeNode treeNode)
            {
                GetLiteralListChildren(literalListElement, treeNode, literalListData, false);
                if (dataGraphEditingHost.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddObjectListNode(ParametersDataTreeNode parentTreeNode, XmlElement objectListElement, ObjectListParameterElementInfo objectListElementInfo)
        {
            ObjectListData objectListData = _objectListDataParser.Parse(objectListElement, objectListElementInfo, dataGraphEditingHost);

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

            void InitObjectListMode(ParameterObjectListElementTreeNode treeNode)
            {
                GetObjectListChildren(objectListElement, treeNode, objectListData, false);
                if (dataGraphEditingHost.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddObjectListNode(ParametersDataTreeNode parentTreeNode, XmlElement objectListElement, ObjectListVariableElementInfo objectListElementInfo)
        {
            ObjectListData objectListData = _objectListDataParser.Parse(objectListElement, objectListElementInfo, dataGraphEditingHost);

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

            void InitObjectListMode(VariableObjectListElementTreeNode treeNode)
            {
                GetObjectListChildren(objectListElement, treeNode, objectListData, false);
                if (dataGraphEditingHost.ExpandedNodes.ContainsKey(treeNode.Name))
                    treeNode.Expand();
            }
        }

        private void AddVariableNode(ParametersDataTreeNode parentTreeNode, XmlElement variableElement)
        {
            VariableData variableData = _variableDataParser.Parse(variableElement);
            if (!_getValidConfigurationFromData.TryGetVariable(variableData, dataGraphEditingHost.Application, out VariableBase? variable))
                return;

            _dataGraphTreeViewHelper.AddVariableTreeNode(parentTreeNode, variableElement, variable.Name);
        }

        private void GetAssertFunctionChildren(XmlElement functionElement, AssertFunctionElementTreeNode assertFunctionElementTreeNode)
        {
            AssertFunctionData functionData = _assertFunctionDataParser.Parse(functionElement);
            if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out Function? _))
                return;

            if (!_getValidConfigurationFromData.TryGetVariable(_variableDataParser.Parse(functionData.VariableElement), dataGraphEditingHost.Application, out VariableBase? variable))
                return;

            if (!_typeLoadHelper.TryGetSystemType(variable, dataGraphEditingHost.Application, out Type? variableType))
                return;

            //literalVariable, objectVariable, literalListVariable, objectListVariable
            XmlElement variableElement = _xmlDocumentHelpers.GetSingleChildElement(functionData.VariableValueElement);

            GetValueChild(AddVariableNode());

            ParametersDataTreeNode AddVariableNode()
            {
                return variable switch
                {
                    LiteralVariable => _dataGraphTreeViewHelper.AddLiteralVariableTreeNode(assertFunctionElementTreeNode, variableElement, variable.Name, variableType, _xmlDocumentHelpers.GetVisibleText(variableElement)),
                    ObjectVariable => _dataGraphTreeViewHelper.AddObjectVariableTreeNode(assertFunctionElementTreeNode, variableElement, variable.Name, variableType, GetVisibleTextForObjectType(variableElement)),
                    ListOfLiteralsVariable => _dataGraphTreeViewHelper.AddListOfLiteralsVariableTreeNode(assertFunctionElementTreeNode, variableElement, variable.Name, variableType, GetVisibleTextForObjectType(variableElement)),
                    ListOfObjectsVariable => _dataGraphTreeViewHelper.AddListOfObjectsVariableTreeNode(assertFunctionElementTreeNode, variableElement, variable.Name, variableType, GetVisibleTextForObjectType(variableElement)),
                    _ => throw _exceptionHelper.CriticalException("{0806B331-D0A8-4428-851A-F8144C882310}"),
                };
            }

            void GetValueChild(ParametersDataTreeNode variableTreeNode)
            {
                _treeViewService.MakeVisible(variableTreeNode); //AssertFunction is always at the root

                switch (variable)
                {
                    case ObjectVariable:
                        GetObjectParameterChildren(variableElement, variableTreeNode);
                        break;
                    case ListOfLiteralsVariable listOfLiteralsVariable:
                        GetLiteralListVariableChildren(variableElement, variableTreeNode, _literalListVariableElementInfoHelper.GetLiteralListElementInfo(listOfLiteralsVariable));
                        break;
                    case ListOfObjectsVariable listOfObjectsVariable:
                        GetObjectListVariableChildren(variableElement, variableTreeNode, _objectListVariableElementInfoHelper.GetObjectListElementInfo(listOfObjectsVariable));
                        break;
                    default:
                        break;
                }

                if (dataGraphEditingHost.ExpandedNodes.ContainsKey(variableTreeNode.Name))
                    variableTreeNode.Expand();
            }

            string GetVisibleTextForObjectType(XmlElement parameterElement)
                => _xmlDocumentHelpers.GetSingleChildElement(parameterElement).GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE);
        }

        private void GetConstructorChildren(XmlElement constructorElement, ConstructorElementTreeNode constructorElementTreeNode, bool root)
        {
            ConstructorData constructorData = _constructorDataParser.Parse(constructorElement);
            if (!_getValidConfigurationFromData.TryGetConstructor(constructorData, dataGraphEditingHost.Application, out Constructor? constructor))
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
            if (!_getValidConfigurationFromData.TryGetFunction(functionData, dataGraphEditingHost.Application, out Function? function))
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

        private void GetLiteralListChildren(XmlElement literalListElement, ParameterLiteralListElementTreeNode literalListElementTreeNode, LiteralListData literalListData, bool root)
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

        private void GetLiteralListChildren(XmlElement literalListElement, VariableLiteralListElementTreeNode literalListElementTreeNode, LiteralListData literalListData, bool root)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{0DEEE33A-4600-441D-B664-687D5DEBC2D6}");

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
                            _objectListParameterElementInfoHelper.GetObjectListElementInfo(_objectListDataParser.Parse(childOfParameterElement))
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{3F1D685C-4E50-4FEE-975E-D4C1BCE15C05}");
                }
            }
        }

        private void GetLiteralListVariableChildren(XmlElement variableElement, ParametersDataTreeNode variableTreeNode, LiteralListVariableElementInfo literalListElementInfo)
        {
            GetChildren(_xmlDocumentHelpers.GetSingleChildElement(variableElement));
            void GetChildren(XmlElement childOfParameterElement)//constructor,function,variable,literalList,objectList
            {
                switch (childOfParameterElement.Name)
                {
                    case XmlDataConstants.CONSTRUCTORELEMENT:
                        AddConstructorNode(variableTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.FUNCTIONELEMENT:
                        AddFunctionNode(variableTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.VARIABLEELEMENT:
                        AddVariableNode(variableTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.LITERALLISTELEMENT:
                        AddLiteralListNode
                        (
                            variableTreeNode,
                            childOfParameterElement,
                            literalListElementInfo
                        );
                        break;
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        AddObjectListNode
                        (
                            variableTreeNode,
                            childOfParameterElement,
                            _objectListParameterElementInfoHelper.GetObjectListElementInfo(_objectListDataParser.Parse(childOfParameterElement))
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{3F1D685C-4E50-4FEE-975E-D4C1BCE15C05}");
                }
            }
        }

        private void GetObjectListChildren(XmlElement objectListElement, ParameterObjectListElementTreeNode objectListElementTreeNode, ObjectListData objectListData, bool root)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{43CFC2BC-9DCE-4DEA-97A0-9C0D638355A7}");

            if (!_typeLoadHelper.TryGetSystemType(objectListData.ObjectType, dataGraphEditingHost.Application, out Type? objectType))
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
                            _literalListParameterElementInfoHelper.GetLiteralListElementInfo(_literalListDataParser.Parse(childElement))
                        );
                        break;
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        AddObjectListNode
                        (
                            objectTreeNode, 
                            childElement,
                            //child of <object> is an objectList - can't use objectListElementTreeNode.ListInfo here (belongs to the parent of <object>)
                            _objectListParameterElementInfoHelper.GetObjectListElementInfo(_objectListDataParser.Parse(childElement))
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{B659D66A-2DE0-416D-9EDD-85081F23531C}");
                }
            }

            string GetVisibleTextForObjectType(XmlElement parameterElement)
                => _xmlDocumentHelpers.GetSingleChildElement(parameterElement).GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE);
        }

        private void GetObjectListChildren(XmlElement objectListElement, VariableObjectListElementTreeNode objectListElementTreeNode, ObjectListData objectListData, bool root)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{E2B8D895-8035-4263-8DE8-BE0A9BA95E82}");

            if (!_typeLoadHelper.TryGetSystemType(objectListData.ObjectType, dataGraphEditingHost.Application, out Type? objectType))
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
                            _literalListParameterElementInfoHelper.GetLiteralListElementInfo(_literalListDataParser.Parse(childElement))
                        );
                        break;
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        AddObjectListNode
                        (
                            objectTreeNode,
                            childElement,
                            //child of <object> is an objectList - can't use objectListElementTreeNode.ListInfo here (belongs to the parent of <object>)
                            _objectListParameterElementInfoHelper.GetObjectListElementInfo(_objectListDataParser.Parse(childElement))
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
                            _literalListParameterElementInfoHelper.GetLiteralListElementInfo(_literalListDataParser.Parse(childOfParameterElement))
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

        private void GetObjectListVariableChildren(XmlElement variableElement, ParametersDataTreeNode variableTreeNode, ObjectListVariableElementInfo objectListVariableElementInfo)
        {
            GetChildren(_xmlDocumentHelpers.GetSingleChildElement(variableElement));
            void GetChildren(XmlElement childOfParameterElement)//constructor,function,variable,literalList,objectList
            {
                switch (childOfParameterElement.Name)
                {
                    case XmlDataConstants.CONSTRUCTORELEMENT:
                        AddConstructorNode(variableTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.FUNCTIONELEMENT:
                        AddFunctionNode(variableTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.VARIABLEELEMENT:
                        AddVariableNode(variableTreeNode, childOfParameterElement);
                        break;
                    case XmlDataConstants.LITERALLISTELEMENT:
                        AddLiteralListNode
                        (
                            variableTreeNode,
                            childOfParameterElement,
                            _literalListParameterElementInfoHelper.GetLiteralListElementInfo(_literalListDataParser.Parse(childOfParameterElement))
                        );
                        break;
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        AddObjectListNode
                        (
                            variableTreeNode,
                            childOfParameterElement,
                            objectListVariableElementInfo
                        );
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{E7FEA949-4909-4045-A23C-320CCB4984E2}");
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
                            _literalListParameterElementInfoHelper.GetLiteralListElementInfo(_literalListDataParser.Parse(childOfParameterElement))
                        );
                        break;
                    case XmlDataConstants.OBJECTLISTELEMENT:
                        AddObjectListNode
                        (
                            parameterTreeNode, 
                            childOfParameterElement, 
                            _objectListParameterElementInfoHelper.GetObjectListElementInfo(_objectListDataParser.Parse(childOfParameterElement))
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

                if (!_typeLoadHelper.TryGetSystemType(parameter, dataGraphEditingHost.Application, out Type? parameterType))
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
                    ListOfLiteralsParameter listOfLiteralsParameter => _dataGraphTreeViewHelper.AddListOfLiteralsParameterTreeNode(parentNode, parameterElement, parameterType, GetVisibleTextForObjectType(parameterElement)),
                    ListOfObjectsParameter listOfObjectsParameter => _dataGraphTreeViewHelper.AddListOfObjectsParameterTreeNode(parentNode, parameterElement, parameterType, GetVisibleTextForObjectType(parameterElement)),
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
                        GetObjectListParameterChildren(parameterElement, parameterTreeNode, _objectListParameterElementInfoHelper.GetObjectListElementInfo(listOfObjectsParameter));
                        break;
                    case ListOfLiteralsParameter listOfLiteralsParameter:
                        parameterElements.TryGetValue(listOfLiteralsParameter.PropertySourceParameter, out XmlElement? parameterSourceElement);
                        GetLiteralListParameterChildren
                        (
                            parameterElement, 
                            parameterTreeNode, 
                            _literalListParameterElementInfoHelper.GetLiteralListElementInfo
                            (
                                listOfLiteralsParameter, 
                                parameterSourceElement?.InnerText ?? string.Empty// The corresponding LiteralListControl (unlike LiteralListParameterRichTextBoxControl) will not 
                            )                                       //have access to the other parameters (editControlsSet) therefore the parameterSourceClassName must
                        );                                          //included here if applicable.
                        break;
                    default:
                        break;
                }

                if (dataGraphEditingHost.ExpandedNodes.ContainsKey(parameterTreeNode.Name))
                    parameterTreeNode.Expand();
            }
        }
        #endregion Private Methods
    }
}
