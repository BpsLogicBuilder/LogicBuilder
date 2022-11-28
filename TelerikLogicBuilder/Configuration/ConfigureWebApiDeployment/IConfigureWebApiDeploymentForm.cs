using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment
{
    internal interface IConfigureWebApiDeploymentForm : IDisposable
    {
        DialogResult DialogResult { get; }
        DialogResult ShowDialog(IWin32Window owner);
        WebApiDeployment WebApiDeployment { get; }
    }
}
