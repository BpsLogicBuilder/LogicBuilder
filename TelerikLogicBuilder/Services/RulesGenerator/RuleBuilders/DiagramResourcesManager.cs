using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.RuleBuilders;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.RuleBuilders;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.RuleBuilders
{
    internal class DiagramResourcesManager : IDiagramResourcesManager
    {
        private readonly IExceptionHelper _exceptionHelper;

        public DiagramResourcesManager(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public string GetShortString(XmlNode xmlNode, IDictionary<string, string> resourceStrings, string moduleName) 
            => new DiagramResourcesManagerUtility
            (
                _exceptionHelper,
                resourceStrings,
                moduleName
            ).GetShortString(xmlNode);

        public string GetShortString(string longString, IDictionary<string, string> resourceStrings, string moduleName)
            => new DiagramResourcesManagerUtility
            (
                _exceptionHelper,
                resourceStrings,
                moduleName
            ).GetShortString(longString);
    }
}
