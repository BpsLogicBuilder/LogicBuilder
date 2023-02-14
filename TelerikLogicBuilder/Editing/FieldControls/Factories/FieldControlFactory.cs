﻿using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class FieldControlFactory : IFieldControlFactory
    {
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterDomainAutoCompleteControl> _getLiteralParameterDomainAutoCompleteControl;
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterDomainMultilineControl> _getLiteralParameterDomainMultilineControl;
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterDomainRichInputBoxControl> _getLiteralParameterDomainRichInputBoxControl;
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterDropDownListControl> _getLiteralParameterDropDownListControl;
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterMultilineControl> _getLiteralParameterMultilineControl;
        private readonly Func<IEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl> _getLiteralParameterRichInputBoxControl;
        private readonly Func<IEditingControl, ILiteralParameterTypeAutoCompleteControl> _getLiteralParameterTypeAutoCompleteControl;

        public FieldControlFactory(
            Func<IEditingControl, LiteralParameter, ILiteralParameterDomainAutoCompleteControl> getLiteralParameterDomainAutoCompleteControl,
            Func<IEditingControl, LiteralParameter, ILiteralParameterDomainMultilineControl> getLiteralParameterDomainMultilineControl,
            Func<IEditingControl, LiteralParameter, ILiteralParameterDomainRichInputBoxControl> getLiteralParameterDomainRichInputBoxControl,
            Func<IEditingControl, LiteralParameter, ILiteralParameterDropDownListControl> getLiteralParameterDropDownListControl,
            Func<IEditingControl, LiteralParameter, ILiteralParameterMultilineControl> getLiteralParameterMultilineControl,
            Func<IEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl> getLiteralParameterRichInputBoxControl,
            Func<IEditingControl, ILiteralParameterTypeAutoCompleteControl> getLiteralParameterTypeAutoCompleteControl)
        {
            _getLiteralParameterDomainAutoCompleteControl = getLiteralParameterDomainAutoCompleteControl;
            _getLiteralParameterDomainMultilineControl = getLiteralParameterDomainMultilineControl;
            _getLiteralParameterDomainRichInputBoxControl = getLiteralParameterDomainRichInputBoxControl;
            _getLiteralParameterDropDownListControl = getLiteralParameterDropDownListControl;
            _getLiteralParameterMultilineControl = getLiteralParameterMultilineControl;
            _getLiteralParameterRichInputBoxControl = getLiteralParameterRichInputBoxControl;
            _getLiteralParameterTypeAutoCompleteControl = getLiteralParameterTypeAutoCompleteControl;
        }

        public ILiteralParameterDomainAutoCompleteControl GetLiteralParameterDomainAutoCompleteControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDomainAutoCompleteControl(editingControl, literalParameter);

        public ILiteralParameterDomainMultilineControl GetLiteralParameterDomainMultilineControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDomainMultilineControl(editingControl, literalParameter);

        public ILiteralParameterDomainRichInputBoxControl GetLiteralParameterDomainRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDomainRichInputBoxControl(editingControl, literalParameter);

        public ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDropDownListControl(editingControl, literalParameter);

        public ILiteralParameterMultilineControl GetLiteralParameterMultilineControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterMultilineControl(editingControl, literalParameter);

        public ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterRichInputBoxControl(editingControl, literalParameter);

        public ILiteralParameterTypeAutoCompleteControl GetLiteralParameterTypeAutoCompleteControl(IEditingControl editingControl)
            => _getLiteralParameterTypeAutoCompleteControl(editingControl);
    }
}
