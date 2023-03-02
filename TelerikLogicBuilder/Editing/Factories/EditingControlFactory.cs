using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditBinaryFunction;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
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
        private readonly Func<IDataGraphEditingForm, LiteralListElementInfo, Type, XmlDocument, string, int?, IEditLiteralListControl> _getEditLiteralListControl;
        private readonly Func<IDataGraphEditingForm, ObjectListElementInfo, Type, XmlDocument, string, int?, IEditObjectListControl> _getEditObjectListControl;
        private readonly Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> _getEditStandardFunctionControl;
        private readonly Func<IEditingForm, Type, IEditVariableControl> _getEditVariableControl;

        public EditingControlFactory(
            Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditBinaryFunctionControl> getEditBinaryFunctionControl,
            Func<IEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> getEditConstructorControl,
            Func<IDataGraphEditingForm, LiteralListElementInfo, Type, XmlDocument, string, int?, IEditLiteralListControl> getEditLiteralListControl,
            Func<IDataGraphEditingForm, ObjectListElementInfo, Type, XmlDocument, string, int?, IEditObjectListControl> getEditObjectListControl,
            Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> getEditStandardFunctionControl,
            Func<IEditingForm, Type, IEditVariableControl> getEditVariableControl)
        {
            _getEditBinaryFunctionControl = getEditBinaryFunctionControl;
            _getEditConstructorControl = getEditConstructorControl;
            _getEditLiteralListControl = getEditLiteralListControl;
            _getEditObjectListControl = getEditObjectListControl;
            _getEditStandardFunctionControl = getEditStandardFunctionControl;
            _getEditVariableControl = getEditVariableControl;
        }

        public IEditBinaryFunctionControl GetEditBinaryFunctionControl(IEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditBinaryFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditConstructorControl GetEditConstructorControl(IEditingForm editingForm, Constructor constructor, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditConstructorControl(editingForm, constructor, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditLiteralListControl GetEditLiteralListControl(IDataGraphEditingForm dataGraphEditingForm, LiteralListElementInfo literalListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => _getEditLiteralListControl(dataGraphEditingForm, literalListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedIndex);

        public IEditObjectListControl GetEditObjectListControl(IDataGraphEditingForm dataGraphEditingForm, ObjectListElementInfo objectListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => _getEditObjectListControl(dataGraphEditingForm, objectListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedIndex);

        public IEditStandardFunctionControl GetEditStandardFunctionControl(IEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditStandardFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditVariableControl GetEditVariableControl(IEditingForm editingForm, Type assignedTo)
            => _getEditVariableControl(editingForm, assignedTo);
    }
}
