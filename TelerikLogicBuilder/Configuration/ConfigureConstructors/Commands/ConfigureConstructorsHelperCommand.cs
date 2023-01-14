using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsHelperCommand : ClickCommandBase
    {
        private readonly IConfigureConstructorsXmlTreeViewSynchronizer _configureConstructorsXmlTreeViewSynchronizer;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers; 
        
        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsHelperCommand(
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
            DisplayMessage.Show("ConfigureConstructorsHelperCommand");
        }
    }
}
