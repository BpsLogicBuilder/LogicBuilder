using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal class ParameterFieldControlFactory : IParameterFieldControlFactory
    {
        private readonly Func<IEditConstructorControl, IConstructorGenericParametersControl> _getConstructorGenericParametersControl;
        private readonly Func<IEditFunctionControl, IFunctionGenericParametersControl> _getFunctionGenericParametersControl;
        private readonly Func<IDataGraphEditingControl, ListOfLiteralsParameter, IDictionary<string, ParameterControlSet>, ILiteralListParameterRichTextBoxControl> _getLiteralListParameterRichTextBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDomainAutoCompleteControl> _getLiteralParameterDomainAutoCompleteControl;
        private readonly Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDomainMultilineControl> _getLiteralParameterDomainMultilineControl;
        private readonly Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDomainRichInputBoxControl> _getLiteralParameterDomainRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDropDownListControl> _getLiteralParameterDropDownListControl;
        private readonly Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterMultilineControl> _getLiteralParameterMultilineControl;
        private readonly Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterPropertyInputRichInputBoxControl> _getLiteralParameterPropertyInputRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl> _getLiteralParameterRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralParameter, IDictionary<string, ParameterControlSet>, ILiteralParameterSourcedPropertyRichInputBoxControl> _getLiteralParameterSourcedPropertyRichInputBoxControl;
        private readonly Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterTypeAutoCompleteControl> _getLiteralParameterTypeAutoCompleteControl;
        private readonly Func<IDataGraphEditingControl, ListOfObjectsParameter, IObjectListParameterRichTextBoxControl> _getObjectListParameterRichTextBoxControl;
        private readonly Func<IDataGraphEditingControl, ObjectParameter, IObjectParameterRichTextBoxControl> _getObjectParameterRichTextBoxControl;

        public ParameterFieldControlFactory(
            Func<IEditConstructorControl, IConstructorGenericParametersControl> getConstructorGenericParametersControl,
            Func<IEditFunctionControl, IFunctionGenericParametersControl> getFunctionGenericParametersControl,
            Func<IDataGraphEditingControl, ListOfLiteralsParameter, IDictionary<string, ParameterControlSet>, ILiteralListParameterRichTextBoxControl> getLiteralListParameterRichTextBoxControl,
            Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDomainAutoCompleteControl> getLiteralParameterDomainAutoCompleteControl,
            Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDomainMultilineControl> getLiteralParameterDomainMultilineControl,
            Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDomainRichInputBoxControl> getLiteralParameterDomainRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterDropDownListControl> getLiteralParameterDropDownListControl,
            Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterMultilineControl> getLiteralParameterMultilineControl,
            Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterPropertyInputRichInputBoxControl> getLiteralParameterPropertyInputRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterRichInputBoxControl> getLiteralParameterRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralParameter, IDictionary<string, ParameterControlSet>, ILiteralParameterSourcedPropertyRichInputBoxControl> getLiteralParameterSourcedPropertyRichInputBoxControl,
            Func<IDataGraphEditingControl, LiteralParameter, ILiteralParameterTypeAutoCompleteControl> getLiteralParameterTypeAutoCompleteControl,
            Func<IDataGraphEditingControl, ListOfObjectsParameter, IObjectListParameterRichTextBoxControl> getObjectListParameterRichTextBoxControl,
            Func<IDataGraphEditingControl, ObjectParameter, IObjectParameterRichTextBoxControl> getObjectParameterRichTextBoxControl)
        {
            _getConstructorGenericParametersControl = getConstructorGenericParametersControl;
            _getFunctionGenericParametersControl = getFunctionGenericParametersControl;
            _getLiteralListParameterRichTextBoxControl = getLiteralListParameterRichTextBoxControl;
            _getLiteralParameterDomainAutoCompleteControl = getLiteralParameterDomainAutoCompleteControl;
            _getLiteralParameterDomainMultilineControl = getLiteralParameterDomainMultilineControl;
            _getLiteralParameterDomainRichInputBoxControl = getLiteralParameterDomainRichInputBoxControl;
            _getLiteralParameterDropDownListControl = getLiteralParameterDropDownListControl;
            _getLiteralParameterMultilineControl = getLiteralParameterMultilineControl;
            _getLiteralParameterPropertyInputRichInputBoxControl = getLiteralParameterPropertyInputRichInputBoxControl;
            _getLiteralParameterRichInputBoxControl = getLiteralParameterRichInputBoxControl;
            _getLiteralParameterSourcedPropertyRichInputBoxControl = getLiteralParameterSourcedPropertyRichInputBoxControl;
            _getLiteralParameterTypeAutoCompleteControl = getLiteralParameterTypeAutoCompleteControl;
            _getObjectListParameterRichTextBoxControl = getObjectListParameterRichTextBoxControl;
            _getObjectParameterRichTextBoxControl = getObjectParameterRichTextBoxControl;
        }

        public IConstructorGenericParametersControl GetConstructorGenericParametersControl(IEditConstructorControl editConstructorControl)
            => _getConstructorGenericParametersControl(editConstructorControl);

        public IFunctionGenericParametersControl GetFunctionGenericParametersControl(IEditFunctionControl editFunctionControl)
            => _getFunctionGenericParametersControl(editFunctionControl);

        public ILiteralListParameterRichTextBoxControl GetiteralListParameterRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter listOfLiteralsParameter, IDictionary<string, ParameterControlSet> editControlSet)
            => _getLiteralListParameterRichTextBoxControl(editingControl, listOfLiteralsParameter, editControlSet);

        public ILiteralParameterDomainAutoCompleteControl GetLiteralParameterDomainAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDomainAutoCompleteControl(editingControl, literalParameter);

        public ILiteralParameterDomainMultilineControl GetLiteralParameterDomainMultilineControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDomainMultilineControl(editingControl, literalParameter);

        public ILiteralParameterDomainRichInputBoxControl GetLiteralParameterDomainRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDomainRichInputBoxControl(editingControl, literalParameter);

        public ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterDropDownListControl(editingControl, literalParameter);

        public ILiteralParameterMultilineControl GetLiteralParameterMultilineControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterMultilineControl(editingControl, literalParameter);

        public ILiteralParameterPropertyInputRichInputBoxControl GetLiteralParameterPropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterPropertyInputRichInputBoxControl(editingControl, literalParameter);

        public ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterRichInputBoxControl(editingControl, literalParameter);

        public ILiteralParameterSourcedPropertyRichInputBoxControl GetLiteralParameterSourcedPropertyRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter, IDictionary<string, ParameterControlSet> editControlsSet)
            => _getLiteralParameterSourcedPropertyRichInputBoxControl(editingControl, literalParameter, editControlsSet);

        public ILiteralParameterTypeAutoCompleteControl GetLiteralParameterTypeAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter)
            => _getLiteralParameterTypeAutoCompleteControl(editingControl, literalParameter);

        public IObjectListParameterRichTextBoxControl GetObjectListParameterRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfObjectsParameter listOfObjectsParameter)
            => _getObjectListParameterRichTextBoxControl(editingControl, listOfObjectsParameter);

        public IObjectParameterRichTextBoxControl GetObjectParameterRichTextBoxControl(IDataGraphEditingControl editingControl, ObjectParameter objectParameter)
            => _getObjectParameterRichTextBoxControl(editingControl, objectParameter);
    }
}
