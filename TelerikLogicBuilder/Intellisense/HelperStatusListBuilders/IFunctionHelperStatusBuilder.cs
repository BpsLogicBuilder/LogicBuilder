using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders
{
    internal interface IFunctionHelperStatusBuilder
    {
        HelperStatus? Build(ReferenceCategories referenceCategory, FunctionCategories functionCategory, string typeName, IList<string> referenceDefinitionArray, IList<string> referenceNameArray, IList<string> castReferenceAsArray, string methodName, IList<ParameterBase> configuredParameters);
    }
}
