using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Commands
{
    internal class ConfigureFunctionsImportCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILoadFunctions _loadFunctions;
        private readonly IPathHelper _pathHelper;

        private readonly IConfigureFunctionsForm configureFunctionsForm;

        public ConfigureFunctionsImportCommand(
            IConfigurationService configurationService,
            ILoadFunctions loadFunctions,
            IPathHelper pathHelper,
            IConfigureFunctionsForm configureFunctionsForm)
        {
            _configurationService = configurationService;
            _loadFunctions = loadFunctions;
            _pathHelper = pathHelper;
            this.configureFunctionsForm = configureFunctionsForm;
        }

        public override void Execute()
        {
            try
            {
                ((Control)configureFunctionsForm).Cursor = Cursors.WaitCursor;
                using RadOpenFileDialog openFileDialog = new();
                openFileDialog.Filter = $"{Strings.configurationDataFile}|*{FileExtensions.CONFIGFILEEXTENSION}|{Strings.configurationDataFile}|*{FileExtensions.XMLFILEEXTENSION}";
                openFileDialog.MultiSelect = false;
                openFileDialog.InitialDirectory = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
                if (openFileDialog.ShowDialog((IWin32Window)configureFunctionsForm) == DialogResult.OK)
                {
                    configureFunctionsForm.ClearMessage();
                    Import(openFileDialog.FileName);
                }
            }
            catch (XmlException ex)
            {
                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
            catch (XmlValidationException ex)
            {
                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                configureFunctionsForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                ((Control)configureFunctionsForm).Cursor = Cursors.Default;
            }
        }

        private void Import(string fullPath)
        {
            configureFunctionsForm.ReloadXmlDocument(_loadFunctions.Load(fullPath).OuterXml);
            configureFunctionsForm.RebuildTreeView();
        }
    }
}
