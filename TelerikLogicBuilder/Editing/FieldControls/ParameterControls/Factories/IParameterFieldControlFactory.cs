using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.Factories
{
    internal interface IParameterFieldControlFactory
    {
        IConstructorGenericParametersControl GetConstructorGenericParametersControl(IEditConstructorControl editConstructorControl);
        IFunctionGenericParametersControl GetFunctionGenericParametersControl(IEditFunctionControl editFunctionControl);
        ILiteralListParameterRichTextBoxControl GetiteralListParameterRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfLiteralsParameter listOfLiteralsParameter, IDictionary<string, ParameterControlSet> editControlSet);
        ILiteralParameterDomainAutoCompleteControl GetLiteralParameterDomainAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDomainMultilineControl GetLiteralParameterDomainMultilineControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDomainRichInputBoxControl GetLiteralParameterDomainRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterDropDownListControl GetLiteralParameterDropDownListControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterMultilineControl GetLiteralParameterMultilineControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterPropertyInputRichInputBoxControl GetLiteralParameterPropertyInputRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterRichInputBoxControl GetLiteralParameterRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter);
        ILiteralParameterSourcedPropertyRichInputBoxControl GetLiteralParameterSourcedPropertyRichInputBoxControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter, IDictionary<string, ParameterControlSet> editControlsSet);
        ILiteralParameterTypeAutoCompleteControl GetLiteralParameterTypeAutoCompleteControl(IDataGraphEditingControl editingControl, LiteralParameter literalParameter);
        IObjectListParameterRichTextBoxControl GetObjectListParameterRichTextBoxControl(IDataGraphEditingControl editingControl, ListOfObjectsParameter listOfObjectsParameter);
        IObjectParameterRichTextBoxControl GetObjectParameterRichTextBoxControl(IDataGraphEditingControl editingControl, ObjectParameter objectParameter);
    }
}
