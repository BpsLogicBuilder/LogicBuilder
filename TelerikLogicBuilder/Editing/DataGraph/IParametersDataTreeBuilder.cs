using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph.TreeNodes;
using System;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.DataGraph
{
    internal interface IParametersDataTreeBuilder
    {
        void CreateAssertFunctionTreeProfile(RadTreeView treeView, XmlDocument xmlDocument);
        void CreateConstructorTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType);
        void CreateFunctionTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType);
        void CreateLiteralListTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType, LiteralListParameterElementInfo literalListInfo);
        void CreateLiteralListTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType, LiteralListVariableElementInfo literalListInfo);
        void CreateObjectListTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType, ObjectListParameterElementInfo objectListInfo);
        void CreateObjectListTreeProfile(RadTreeView treeView, XmlDocument xmlDocument, Type rootAssignedToType, ObjectListVariableElementInfo objectListInfo);
        void CreateRetractFunctionTreeProfile(RadTreeView treeView, XmlDocument xmlDocument);
        void RefreshTreeNode(RadTreeView treeView, XmlDocument xmlDocument, ParametersDataTreeNode node);
    }
}
