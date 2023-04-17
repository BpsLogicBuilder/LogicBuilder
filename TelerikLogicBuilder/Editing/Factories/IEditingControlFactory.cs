using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBinaryFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueToNullFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditStandardFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal interface IEditingControlFactory
    {
        IEditBinaryFunctionControl GetEditBinaryFunctionControl(
            IDataGraphEditingHost dataGraphEditingHost,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);

        IEditConstructorControl GetEditConstructorControl(
            IDataGraphEditingHost dataGraphEditingHost,
            Constructor constructor,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);

        IEditParameterLiteralListControl GetEditParameterLiteralListControl(
            IDataGraphEditingHost dataGraphEditingHost,
            LiteralListParameterElementInfo literalListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex);

        IEditParameterObjectListControl GetEditParameterObjectListControl(
            IDataGraphEditingHost dataGraphEditingHost,
            ObjectListParameterElementInfo objectListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex);

        IEditSetValueFunctionControl GetEditSetValueFunctionControl(
            IDataGraphEditingHost dataGraphEditingHost,
            Function function,
            XmlDocument formDocument,
            string treeNodeXPath);

        IEditSetValueToNullFunctionControl GetEditSetValueToNullFunctionControl(
            IDataGraphEditingHost dataGraphEditingHost,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath);

        IEditStandardFunctionControl GetEditStandardFunctionControl(
            IDataGraphEditingHost dataGraphEditingHost,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);

        IEditVariableControl GetEditVariableControl(IEditVariableHost editVariableHost, Type assignedTo);

        IEditVariableLiteralListControl GetEditVariableLiteralListControl(
            IDataGraphEditingHost dataGraphEditingHost,
            LiteralListVariableElementInfo literalListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex);

        IEditVariableObjectListControl GetEditVariableObjectListControl(
            IDataGraphEditingHost dataGraphEditingHost,
            ObjectListVariableElementInfo objectListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex);
    }
}
