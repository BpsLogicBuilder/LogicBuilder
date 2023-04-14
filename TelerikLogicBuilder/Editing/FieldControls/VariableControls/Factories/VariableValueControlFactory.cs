using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.Factories
{
    internal class VariableValueControlFactory : IVariableValueControlFactory
    {
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsVariable, ILiteralListVariableRichTextBoxControl> _getLiteralListVariableRichTextBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDomainAutoCompleteControl> _getLiteralVariableDomainAutoCompleteControl;
        private readonly Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDomainMultilineControl> _getLiteralVariableDomainMultilineControl;
        private readonly Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDomainRichInputBoxControl> _getLiteralVariableDomainRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDropDownListControl> _getLiteralVariableDropDownListControl;
        private readonly Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableMultilineControl> _getLiteralVariableMultilineControl;
        private readonly Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariablePropertyInputRichInputBoxControl> _getLiteralVariablePropertyInputRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableRichInputBoxControl> _getLiteralVariableRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableTypeAutoCompleteControl> _getLiteralVariableTypeAutoCompleteControl;
        private readonly Func<IDataGraphEditingControl, ListOfObjectsVariable, IObjectListVariableRichTextBoxControl> _getObjectListVariableRichTextBoxControl;
        private readonly Func<IDataGraphEditingControl, ObjectVariable, IObjectVariableRichTextBoxControl> _getObjectVariableRichTextBoxControl;

        public VariableValueControlFactory(
            Func<IDataGraphEditingControl, ListOfLiteralsVariable, ILiteralListVariableRichTextBoxControl> getLiteralListVariableRichTextBoxControl,
            Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDomainAutoCompleteControl> getLiteralVariableDomainAutoCompleteControl,
            Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDomainMultilineControl> getLiteralVariableDomainMultilineControl,
            Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDomainRichInputBoxControl> getLiteralVariableDomainRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableDropDownListControl> getLiteralVariableDropDownListControl,
            Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableMultilineControl> getLiteralVariableMultilineControl,
            Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariablePropertyInputRichInputBoxControl> getLiteralVariablePropertyInputRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableRichInputBoxControl> getLiteralVariableRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralVariable, ILiteralVariableTypeAutoCompleteControl> getLiteralVariableTypeAutoCompleteControl,
            Func<IDataGraphEditingControl, ListOfObjectsVariable, IObjectListVariableRichTextBoxControl> getObjectListVariableRichTextBoxControl,
            Func<IDataGraphEditingControl, ObjectVariable, IObjectVariableRichTextBoxControl> getObjectVariableRichTextBoxControl)
        {
            _getLiteralListVariableRichTextBoxControl = getLiteralListVariableRichTextBoxControl;
            _getLiteralVariableDomainAutoCompleteControl = getLiteralVariableDomainAutoCompleteControl;
            _getLiteralVariableDomainMultilineControl = getLiteralVariableDomainMultilineControl;
            _getLiteralVariableDomainRichInputBoxControl = getLiteralVariableDomainRichInputBoxControl;
            _getLiteralVariableDropDownListControl = getLiteralVariableDropDownListControl;
            _getLiteralVariableMultilineControl = getLiteralVariableMultilineControl;
            _getLiteralVariablePropertyInputRichInputBoxControl = getLiteralVariablePropertyInputRichInputBoxControl;
            _getLiteralVariableRichInputBoxControl = getLiteralVariableRichInputBoxControl;
            _getLiteralVariableTypeAutoCompleteControl = getLiteralVariableTypeAutoCompleteControl;
            _getObjectListVariableRichTextBoxControl = getObjectListVariableRichTextBoxControl;
            _getObjectVariableRichTextBoxControl = getObjectVariableRichTextBoxControl;
        }

        public ILiteralListVariableRichTextBoxControl GetiteralListVariableRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsVariable listOfLiteralsVariable)
            => _getLiteralListVariableRichTextBoxControl(editingControl, listOfLiteralsVariable);

        public ILiteralVariableDomainAutoCompleteControl GetLiteralVariableDomainAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => _getLiteralVariableDomainAutoCompleteControl(editingControl, literalVariable);

        public ILiteralVariableDomainMultilineControl GetLiteralVariableDomainMultilineControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => _getLiteralVariableDomainMultilineControl(editingControl, literalVariable);

        public ILiteralVariableDomainRichInputBoxControl GetLiteralVariableDomainRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => _getLiteralVariableDomainRichInputBoxControl(editingControl, literalVariable);

        public ILiteralVariableDropDownListControl GetLiteralVariableDropDownListControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => _getLiteralVariableDropDownListControl(editingControl, literalVariable);

        public ILiteralVariableMultilineControl GetLiteralVariableMultilineControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => _getLiteralVariableMultilineControl(editingControl, literalVariable);

        public ILiteralVariablePropertyInputRichInputBoxControl GetLiteralVariablePropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => _getLiteralVariablePropertyInputRichInputBoxControl(editingControl, literalVariable);

        public ILiteralVariableRichInputBoxControl GetLiteralVariableRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => _getLiteralVariableRichInputBoxControl(editingControl, literalVariable);

        public ILiteralVariableTypeAutoCompleteControl GetLiteralVariableTypeAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralVariable literalVariable)
            => _getLiteralVariableTypeAutoCompleteControl(editingControl, literalVariable);

        public IObjectListVariableRichTextBoxControl GetObjectListVariableRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfObjectsVariable listOfObjectsVariable)
            => _getObjectListVariableRichTextBoxControl(editingControl, listOfObjectsVariable);

        public IObjectVariableRichTextBoxControl GetObjectVariableRichTextBoxControl(IDataGraphEditingControl editingControl, ObjectVariable objectVariable)
            => _getObjectVariableRichTextBoxControl(editingControl, objectVariable);
    }
}
