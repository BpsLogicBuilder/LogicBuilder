using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Commands
{
    internal class ConfigureConstructorsImportCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILoadConstructors _loadConstructors;
        private readonly IPathHelper _pathHelper;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorsImportCommand(
            IConfigurationService configurationService,
            ILoadConstructors loadConstructors,
            IPathHelper pathHelper,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            _configurationService = configurationService;
            _loadConstructors = loadConstructors;
            _pathHelper = pathHelper;
            this.configureConstructorsForm = configureConstructorsForm;
        }

        public override void Execute()
        {
            try
            {
                using RadOpenFileDialog openFileDialog = new();
                openFileDialog.Filter = string.Concat(Strings.configurationDataFile, "|*", FileExtensions.CONFIGFILEEXTENSION);
                openFileDialog.MultiSelect = false;
                openFileDialog.InitialDirectory = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    configureConstructorsForm.ClearMessage();
                    Import(openFileDialog.FileName);
                }
            }
            catch (XmlException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
            catch (XmlValidationException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                configureConstructorsForm.SetErrorMessage(ex.Message);
            }
        }

        private void Import(string fullPath)
        {
            configureConstructorsForm.ReloadXmlDocument(_loadConstructors.Load(fullPath).OuterXml);
            configureConstructorsForm.RebuildTreeView();
        }
    }
}
