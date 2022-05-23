using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal class TableResourcesManagerUtility : ResourcesManagerUtilityBase
    {
        public TableResourcesManagerUtility(
            IExceptionHelper exceptionHelper,
            IDictionary<string, string> resourceStrings,
            string moduleName) : base(exceptionHelper, resourceStrings, moduleName)
        {
        }

        protected override string Prefix => $"{moduleName}{FileConstants.TABLESTRING}";
    }
}
