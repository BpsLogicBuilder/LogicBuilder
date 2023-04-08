namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal interface ISetDialogMessages
    {
        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}
