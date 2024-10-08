﻿using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories
{
    internal interface IFieldControlCommandFactory
    {
        AddUpdateConstructorGenericArgumentsCommand GetAddUpdateConstructorGenericArgumentsCommand(IConstructorGenericParametersControl constructorGenericParametersControl);
        AddUpdateFunctionGenericArgumentsCommand GetAddUpdateFunctionGenericArgumentsCommand(IFunctionGenericParametersControl functionGenericParametersControl);
        ClearRichInputBoxTextCommand GetClearRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        CopyRichInputBoxTextCommand GetCopyRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        CutRichInputBoxTextCommand GetCutRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        DeleteRichInputBoxTextCommand GetDeleteRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        EditObjectRichTextBoxConstructorCommand GetEditObjectRichTextBoxConstructorCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        EditObjectRichTextBoxFunctionCommand GetEditObjectRichTextBoxFunctionCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        EditParameterObjectRichTextBoxLiteralListCommand GetEditParameterObjectRichTextBoxLiteralListCommand(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl);
        EditParameterObjectRichTextBoxObjectListCommand GetEditParameterObjectRichTextBoxObjectListCommand(IParameterRichTextBoxValueControl parameterRichTextBoxValueControl);
        EditObjectRichTextBoxVariableCommand GetEditObjectRichTextBoxVariableCommand(IObjectRichTextBoxValueControl objectRichTextBoxValueControl);
        EditRichInputBoxConstructorCommand GetEditRichInputBoxConstructorCommand(IRichInputBoxValueControl richInputBoxValueControl);
        EditRichInputBoxFunctionCommand GetEditRichInputBoxFunctionCommand(IRichInputBoxValueControl richInputBoxValueControl);
        EditRichInputBoxVariableCommand GetEditRichInputBoxVariableCommand(IRichInputBoxValueControl richInputBoxValueControl);
        EditVariableObjectRichTextBoxLiteralListCommand GetEditVariableObjectRichTextBoxLiteralListCommand(IVariableRichTextBoxValueControl variableRichTextBoxValueControl);
        EditVariableObjectRichTextBoxObjectListCommand GetEditVariableObjectRichTextBoxObjectListCommand(IVariableRichTextBoxValueControl variableRichTextBoxValueControl);
        PasteRichInputBoxTextCommand GetPasteRichInputBoxTextCommand(IRichInputBoxValueControl richInputBoxValueControl);
        SelectDomainItemCommand GetSelectDomainItemCommand(IDomainRichInputBoxValueControl richInputBoxValueControl);
        SelectItemFromPropertyListCommand GetSelectItemFromPropertyListCommand(IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl);
        SelectItemFromReferencesTreeViewCommand GetSelectItemFromReferencesTreeViewCommand(IPropertyInputRichInputBoxControl propertyInputRichInputBoxControl);
        ToCamelCaseRichInputBoxCommand GetToCamelCaseRichInputBoxCommand(IRichInputBoxValueControl richInputBoxValueControl);
    }
}
