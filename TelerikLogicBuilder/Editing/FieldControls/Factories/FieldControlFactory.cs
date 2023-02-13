using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class FieldControlFactory : IFieldControlFactory
    {
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterDomainAutoCompleteControl> _getLiteralParameterDomainAutoCompleteControl;
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterDomainRichInputBoxControl> _getLiteralParameterDomainRichInputBoxControl;
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterDropDownListControl> _getLiteralParameterDropDownListControl;
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl> _getLiteralParameterRichInputBoxControl;

        public FieldControlFactory(
            Func<IEditingControl, LiteralParameter, ILiteralParameterDomainAutoCompleteControl> getLiteralParameterDomainAutoCompleteControl,
            Func<IEditingControl, LiteralParameter, ILiteralParameterDomainRichInputBoxControl> getLiteralParameterDomainRichInputBoxControl,
            Func<IEditingControl, LiteralParameter, ILiteralParameterDropDownListControl> getLiteralParameterDropDownListControl,
            Func<IEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl> getLiteralParameterRichInputBoxControl)
        {
            _getLiteralParameterDomainAutoCompleteControl = getLiteralParameterDomainAutoCompleteControl;
            _getLiteralParameterDomainRichInputBoxControl = getLiteralParameterDomainRichInputBoxControl;
            _getLiteralParameterDropDownListControl = getLiteralParameterDropDownListControl;
            _getLiteralParameterRichInputBoxControl = getLiteralParameterRichInputBoxControl;
        }

        public ILiteralParameterDomainAutoCompleteControl GetLiteralParameterDomainAutoCompleteControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDomainAutoCompleteControl(editingControl, literalParameter);

        public ILiteralParameterDomainRichInputBoxControl GetLiteralParameterDomainRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDomainRichInputBoxControl(editingControl, literalParameter);

        public ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDropDownListControl(editingControl, literalParameter);

        public ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterRichInputBoxControl(editingControl, literalParameter);
    }
}
