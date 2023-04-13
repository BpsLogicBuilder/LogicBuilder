using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal interface IFieldControlHelperFactory
    {
        IEditLiteralConstructorHelper GetEditLiteralConstructorHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IEditLiteralFunctionHelper GetEditLiteralFunctionHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IEditLiteralVariableHelper GetEditLiteralVariableHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IEditObjectConstructorHelper GetEditObjectConstructorHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        IEditObjectFunctionHelper GetEditObjectFunctionHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        IEditObjectVariableHelper GetEditObjectVariableHelper(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        IEditParameterLiteralListHelper GetEditParameterLiteralListHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl);
        IEditParameterObjectListHelper GetEditParameterObjectListHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl);
        ILiteralListItemRichInputBoxEventsHelper GetLiteralListItemRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IParameterObjectRichTextBoxEventsHelper GetObjectRichTextBoxEventsHelper(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl);
        IParameterRichInputBoxEventsHelper GetParameterRichInputBoxEventsHelper(IParameterRichInputBoxValueControl richInputBoxValueControl);
        IRichInputBoxEventsHelper GetRichInputBoxEventsHelper(IRichInputBoxValueControl richInputBoxValueControl);
        IUpdateObjectRichTextBoxXml GetUpdateObjectRichTextBoxXml(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
    }
}
