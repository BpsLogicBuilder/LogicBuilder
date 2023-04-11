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
        ListOfObjectsParameterElementTreeNode AddListOfObjectsParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListParameterElement, Type parameterType, string toolTipText);
        ParameterLiteralListElementTreeNode AddLiteralListTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListElement, LiteralListParameterElementInfo listElementInfo, string text, string toolTipText);
        LiteralParameterElementTreeNode AddLiteralParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement literalParameterElement, Type parameterType, string toolTipText);
        LiteralElementTreeNode AddLiteralTreeNode(ParameterLiteralListElementTreeNode parentNode, XmlElement literalElement, Type literalType, string toolTipText, int nodeIndex);
        ParameterObjectListElementTreeNode AddObjectListTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListElement, ObjectListParameterElementInfo listElementInfo, string text, string toolTipText);
        ObjectParameterElementTreeNode AddObjectParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement objectParameterElement, Type parameterType, string toolTipText);
        ObjectElementTreeNode AddObjectTreeNode(ParameterObjectListElementTreeNode parentNode, XmlElement objectElement, Type objectType, string toolTipText, int nodeIndex);
        ConstructorElementTreeNode AddRootConstructorTreeNode(RadTreeView radTreeView, XmlElement constructorElement, Type rootAssignedToType, string toolTipText);
        FunctionElementTreeNode AddRootFunctionTreeNode(RadTreeView radTreeView, XmlElement functionElement, Type rootAssignedToType, string toolTipText);
        ParameterLiteralListElementTreeNode AddRootLiteralListTreeNode(RadTreeView radTreeView, XmlElement literalListElement, Type rootAssignedToType, LiteralListParameterElementInfo listElementInfo, string text, string toolTipText);
        ParameterObjectListElementTreeNode AddRootObjectListTreeNode(RadTreeView radTreeView, XmlElement objectListElement, Type rootAssignedToType, ObjectListParameterElementInfo listElementInfo, string text, string toolTipText);
        VariableElementTreeNode AddVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement variableElement, string toolTipText);
    }
}
