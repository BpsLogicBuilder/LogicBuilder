using ABIS.LogicBuilder.FlowBuilder.Configuration.Forms;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Factories
{
    internal interface IConfigurationFormFactory : IDisposable
    {
        ConfigureProjectProperties GetConfigureProjectProperties(bool openedAsReadOnly);
    }
}
