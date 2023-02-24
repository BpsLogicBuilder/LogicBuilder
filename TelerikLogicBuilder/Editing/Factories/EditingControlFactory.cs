using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlFactory : IEditingControlFactory
    {
        private readonly Func<IEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> _getEditConstructorControl;
        private readonly Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> _getEditStandardFunctionControl;

        public EditingControlFactory(
            Func<IEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> getEditConstructorControl,
            Func<IEditingForm, Function, Type, XmlDocument, string, string?, IEditStandardFunctionControl> getEditStandardFunctionControl)
        {
            _getEditConstructorControl = getEditConstructorControl;
            _getEditStandardFunctionControl = getEditStandardFunctionControl;
        }

        public IEditConstructorControl GetEditConstructorControl(IEditingForm editingForm, Constructor constructor, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditConstructorControl(editingForm, constructor, assignedTo, formDocument, treeNodeXPath, selectedParameter);

        public IEditStandardFunctionControl GetEditStandardFunctionControl(IEditingForm editingForm, Function function, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditStandardFunctionControl(editingForm, function, assignedTo, formDocument, treeNodeXPath, selectedParameter);
    }
}
