using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindVariableInFiles : FindConfiguredItemInFilesBase
    {
        public FindVariableInFiles(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper) : base(
                configurationService,
                exceptionHelper,
                formInitializer,
                radDropDownListHelper)
        {
        }

        protected override void GetSettings()
        {
            radDropDownListText.Text = Settings.Default.variableFindWhat;
        }

        protected override void Initialize()
        {
            this.FormClosing += FindVariableInFiles_FormClosing;
            base.Initialize();
        }

        protected override void InitializeControls()
        {
            base.InitializeControls();

            this.Text = Strings.findVariableInFilesFormText;
            IList<string> variables = _configurationService.VariableList.Variables.Keys.OrderBy(k => k).ToArray();
            if (variables.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListText, variables, RadDropDownStyle.DropDown);
            }
        }

        private void FindVariableInFiles_FormClosing(object? sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Settings.Default.variableFindWhat = radDropDownListText.Text;
            Settings.Default.Save();
        }
    }
}
