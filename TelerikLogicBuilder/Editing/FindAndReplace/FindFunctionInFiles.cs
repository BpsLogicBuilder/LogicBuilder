using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindFunctionInFiles : FindConfiguredItemInFilesBase, IFindFunctionInFiles
    {
        public FindFunctionInFiles(
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
            radDropDownListText.Text = Settings.Default.functionFindWhat;
        }

        protected override void Initialize()
        {
            this.FormClosing += FindFunctionInFiles_FormClosing;
            base.Initialize();
        }

        protected override void InitializeControls()
        {
            base.InitializeControls();

            this.Text = Strings.findFunctionInFilesFormText;
            IList<string> functions = _configurationService.FunctionList.Functions.Keys.OrderBy(k => k).ToArray();
            if (functions.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListText, functions, RadDropDownStyle.DropDown);
            }
        }

        private void FindFunctionInFiles_FormClosing(object? sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Settings.Default.functionFindWhat = radDropDownListText.Text;
            Settings.Default.Save();
        }
    }
}
