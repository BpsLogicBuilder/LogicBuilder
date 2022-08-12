using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindConstructorInFiles : FindConfiguredItemInFilesBase
    {
        public FindConstructorInFiles(
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
            radDropDownListText.Text = Settings.Default.constructorFindWhat;
        }

        protected override void Initialize()
        {
            this.FormClosing += FindConstructorInFiles_FormClosing;
            base.Initialize();
        }

        protected override void InitializeControls()
        {
            base.InitializeControls();

            this.Text = Strings.findConstructorInFilesFormText;
            IList<string> constructors = _configurationService.ConstructorList.Constructors.Keys.OrderBy(k => k).ToArray();
            if (constructors.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListText, constructors, RadDropDownStyle.DropDown);
            }
        }

        private void FindConstructorInFiles_FormClosing(object? sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            Settings.Default.constructorFindWhat = radDropDownListText.Text;
            Settings.Default.Save();
        }
    }
}
