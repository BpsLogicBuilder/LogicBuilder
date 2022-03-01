using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions
{
    internal interface IFunctionValidationHelper
    {
        void ValidateFunctionIndirectReferenceName(ValidIndirectReference validIndirectReference, string referenceName, string functionName, ICollection<string> errors);
    }
}
