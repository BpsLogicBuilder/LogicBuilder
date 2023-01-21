namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers
{
    internal interface IFunctionControlsValidator
    {
        void CmbReferenceDefinitionValidating();
        void TxtCastReferenceAsValidating();
        void TxtFunctionNameValidating();
        void TxtMemberNameValidating();
        void TxtReferenceNameValidating();
        void TxtTypeNameValidating();
        void ValidateInputBoxes();
    }
}
