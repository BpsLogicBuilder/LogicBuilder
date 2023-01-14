using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors
{
    internal interface IConfigureConstructorsForm : IConfigurationForm
    {
        IDictionary<string, Constructor> ConstructorsDictionary { get; }
        IConfigureConstructorsTreeNodeControl CurrentTreeNodeControl { get; }
        ConstructorHelperStatus? HelperStatus { get; }
    }
}
