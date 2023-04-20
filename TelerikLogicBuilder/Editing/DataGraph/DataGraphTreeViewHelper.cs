using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System;
using System.Globalization;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph
{
    internal class DataGraphTreeViewHelper : IDataGraphTreeViewHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;

        public DataGraphTreeViewHelper(
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser)
        {
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
        }

        public ConstructorElementTreeNode AddConstructorTreeNode(ParametersDataTreeNode parentNode, XmlElement constructorElement, string toolTipText)
        {
            if (constructorElement.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                throw _exceptionHelper.CriticalException("{C537E4D3-BB24-4F86-906C-243C54978C23}");

            string constructorName = constructorElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value;
            ConstructorElementTreeNode newTreeNode = new
            (
                constructorName,
                $"{parentNode.Name}/{XmlDataConstants.CONSTRUCTORELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{constructorName}\"]",
                parentNode.AssignedToType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public FunctionElementTreeNode AddFunctionTreeNode(ParametersDataTreeNode parentNode, XmlElement functionElement, string toolTipText)
        {
            if (functionElement.Name != XmlDataConstants.FUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{5868845F-DF85-4284-AFF0-CEB64BE1A558}");

            string functionName = functionElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value;
            FunctionElementTreeNode newTreeNode = new
            (
                functionName,
                $"{parentNode.Name}/{XmlDataConstants.FUNCTIONELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{functionName}\"]",
                parentNode.AssignedToType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ListOfLiteralsParameterElementTreeNode AddListOfLiteralsParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListParameterElement, Type parameterType, string toolTipText)
        {
            if (literalListParameterElement.Name != XmlDataConstants.LITERALLISTPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{7F702BA7-195F-48CE-901E-7BA71516C722}");

            string parameterName = literalListParameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value;
            ListOfLiteralsParameterElementTreeNode newTreeNode = new
            (
                parameterName,
                $"{parentNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{literalListParameterElement.Name}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{parameterName}\"]",
                parameterType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ListOfLiteralsVariableElementTreeNode AddListOfLiteralsVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListVariableElement, string variableName, Type variableType, string toolTipText)
        {
            if (literalListVariableElement.Name != XmlDataConstants.LITERALLISTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{8B176FC5-F8E7-47AA-BDBB-11CC7E851A0D}");

            ListOfLiteralsVariableElementTreeNode newTreeNode = new
            (
                variableName,
                $"{parentNode.Name}/{XmlDataConstants.VARIABLEVALUEELEMENT}/{literalListVariableElement.Name}",
                variableType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ListOfObjectsParameterElementTreeNode AddListOfObjectsParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListParameterElement, Type parameterType, string toolTipText)
        {
            if (objectListParameterElement.Name != XmlDataConstants.OBJECTLISTPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{7A12A49D-7646-4C99-B2D7-4A0317A81454}");

            string parameterName = objectListParameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value;
            ListOfObjectsParameterElementTreeNode newTreeNode = new
            (
                parameterName,
                $"{parentNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{objectListParameterElement.Name}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{parameterName}\"]",
                parameterType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ListOfObjectsVariableElementTreeNode AddListOfObjectsVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListVariableElement, string variableName, Type variableType, string toolTipText)
        {
            if (objectListVariableElement.Name != XmlDataConstants.OBJECTLISTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{1710089A-6600-4465-B362-7BFBE5125DA4}");

            ListOfObjectsVariableElementTreeNode newTreeNode = new
            (
                variableName,
                $"{parentNode.Name}/{XmlDataConstants.VARIABLEVALUEELEMENT}/{objectListVariableElement.Name}",
                variableType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ParameterLiteralListElementTreeNode AddLiteralListTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListElement, LiteralListParameterElementInfo listElementInfo, string text, string toolTipText)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{7395E053-8D51-45A5-AE4C-68F460BBBA52}");

            ParameterLiteralListElementTreeNode newTreeNode = new
            (
                text,
                $"{parentNode.Name}/{XmlDataConstants.LITERALLISTELEMENT}",//always a single child and its child elements are unique by nodes index
                parentNode.AssignedToType,
                listElementInfo
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public VariableLiteralListElementTreeNode AddLiteralListTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListElement, LiteralListVariableElementInfo listElementInfo, string text, string toolTipText)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{224C9B3E-DA31-4867-AA7C-A9BF311303B4}");

            VariableLiteralListElementTreeNode newTreeNode = new
            (
                text,
                $"{parentNode.Name}/{XmlDataConstants.LITERALLISTELEMENT}",//always a single child and its child elements are unique by nodes index
                parentNode.AssignedToType,
                listElementInfo
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public LiteralParameterElementTreeNode AddLiteralParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement literalParameterElement, Type parameterType, string toolTipText)
        {
            if (literalParameterElement.Name != XmlDataConstants.LITERALPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{5F5091BF-B786-4625-8D84-57463559A46F}");

            string parameterName = literalParameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value;
            LiteralParameterElementTreeNode newTreeNode = new
            (
                parameterName,
                $"{parentNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{literalParameterElement.Name}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{parameterName}\"]",
                parameterType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public LiteralElementTreeNode AddLiteralTreeNode(ParameterLiteralListElementTreeNode parentNode, XmlElement literalElement, Type literalType, string toolTipText, int nodeIndex)
        {
            if (literalElement.Name != XmlDataConstants.LITERALELEMENT)
                throw _exceptionHelper.CriticalException("{9C9FA2A8-DCB9-4DC0-8406-D00F4C4C74BF}");

            LiteralElementTreeNode newTreeNode = new
            (
                string.Format(CultureInfo.CurrentCulture, Strings.literalListChildNodeTextFormat, parentNode.ListInfo.Name, nodeIndex),
                $"{parentNode.Name}/{literalElement.Name}[{nodeIndex + 1}]",
                literalType,
                nodeIndex
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public LiteralElementTreeNode AddLiteralTreeNode(VariableLiteralListElementTreeNode parentNode, XmlElement literalElement, Type literalType, string toolTipText, int nodeIndex)
        {
            if (literalElement.Name != XmlDataConstants.LITERALELEMENT)
                throw _exceptionHelper.CriticalException("{C0CA25DD-5FE1-4A3B-9E6A-76D4A80E77B1}");

            LiteralElementTreeNode newTreeNode = new
            (
                string.Format(CultureInfo.CurrentCulture, Strings.literalListChildNodeTextFormat, parentNode.ListInfo.Name, nodeIndex),
                $"{parentNode.Name}/{literalElement.Name}[{nodeIndex + 1}]",
                literalType,
                nodeIndex
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public LiteralVariableElementTreeNode AddLiteralVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement literalVariableElement, string variableName, Type variableType, string toolTipText)
        {
            if (literalVariableElement.Name != XmlDataConstants.LITERALVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{1544809B-3EF7-4834-AAE6-9C0A31488189}");

            LiteralVariableElementTreeNode newTreeNode = new
            (
                variableName,
                $"{parentNode.Name}/{XmlDataConstants.VARIABLEVALUEELEMENT}/{literalVariableElement.Name}",
                variableType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ParameterObjectListElementTreeNode AddObjectListTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListElement, ObjectListParameterElementInfo listElementInfo, string text, string toolTipText)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{15132D22-190F-4FE8-8B84-4D5D49A8FD8C}");

            ParameterObjectListElementTreeNode newTreeNode = new
            (
                text,
                $"{parentNode.Name}/{XmlDataConstants.OBJECTLISTELEMENT}",//always a single child and its child elements are unique by nodes index
                parentNode.AssignedToType,
                listElementInfo
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public VariableObjectListElementTreeNode AddObjectListTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListElement, ObjectListVariableElementInfo listElementInfo, string text, string toolTipText)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{FB44A70C-FBED-4F91-974B-F24AB0EED92A}");

            VariableObjectListElementTreeNode newTreeNode = new
            (
                text,
                $"{parentNode.Name}/{XmlDataConstants.OBJECTLISTELEMENT}",//always a single child and its child elements are unique by nodes index
                parentNode.AssignedToType,
                listElementInfo
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ObjectParameterElementTreeNode AddObjectParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement objectParameterElement, Type parameterType, string toolTipText)
        {
            if (objectParameterElement.Name != XmlDataConstants.OBJECTPARAMETERELEMENT)
                throw _exceptionHelper.CriticalException("{D254B1A7-4724-4CDA-822C-BC13CC433EFD}");

            string parameterName = objectParameterElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value;
            ObjectParameterElementTreeNode newTreeNode = new
            (
                parameterName,
                $"{parentNode.Name}/{XmlDataConstants.PARAMETERSELEMENT}/{objectParameterElement.Name}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{parameterName}\"]",
                parameterType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ObjectElementTreeNode AddObjectTreeNode(ParameterObjectListElementTreeNode parentNode, XmlElement objectElement, Type objectType, string toolTipText, int nodeIndex)
        {
            if (objectElement.Name != XmlDataConstants.OBJECTELEMENT)
                throw _exceptionHelper.CriticalException("{FB03B89A-CCEF-479D-8610-5365CF528B17}");

            ObjectElementTreeNode newTreeNode = new
            (
                string.Format(CultureInfo.CurrentCulture, Strings.objectListChildNodeTextFormat, parentNode.ListInfo.Name, nodeIndex),
                $"{parentNode.Name}/{objectElement.Name}[{nodeIndex + 1}]",
                objectType,
                nodeIndex
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ObjectElementTreeNode AddObjectTreeNode(VariableObjectListElementTreeNode parentNode, XmlElement objectElement, Type objectType, string toolTipText, int nodeIndex)
        {
            if (objectElement.Name != XmlDataConstants.OBJECTELEMENT)
                throw _exceptionHelper.CriticalException("{2D2B8120-4D0B-4FB1-8252-BF62DE979C8D}");

            ObjectElementTreeNode newTreeNode = new
            (
                string.Format(CultureInfo.CurrentCulture, Strings.objectListChildNodeTextFormat, parentNode.ListInfo.Name, nodeIndex),
                $"{parentNode.Name}/{objectElement.Name}[{nodeIndex + 1}]",
                objectType,
                nodeIndex
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ObjectVariableElementTreeNode AddObjectVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement objectVariableElement, string variableName, Type variableType, string toolTipText)
        {
            if (objectVariableElement.Name != XmlDataConstants.OBJECTVARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{471B39D3-3510-4227-A3FE-DC1EAFBF59A9}");

            ObjectVariableElementTreeNode newTreeNode = new
            (
                variableName,
                $"{parentNode.Name}/{XmlDataConstants.VARIABLEVALUEELEMENT}/{objectVariableElement.Name}",
                variableType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public AssertFunctionElementTreeNode AddRootAssertFunctionTreeNode(RadTreeView radTreeView, XmlElement functionElement, string toolTipText)
        {
            if (functionElement.Name != XmlDataConstants.ASSERTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{9AF6A84A-83D7-48BC-956B-19C0164D2AC8}");

            AssertFunctionElementTreeNode newTreeNode = new
            (
                functionElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                $"/{functionElement.Name}"
            )
            {
                ToolTipText = toolTipText
            };

            radTreeView.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ConstructorElementTreeNode AddRootConstructorTreeNode(RadTreeView radTreeView, XmlElement constructorElement, Type rootAssignedToType, string toolTipText)
        {
            if (constructorElement.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                throw _exceptionHelper.CriticalException("{BBD93393-7327-40A2-AA9B-7F76C6A52F2B}");

            ConstructorElementTreeNode newTreeNode = new
            (
                constructorElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                $"/{constructorElement.Name}",
                rootAssignedToType
            )
            {
                ToolTipText = toolTipText
            };

            radTreeView.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public FunctionElementTreeNode AddRootFunctionTreeNode(RadTreeView radTreeView, XmlElement functionElement, Type rootAssignedToType, string toolTipText)
        {
            if (functionElement.Name != XmlDataConstants.FUNCTIONELEMENT
                && functionElement.Name != XmlDataConstants.NOTELEMENT)
                throw _exceptionHelper.CriticalException("{15EF6982-67D9-4AD9-82E5-F1071FD752F1}");

            FunctionElementTreeNode newTreeNode = new
            (
                _functionDataParser.Parse(functionElement).Name,
                $"/{XmlDataConstants.NOTELEMENT}|/{XmlDataConstants.FUNCTIONELEMENT}",
                rootAssignedToType
            )
            {
                ToolTipText = toolTipText
            };

            radTreeView.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ParameterLiteralListElementTreeNode AddRootLiteralListTreeNode(RadTreeView radTreeView, XmlElement literalListElement, Type rootAssignedToType, LiteralListParameterElementInfo listElementInfo, string text, string toolTipText)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{C83A731F-854D-475A-9BCB-1AA76D22DEB4}");

            ParameterLiteralListElementTreeNode newTreeNode = new
            (
                text,
                $"/{literalListElement.Name}",
                rootAssignedToType,
                listElementInfo
            )
            {
                ToolTipText = toolTipText
            };

            radTreeView.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public VariableLiteralListElementTreeNode AddRootLiteralListTreeNode(RadTreeView radTreeView, XmlElement literalListElement, Type rootAssignedToType, LiteralListVariableElementInfo listElementInfo, string text, string toolTipText)
        {
            if (literalListElement.Name != XmlDataConstants.LITERALLISTELEMENT)
                throw _exceptionHelper.CriticalException("{F5846795-ABB4-4E2C-A138-F8AFB550428B}");

            VariableLiteralListElementTreeNode newTreeNode = new
            (
                text,
                $"/{literalListElement.Name}",
                rootAssignedToType,
                listElementInfo
            )
            {
                ToolTipText = toolTipText
            };

            radTreeView.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public ParameterObjectListElementTreeNode AddRootObjectListTreeNode(RadTreeView radTreeView, XmlElement objectListElement, Type rootAssignedToType, ObjectListParameterElementInfo listElementInfo, string text, string toolTipText)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{242D94DC-EE27-48BF-9B32-3EDC5886F31F}");

            ParameterObjectListElementTreeNode newTreeNode = new
            (
                text,
                $"/{objectListElement.Name}",
                rootAssignedToType,
                listElementInfo
            )
            {
                ToolTipText = toolTipText
            };

            radTreeView.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public VariableObjectListElementTreeNode AddRootObjectListTreeNode(RadTreeView radTreeView, XmlElement objectListElement, Type rootAssignedToType, ObjectListVariableElementInfo listElementInfo, string text, string toolTipText)
        {
            if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                throw _exceptionHelper.CriticalException("{8262F69D-3ED6-46EA-9080-3BAA8AED6105}");

            VariableObjectListElementTreeNode newTreeNode = new
            (
                text,
                $"/{objectListElement.Name}",
                rootAssignedToType,
                listElementInfo
            )
            {
                ToolTipText = toolTipText
            };

            radTreeView.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public RetractFunctionElementTreeNode AddRootRetractFunctionTreeNode(RadTreeView radTreeView, XmlElement functionElement, string toolTipText)
        {
            if (functionElement.Name != XmlDataConstants.RETRACTFUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{F215A7B8-6F3A-44D8-A69A-C691BE6BBA06}");

            RetractFunctionElementTreeNode newTreeNode = new
            (
                functionElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                $"/{functionElement.Name}"
            )
            {
                ToolTipText = toolTipText
            };

            radTreeView.Nodes.Add(newTreeNode);
            return newTreeNode;
        }

        public VariableElementTreeNode AddVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement variableElement, string toolTipText)
        {
            if (variableElement.Name != XmlDataConstants.VARIABLEELEMENT)
                throw _exceptionHelper.CriticalException("{EA9DE633-39F8-4EA9-9773-DECAF38D3C9F}");

            string variableName = variableElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value;
            VariableElementTreeNode newTreeNode = new
            (
                variableName,
                $"{parentNode.Name}/{XmlDataConstants.VARIABLEELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{variableName}\"]",
                parentNode.AssignedToType
            )
            {
                ToolTipText = toolTipText
            };

            parentNode.Nodes.Add(newTreeNode);
            return newTreeNode;
        }
    }
}
