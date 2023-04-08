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
        private readonly Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditBinaryFunctionControl> _getEditBinaryFunctionControl;
        private readonly Func<IDataGraphEditingHost, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> _getEditConstructorControl;
        private readonly Func<IDataGraphEditingHost, LiteralListParameterElementInfo, Type, XmlDocument, string, int?, IEditLiteralListControl> _getEditLiteralListControl;
        private readonly Func<IDataGraphEditingHost, ObjectListParameterElementInfo, Type, XmlDocument, string, int?, IEditObjectListControl> _getEditObjectListControl;
        private readonly Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditSetValueFunctionControl> _getEditSetValueFunctionControl;
        private readonly Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditSetValueToNullFunctionControl> _getEditSetValueToNullFunctionControl;
        private readonly Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> _getEditStandardFunctionControl;
        private readonly Func<IEditVariableHost, Type, IEditVariableControl> _getEditVariableControl;

        public EditingControlFactory(
            Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditBinaryFunctionControl> getEditBinaryFunctionControl,
            Func<IDataGraphEditingHost, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> getEditConstructorControl,
            Func<IDataGraphEditingHost, LiteralListParameterElementInfo, Type, XmlDocument, string, int?, IEditLiteralListControl> getEditLiteralListControl,
            Func<IDataGraphEditingHost, ObjectListParameterElementInfo, Type, XmlDocument, string, int?, IEditObjectListControl> getEditObjectListControl,
            Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditSetValueFunctionControl> getEditSetValueFunctionControl,
            Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditSetValueToNullFunctionControl> getEditSetValueToNullFunctionControl,
            Func<IDataGraphEditingHost, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> getEditStandardFunctionControl,
            Func<IEditVariableHost, Type, IEditVariableControl> getEditVariableControl)
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

        public IEditBinaryFunctionControl GetEditBinaryFunctionControl(IDataGraphEditingHost dataGraphEditingHost, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditBinaryFunctionControl(dataGraphEditingHost, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditConstructorControl GetEditConstructorControl(IDataGraphEditingHost dataGraphEditingHost, Constructor constructor, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditConstructorControl(dataGraphEditingHost, constructor, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditLiteralListControl GetEditLiteralListControl(IDataGraphEditingHost dataGraphEditingHost, LiteralListParameterElementInfo literalListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => _getEditLiteralListControl(dataGraphEditingHost, literalListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedIndex);

        public IEditObjectListControl GetEditObjectListControl(IDataGraphEditingHost dataGraphEditingHost, ObjectListParameterElementInfo objectListElementInfo, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, int? selectedIndex)
            => _getEditObjectListControl(dataGraphEditingHost, objectListElementInfo, assignedTo, formDocument, treeNodeXPath, selectedIndex);

        public IEditSetValueFunctionControl GetEditSetValueFunctionControl(IDataGraphEditingHost dataGraphEditingHost, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditSetValueFunctionControl(dataGraphEditingHost, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditSetValueToNullFunctionControl GetEditSetValueToNullFunctionControl(IDataGraphEditingHost dataGraphEditingHost, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditSetValueToNullFunctionControl(dataGraphEditingHost, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditStandardFunctionControl GetEditStandardFunctionControl(IDataGraphEditingHost dataGraphEditingHost, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditStandardFunctionControl(dataGraphEditingHost, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditVariableControl GetEditVariableControl(IEditVariableHost editVariableHost, Type assignedTo)
            => _getEditVariableControl(editVariableHost, assignedTo);
    }
}
