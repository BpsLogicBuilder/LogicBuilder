using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.ConfigureFunctionsHelper
{
    internal interface IConfigureClassFunctionsHelperForm : IConfiguredItemHelperForm
    {
        IList<Function> Functions { get; }
        ICollection<Constructor> NewConstructors { get; }
    }
}
