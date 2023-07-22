using System;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal interface ISplashScreen : IDisposable
    {
        void Close();
        void Show();
    }
}
