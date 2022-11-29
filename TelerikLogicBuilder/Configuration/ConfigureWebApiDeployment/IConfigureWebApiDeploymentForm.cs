using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureWebApiDeployment
{
    internal interface IConfigureWebApiDeploymentForm : IForm
    {
        WebApiDeployment WebApiDeployment { get; }
    }
}
