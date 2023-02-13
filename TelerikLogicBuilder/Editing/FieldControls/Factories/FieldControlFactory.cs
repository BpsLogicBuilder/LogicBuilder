using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class FieldControlFactory : IFieldControlFactory
    {
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterDropDownListControl> _getLiteralParameterDropDownListControl;
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl> _getLiteralParameterRichInputBoxControl;

        public FieldControlFactory(
            Func<IEditingControl, LiteralParameter, ILiteralParameterDropDownListControl> getLiteralParameterDropDownListControl,
            Func<IEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl> getLiteralParameterRichInputBoxControl)
        {
            _getLiteralParameterDropDownListControl = getLiteralParameterDropDownListControl;
            _getLiteralParameterRichInputBoxControl = getLiteralParameterRichInputBoxControl;
        }

        public ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDropDownListControl(editingControl, literalParameter);

        public ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterRichInputBoxControl(editingControl, literalParameter);
    }
}
