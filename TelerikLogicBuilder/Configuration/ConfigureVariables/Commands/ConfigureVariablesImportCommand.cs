using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Commands
{
    internal class ConfigureVariablesImportCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILoadVariables _loadVariables;
        private readonly IPathHelper _pathHelper;
        private readonly IConfigureVariablesForm configureVariablesForm;

        public ConfigureVariablesImportCommand(
            IConfigurationService configurationService,
            ILoadVariables loadVariables,
            IPathHelper pathHelper,
            IConfigureVariablesForm configureVariablesForm)
        {
            _configurationService = configurationService;
            _loadVariables = loadVariables;
            _pathHelper = pathHelper;
            this.configureVariablesForm = configureVariablesForm;
        }

        public override void Execute()
        {
            try
            {
                ((Control)configureVariablesForm).Cursor = Cursors.WaitCursor;
                using RadOpenFileDialog openFileDialog = new();
                openFileDialog.Filter = $"{Strings.configurationDataFile}|*{FileExtensions.CONFIGFILEEXTENSION}|{Strings.configurationDataFile}|*{FileExtensions.XMLFILEEXTENSION}";
                openFileDialog.MultiSelect = false;
                openFileDialog.InitialDirectory = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
                if (openFileDialog.ShowDialog((IWin32Window)configureVariablesForm) == DialogResult.OK)
                {
                    configureVariablesForm.ClearMessage();
                    Import(openFileDialog.FileName);
                }
            }
            catch (XmlException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
            catch (XmlValidationException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                configureVariablesForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                ((Control)configureVariablesForm).Cursor = Cursors.Default;
            }
        }

        private void Import(string fullPath)
        {
            configureVariablesForm.ReloadXmlDocument(_loadVariables.Load(fullPath).OuterXml);
            configureVariablesForm.RebuildTreeView();
        }
    }
}
