namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal interface IObjectListVariableControlsValidator
    {
        void CmbReferenceDefinitionValidating();
        void TxtCastReferenceAsValidating();
        void TxtCastVariableAsValidating();
        void TxtMemberNameValidating();
        void TxtNameChanged();
        void TxtNameValidating();
        void TxtReferenceNameValidating();
        void TxtTypeNameValidating();
        void ValidateForExistingVariableName(string currentVariableNameAttributeValue);
        void ValidateInputBoxes();
        void ValidateObjectType();
    }
}
