using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Factories
{
    internal class EditingControlFactory : IEditingControlFactory
    {
        private readonly Func<IEditingForm, Constructor, Type, IEditConstructorControl> _getEditConstructorControl;

        public EditingControlFactory(
            Func<IEditingForm, Constructor, Type, IEditConstructorControl> getEditConstructorControl)
        {
            _getEditConstructorControl = getEditConstructorControl;
        }

        public IEditConstructorControl GetEditConstructorControl(IEditingForm editingForm, Constructor constructor, Type assignedTo)
            => _getEditConstructorControl(editingForm, constructor, assignedTo);
    }
}
