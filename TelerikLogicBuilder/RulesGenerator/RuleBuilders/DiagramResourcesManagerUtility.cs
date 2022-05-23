using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders
{
    internal class DiagramResourcesManagerUtility : ResourcesManagerUtilityBase
    {
        public DiagramResourcesManagerUtility(
            IExceptionHelper exceptionHelper,
            IDictionary<string, string> resourceStrings,
            string moduleName) : base(exceptionHelper, resourceStrings, moduleName)
        {
        }

        protected override string Prefix => moduleName;
    }
}
