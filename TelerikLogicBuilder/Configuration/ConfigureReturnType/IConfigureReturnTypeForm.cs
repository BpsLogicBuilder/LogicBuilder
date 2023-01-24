using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureReturnType
{
    internal interface IConfigureReturnTypeForm : IApplicationForm
    {
        IList<string> GenericArguments { get; }
        ReturnTypeBase ReturnType { get; }
        void SetOkValidation(bool isValid);
    }
}
