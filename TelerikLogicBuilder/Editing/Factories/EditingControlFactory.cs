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
    internal class EditingControlFactory : IEditingControlFactory
    {
        private readonly Func<IDataGraphEditingForm, Function, Type, XmlDocument, string, string?, IEditBinaryFunctionControl> _getEditBinaryFunctionControl;
        private readonly Func<IDataGraphEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> _getEditConstructorControl;
        private readonly Func<IDataGraphEditingForm, LiteralListParameterElementInfo, Type, XmlDocument, string, int?, IEditLiteralListControl> _getEditLiteralListControl;
        private readonly Func<IDataGraphEditingForm, ObjectListParameterElementInfo, Type, XmlDocument, string, int?, IEditObjectListControl> _getEditObjectListControl;
        private readonly Func<IDataGraphEditingForm, Function, Type, XmlDocument, string, string?, IEditSetValueFunctionControl> _getEditSetValueFunctionControl;
        private readonly Func<IDataGraphEditingForm, Function, Type, XmlDocument, string, string?, IEditSetValueToNullFunctionControl> _getEditSetValueToNullFunctionControl;
        private readonly Func<IDataGraphEditingForm, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> _getEditStandardFunctionControl;
        private readonly Func<IEditingForm, Type, IEditVariableControl> _getEditVariableControl;

        public EditingControlFactory(
            Func<IDataGraphEditingForm, Function, Type, XmlDocument, string, string?, IEditBinaryFunctionControl> getEditBinaryFunctionControl,
            Func<IDataGraphEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> getEditConstructorControl,
            Func<IDataGraphEditingForm, LiteralListParameterElementInfo, Type, XmlDocument, string, int?, IEditLiteralListControl> getEditLiteralListControl,
            Func<IDataGraphEditingForm, ObjectListParameterElementInfo, Type, XmlDocument, string, int?, IEditObjectListControl> getEditObjectListControl,
            Func<IDataGraphEditingForm, Function, Type, XmlDocument, string, string?, IEditSetValueFunctionControl> getEditSetValueFunctionControl,
            Func<IDataGraphEditingForm, Function, Type, XmlDocument, string, string?, IEditSetValueToNullFunctionControl> getEditSetValueToNullFunctionControl,
            Func<IDataGraphEditingForm, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> getEditStandardFunctionControl,
            Func<IEditingForm, Type, IEditVariableControl> getEditVariableControl)
        {
            _getEditBinaryFunctionControl = getEditBinaryFunctionControl;
            _getEditConstructorControl = getEditConstructorControl;
            _getEditLiteralListControl = getEditLiteralListControl;
            _getEditObjectListControl = getEditObjectListControl;
            _getEditSetValueFunctionControl = getEditSetValueFunctionControl;
            _getEditSetValueToNullFunctionControl = getEditSetValueToNullFunctionControl;
            _getEditStandardFunctionControl = getEditStandardFunctionControl;
            _getEditVariableControl = getEditVariableControl;
        }

        public IEditBinaryFunctionControl GetEditBinaryFunctionControl(IDataGraphEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditBinaryFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditConstructorControl GetEditConstructorControl(IDataGraphEditingForm editingForm, Constructor constructor, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditConstructorControl(editingForm, constructor, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditLiteralListControl GetEditLiteralListControl(IDataGraphEditingForm dataGraphEditingForm, LiteralListParameterElementInfo literalListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => _getEditLiteralListControl(dataGraphEditingForm, literalListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedIndex);

        public IEditObjectListControl GetEditObjectListControl(IDataGraphEditingForm dataGraphEditingForm, ObjectListParameterElementInfo objectListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => _getEditObjectListControl(dataGraphEditingForm, objectListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedIndex);

        public IEditSetValueFunctionControl GetEditSetValueFunctionControl(IDataGraphEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditSetValueFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditSetValueToNullFunctionControl GetEditSetValueToNullFunctionControl(IDataGraphEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditSetValueToNullFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditStandardFunctionControl GetEditStandardFunctionControl(IDataGraphEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditStandardFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditVariableControl GetEditVariableControl(IEditingForm editingForm, Type assignedTo)
            => _getEditVariableControl(editingForm, assignedTo);
    }
}
