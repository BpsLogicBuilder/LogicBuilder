using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class ExitCommand : ClickCommandBase
    {
        private readonly IMainWindow mainWindow;

        public ExitCommand(IMainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public override void Execute()
        {
            this.mainWindow.Instance.Close();
        }
    }
}
