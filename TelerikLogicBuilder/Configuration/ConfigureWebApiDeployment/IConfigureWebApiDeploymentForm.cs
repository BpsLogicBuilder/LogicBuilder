using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment
{
    internal interface IConfigureWebApiDeploymentForm : IDisposable
    {
        WebApiDeployment WebApiDeployment { get; }
    }
}
