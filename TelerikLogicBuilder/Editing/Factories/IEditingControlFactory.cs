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
            IEditingForm editingForm,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);

        IEditConstructorControl GetEditConstructorControl(
            IEditingForm editingForm,
            Constructor constructor,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);

        IEditLiteralListControl GetEditLiteralListControl(
            IDataGraphEditingForm dataGraphEditingForm,
            LiteralListParameterElementInfo literalListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex);

        IEditObjectListControl GetEditObjectListControl(
            IDataGraphEditingForm dataGraphEditingForm,
            ObjectListParameterElementInfo objectListElementInfo,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            int? selectedIndex);

        IEditSetValueFunctionControl GetEditSetValueFunctionControl(
            IEditingForm editingForm,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);

        IEditSetValueToNullFunctionControl GetEditSetValueToNullFunctionControl(
            IEditingForm editingForm,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);

        IEditStandardFunctionControl GetEditStandardFunctionControl(
            IEditingForm editingForm,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null);

        IEditVariableControl GetEditVariableControl(IEditingForm editingForm, Type assignedTo);
    }
}
