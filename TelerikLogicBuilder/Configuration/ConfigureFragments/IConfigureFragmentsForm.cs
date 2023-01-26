using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments
{
    internal interface IConfigureFragmentsForm : IConfigurationForm
    {
        HashSet<string> FragmentNames { get; }
    }
}
