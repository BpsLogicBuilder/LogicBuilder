using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal interface IFieldControlHelperFactory
    {
        IEditLiteralConstructorHelper GetEditLiteralConstructorHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IEditLiteralFunctionHelper GetEditLiteralFunctionHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IEditLiteralListHelper GetEditLiteralListHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl);
        IEditLiteralVariableHelper GetEditLiteralVariableHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IEditObjectConstructorHelper GetEditObjectConstructorHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        IEditObjectFunctionHelper GetEditObjectFunctionHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        IEditObjectListHelper GetEditObjectListHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl);
        IEditObjectVariableHelper GetEditObjectVariableHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        IObjectRichTextBoxEventsHelper GetObjectRichTextBoxEventsHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl);
        IParameterRichInputBoxEventsHelper GetParameterRichInputBoxEventsHelper(IParameterRichInputBoxValueControl richInputBoxValueControl);
        IRichInputBoxEventsHelper GetRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IUpdateObjectRichTextBoxXml GetUpdateObjectRichTextBoxXml(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
    }
}
