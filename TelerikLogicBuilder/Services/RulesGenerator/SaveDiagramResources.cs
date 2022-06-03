using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class SaveDiagramResources : ISaveDiagramResources
    {
        private readonly ISaveResources _saveResources;

        public SaveDiagramResources(ISaveResources saveResources)
        {
            _saveResources = saveResources;
        }

        public void Save(string sourceFile, IDictionary<string, string> resourceStrings)
        {
            _saveResources.Save
            (
                sourceFile,
                resourceStrings,
                ProjectPropertiesConstants.DIAGRAMFOLDER
            );
        }
    }
}
