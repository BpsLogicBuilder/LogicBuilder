using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBinaryFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueToNullFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlFactory : IEditingControlFactory
    {
        private readonly Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditBinaryFunctionControl> _getEditBinaryFunctionControl;
        private readonly Func<IEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> _getEditConstructorControl;
        private readonly Func<IDataGraphEditingForm, LiteralListParameterElementInfo, Type, XmlDocument, string, int?, IEditLiteralListControl> _getEditLiteralListControl;
        private readonly Func<IDataGraphEditingForm, ObjectListElementInfo, Type, XmlDocument, string, int?, IEditObjectListControl> _getEditObjectListControl;
        private readonly Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditSetValueFunctionControl> _getEditSetValueFunctionControl;
        private readonly Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditSetValueToNullFunctionControl> _getEditSetValueToNullFunctionControl;
        private readonly Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> _getEditStandardFunctionControl;
        private readonly Func<IEditingForm, Type, IEditVariableControl> _getEditVariableControl;

        public EditingControlFactory(
            Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditBinaryFunctionControl> getEditBinaryFunctionControl,
            Func<IEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> getEditConstructorControl,
            Func<IDataGraphEditingForm, LiteralListParameterElementInfo, Type, XmlDocument, string, int?, IEditLiteralListControl> getEditLiteralListControl,
            Func<IDataGraphEditingForm, ObjectListElementInfo, Type, XmlDocument, string, int?, IEditObjectListControl> getEditObjectListControl,
            Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditSetValueFunctionControl> getEditSetValueFunctionControl,
            Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditSetValueToNullFunctionControl> getEditSetValueToNullFunctionControl,
            Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> getEditStandardFunctionControl,
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

        public IEditBinaryFunctionControl GetEditBinaryFunctionControl(IEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditBinaryFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditConstructorControl GetEditConstructorControl(IEditingForm editingForm, Constructor constructor, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditConstructorControl(editingForm, constructor, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditLiteralListControl GetEditLiteralListControl(IDataGraphEditingForm dataGraphEditingForm, LiteralListParameterElementInfo literalListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => _getEditLiteralListControl(dataGraphEditingForm, literalListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedIndex);

        public IEditObjectListControl GetEditObjectListControl(IDataGraphEditingForm dataGraphEditingForm, ObjectListElementInfo objectListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => _getEditObjectListControl(dataGraphEditingForm, objectListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedIndex);

        public IEditSetValueFunctionControl GetEditSetValueFunctionControl(IEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditSetValueFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditSetValueToNullFunctionControl GetEditSetValueToNullFunctionControl(IEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditSetValueToNullFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditStandardFunctionControl GetEditStandardFunctionControl(IEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditStandardFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditVariableControl GetEditVariableControl(IEditingForm editingForm, Type assignedTo)
            => _getEditVariableControl(editingForm, assignedTo);
    }
}
