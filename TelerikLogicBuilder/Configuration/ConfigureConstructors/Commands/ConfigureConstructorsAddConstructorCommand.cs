using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsAddConstructorCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsAddConstructorCommand(
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IXmlTreeViewSynchronizerFactory xmlTreeViewSynchronizerFactory,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _configureConstructorsXmlTreeViewSynchronizer = xmlTreeViewSynchronizerFactory.GetConfigureConstructorsXmlTreeViewSynchronizer
            (
                configureConstructorsForm
            );
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public override void Execute()
        {
            DisplayMessage.Show("ConfigureConstructorsAddConstructorCommand");
        }
    }
}
