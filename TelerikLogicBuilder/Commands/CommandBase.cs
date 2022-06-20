using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal abstract class CommandBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public abstract void Execute();
    }
}
