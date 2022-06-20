using System.ComponentModel;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal interface IClickCommand : INotifyPropertyChanged
    {
        void Execute();
    }
}
