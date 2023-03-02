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
        LiteralListParameterElementTreeNode AddLiteralListParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListParameterElement, Type parameterType, string toolTipText);
        LiteralListElementTreeNode AddLiteralListTreeNode(ParametersDataTreeNode parentNode, XmlElement literalListElement, LiteralListElementInfo listElementInfo, string text, string toolTipText);
        LiteralParameterElementTreeNode AddLiteralParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement literalParameterElement, Type parameterType, string toolTipText);
        LiteralElementTreeNode AddLiteralTreeNode(LiteralListElementTreeNode parentNode, XmlElement literalElement, Type literalType, string toolTipText, int nodeIndex);
        ObjectListParameterElementTreeNode AddObjectListParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListParameterElement, Type parameterType, string toolTipText);
        ObjectListElementTreeNode AddObjectListTreeNode(ParametersDataTreeNode parentNode, XmlElement objectListElement, ObjectListElementInfo listElementInfo, string text, string toolTipText);
        ObjectParameterElementTreeNode AddObjectParameterTreeNode(ParametersDataTreeNode parentNode, XmlElement objectParameterElement, Type parameterType, string toolTipText);
        ObjectElementTreeNode AddObjectTreeNode(ObjectListElementTreeNode parentNode, XmlElement objectElement, Type objectType, string toolTipText, int nodeIndex);
        ConstructorElementTreeNode AddRootConstructorTreeNode(RadTreeView radTreeView, XmlElement constructorElement, Type rootAssignedToType, string toolTipText);
        FunctionElementTreeNode AddRootFunctionTreeNode(RadTreeView radTreeView, XmlElement functionElement, Type rootAssignedToType, string toolTipText);
        LiteralListElementTreeNode AddRootLiteralListTreeNode(RadTreeView radTreeView, XmlElement literalListElement, Type rootAssignedToType, LiteralListElementInfo listElementInfo, string text, string toolTipText);
        ObjectListElementTreeNode AddRootObjectListTreeNode(RadTreeView radTreeView, XmlElement objectListElement, Type rootAssignedToType, ObjectListElementInfo listElementInfo, string text, string toolTipText);
        VariableElementTreeNode AddVariableTreeNode(ParametersDataTreeNode parentNode, XmlElement variableElement, string toolTipText);
    }
}
