using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlFactory : IEditingControlFactory
    {
        private readonly Func<IEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> _getEditConstructorControl;

        public EditingControlFactory(
            Func<IEditingForm, Constructor, Type, XmlDocument, string, string?, IEditConstructorControl> getEditConstructorControl)
        {
            _getEditConstructorControl = getEditConstructorControl;
        }

        public IEditConstructorControl GetEditConstructorControl(IEditingForm editingForm, Constructor constructor, Type assignedTo, XmlDocument formDocument, string treeNodeXPath, string? selectedParameter = null)
            => _getEditConstructorControl(editingForm, constructor, assignedTo, formDocument, treeNodeXPath, selectedParameter);
    }
}
