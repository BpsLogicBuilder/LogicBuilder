using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Commands
{
    internal class ConfigureFragmentsImportCommand : ClickCommandBase
    {
        private readonly IConfigurationService _configurationService;
        private readonly ILoadFragments _loadFragments;
        private readonly IPathHelper _pathHelper;
        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentsImportCommand(
            IConfigurationService configurationService,
            ILoadFragments loadFragments,
            IPathHelper pathHelper,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            _configurationService = configurationService;
            _loadFragments = loadFragments;
            _pathHelper = pathHelper;
            this.configureFragmentsForm = configureFragmentsForm;
        }

        public override void Execute()
        {
            try
            {
                ((Control)configureFragmentsForm).Cursor = Cursors.WaitCursor;
                using RadOpenFileDialog openFileDialog = new();
                openFileDialog.Filter = string.Concat(Strings.configurationDataFile, "|*", FileExtensions.CONFIGFILEEXTENSION);
                openFileDialog.MultiSelect = false;
                openFileDialog.InitialDirectory = _pathHelper.CombinePaths(_configurationService.ProjectProperties.ProjectPath, ProjectPropertiesConstants.SOURCEDOCUMENTFOLDER);
                if (openFileDialog.ShowDialog((IWin32Window)configureFragmentsForm) == DialogResult.OK)
                {
                    configureFragmentsForm.ClearMessage();
                    Import(openFileDialog.FileName);
                }
            }
            catch (XmlException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
            catch (XmlValidationException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
            finally
            {
                ((Control)configureFragmentsForm).Cursor = Cursors.Default;
            }
        }

        private void Import(string fullPath)
        {
            configureFragmentsForm.ReloadXmlDocument(_loadFragments.Load(fullPath).OuterXml);
            configureFragmentsForm.RebuildTreeView();
        }
    }
}
