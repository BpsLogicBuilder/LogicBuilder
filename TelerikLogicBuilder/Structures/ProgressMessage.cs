namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class ProgressMessage
    {
        public ProgressMessage(int progress, string message)
        {
            Progress = progress;
            Message = message;
        }

        public int Progress { get; set; }
        public string Message { get; set; }
    }
}
