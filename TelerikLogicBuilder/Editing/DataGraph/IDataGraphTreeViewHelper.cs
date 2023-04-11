using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes;
using System;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph
{
    internal interface IDataGraphTreeViewHelper
    {
        ConstructorElementTreeNode AddConstructorTreeNode(ParametersDataTreeNode parentNode, XmlElement constructorElement, string toolTipText);
        FunctionElementTreeNode AddFunctionTreeNode(ParametersDataTreeNode parentNode, XmlElement functionElement, string toolTipText);
        ListOfLiteralsParameterElementTreeNode AddListOfLiteralsParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListParameterElement, Type parameterType, string toolTipText);
        ListOfLiteralsVariableElementTreeNode AddListOfLiteralsVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListVariableElement, string variableName, Type variableType, string toolTipText);
        ListOfObjectsParameterElementTreeNode AddListOfObjectsParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListParameterElement, Type parameterType, string toolTipText);
        ListOfObjectsVariableElementTreeNode AddListOfObjectsVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListVariableElement, string variableName, Type variableType, string toolTipText);
        ParameterLiteralListElementTreeNode AddLiteralListTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListElement, LiteralListParameterElementInfo listElementInfo, string text, string toolTipText);
        VariableLiteralListElementTreeNode AddLiteralListTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListElement, LiteralListVariableElementInfo listElementInfo, string text, string toolTipText);
        LiteralParameterElementTreeNode AddLiteralParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement literalParameterElement, Type parameterType, string toolTipText);
        LiteralElementTreeNode AddLiteralTreeNode(ParameterLiteralListElementTreeNode parentNode, XmlElement literalElement, Type literalType, string toolTipText, int nodeIndex);
        LiteralElementTreeNode AddLiteralTreeNode(VariableLiteralListElementTreeNode parentNode, XmlElement literalElement, Type literalType, string toolTipText, int nodeIndex);
        LiteralVariableElementTreeNode AddLiteralVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement literalVariableElement, string variableName, Type variableType, string toolTipText);
        ParameterObjectListElementTreeNode AddObjectListTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListElement, ObjectListParameterElementInfo listElementInfo, string text, string toolTipText);
        VariableObjectListElementTreeNode AddObjectListTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListElement, ObjectListVariableElementInfo listElementInfo, string text, string toolTipText);
        ObjectParameterElementTreeNode AddObjectParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement objectParameterElement, Type parameterType, string toolTipText);
        ObjectVariableElementTreeNode AddObjectVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement objectVariableElement, string variableName, Type variableType, string toolTipText);
        ObjectElementTreeNode AddObjectTreeNode(ParameterObjectListElementTreeNode parentNode, XmlElement objectElement, Type objectType, string toolTipText, int nodeIndex);
        ObjectElementTreeNode AddObjectTreeNode(VariableObjectListElementTreeNode parentNode, XmlElement objectElement, Type objectType, string toolTipText, int nodeIndex);
        AssertFunctionElementTreeNode AddRootAssertFunctionTreeNode(RadTreeView radTreeView, XmlElement functionElement, string toolTipText);
        ConstructorElementTreeNode AddRootConstructorTreeNode(RadTreeView radTreeView, XmlElement constructorElement, Type rootAssignedToType, string toolTipText);
        FunctionElementTreeNode AddRootFunctionTreeNode(RadTreeView radTreeView, XmlElement functionElement, Type rootAssignedToType, string toolTipText);
        ParameterLiteralListElementTreeNode AddRootLiteralListTreeNode(RadTreeView radTreeView, XmlElement literalListElement, Type rootAssignedToType, LiteralListParameterElementInfo listElementInfo, string text, string toolTipText);
        ParameterObjectListElementTreeNode AddRootObjectListTreeNode(RadTreeView radTreeView, XmlElement objectListElement, Type rootAssignedToType, ObjectListParameterElementInfo listElementInfo, string text, string toolTipText);
        RetractFunctionElementTreeNode AddRootRetractFunctionTreeNode(RadTreeView radTreeView, XmlElement functionElement, string toolTipText);
        VariableElementTreeNode AddVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement variableElement, string toolTipText);
    }
}
