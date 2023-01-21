using ABIS.LogicBuilder.FlowBuilder.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions
{
    internal interface IConfigureFunctionsForm : IConfigurationForm
    {
        IDictionary<string, Constructor> ConstructorsDictionary { get; }
        XmlDocument ConstructorsDoc { get; }
        IConfigureFunctionsTreeNodeControl CurrentTreeNodeControl { get; }
        HashSet<string> FunctionNames { get; }
        HelperStatus? HelperStatus { get; set; }
        IDictionary<string, VariableBase> VariablesDictionary { get; }

        void UpdateConstructorsConfiguration(ICollection<Constructor> constructors);
    }
}
