namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal interface IVariableControlsValidator
    {
        void CmbReferenceDefinitionValidating();
        void TxtCastReferenceAsValidating();
        void TxtCastVariableAsValidating();
        void TxtMemberNameValidating();
        void TxtNameChanged();
        void TxtNameValidating();
        void TxtReferenceNameValidating();
        void TxtTypeNameValidating();
        void ValidateCastVariableAs();
        void ValidateForExistingVariableName(string currentVariableNameAttributeValue);
        void ValidateMemberName();
        void ValidateReferenceDefinition();
        void ValidateReferenceName();
        void ValidateTypeName();
        void ValidateVariableName();
    }
}
