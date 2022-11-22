using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class ApplicationChangedEventArgs : EventArgs
    {
        public ApplicationChangedEventArgs(ApplicationTypeInfo application)
        {
            Application = application;
        }

        public ApplicationTypeInfo Application { get; private set; }
    }
}
