using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindConstructorInCell : FindConfiguredItemInCellBase
    {
        public FindConstructorInCell(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFindAndReplaceHelper findAndReplaceHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            ISearchFunctions searchFunctions) : base(
                configurationService,
                exceptionHelper,
                findAndReplaceHelper,
                formInitializer,
                radDropDownListHelper,
                searchFunctions)
        {
        }

        protected override void DoFind()
        {
            if (this.dataGridView == null)
                throw _exceptionHelper.CriticalException("{FE60F3D9-EF20-4E65-A27A-F5EE3C6ED131}");

            if (radRadioButtonCurrentRow.IsChecked)
            {
                _findAndReplaceHelper.FindItemCurrentRow
                (
                    this.dataGridView,
                    radListOccurrences,
                    radGroupBoxOccurrences,
                    ref searchCellIndex,
                    radDropDownListText.Text,
                    radCheckBoxMatchCase.Checked,
                    radCheckBoxMatchWholeWord.Checked,
                    _searchFunctions.FindConstructorMatches
                );
            }
            else
            {
                _findAndReplaceHelper.FindItemAllRows
                (
                    this.dataGridView,
                    radListOccurrences,
                    radGroupBoxOccurrences,
                    ref searchRowIndex,
                    ref searchCellIndex,
                    radDropDownListText.Text,
                    radCheckBoxMatchCase.Checked,
                    radCheckBoxMatchWholeWord.Checked,
                    _searchFunctions.FindConstructorMatches
                );
            }
        }

        protected override void GetSettings()
        {
            base.GetSettings();

            radCheckBoxMatchCase.Checked = Settings.Default.findConstructorMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findConstructorMatchWholeWord;
            radDropDownListText.Text = Settings.Default.constructorFindWhat;
        }

        protected override void InitializeControls()
        {
            this.Text = Strings.findConstructorFormText;
            this.radGroupBoxText.Text = Strings.findConstructorFormText;

            IList<string> constructors = _configurationService.ConstructorList.Constructors.Keys.OrderBy(k => k).ToArray();

            if (constructors.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListText, constructors, RadDropDownStyle.DropDown);
            }
        }

        protected override void SaveSettings()
        {
            Settings.Default.findCurrentRow = radRadioButtonCurrentRow.IsChecked;
            Settings.Default.findConstructorMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findConstructorMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.constructorFindWhat = radDropDownListText.Text;
        }
    }
}
