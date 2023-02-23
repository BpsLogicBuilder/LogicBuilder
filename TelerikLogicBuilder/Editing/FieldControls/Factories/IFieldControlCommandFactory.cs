using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal interface IFieldControlCommandFactory
    {
        AddUpdateConstructorGenericArgumentsCommand GetAddUpdateConstructorGenericArgumentsCommand(IConstructorGenericParametersControl constructorGenericParametersControl);
        ClearRichInputBoxTextCommand GetClearRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        CopyRichInputBoxTextCommand GetCopyRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        CutRichInputBoxTextCommand GetCutRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        DeleteRichInputBoxTextCommand GetDeleteRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        EditObjectRichTextBoxConstructorCommand GetEditObjectRichTextBoxConstructorCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        EditObjectRichTextBoxFunctionCommand GetEditObjectRichTextBoxFunctionCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        EditObjectRichTextBoxLiteralListCommand GetEditObjectRichTextBoxLiteralListCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        EditObjectRichTextBoxObjectListCommand GetEditObjectRichTextBoxObjectListCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        EditObjectRichTextBoxVariableCommand GetEditObjectRichTextBoxVariableCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        EditRichInputBoxConstructorCommand GetEditRichInputBoxConstructorCommand(IRichInputBoxValueControl richInputBoxValueControl);
        EditRichInputBoxFunctionCommand GetEditRichInputBoxFunctionCommand(IRichInputBoxValueControl richInputBoxValueControl);
        EditRichInputBoxVariableCommand GetEditRichInputBoxVariableCommand(IRichInputBoxValueControl richInputBoxValueControl);
        PasteRichInputBoxTextCommand GetPasteRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        SelectDomainItemCommand GetSelectDomainItemCommand(IDomainRichInputBoxValueControl richInputBoxValueControl);
        SelectItemFromPropertyListCommand GetSelectItemFromPropertyListCommand(IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl);
        SelectItemFromReferencesTreeViewCommand GetSelectItemFromReferencesTreeViewCommand(IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl);
        ToCamelCaseRichInputBoxCommand GetToCamelCaseRichInputBoxCommand(IRichInputBoxValueControl richInputBoxValueControl);
    }
}
