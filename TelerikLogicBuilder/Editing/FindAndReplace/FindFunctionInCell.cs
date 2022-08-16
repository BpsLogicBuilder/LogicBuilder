using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindFunctionInCell : FindConfiguredItemInCellBase
    {
        public FindFunctionInCell(
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
                throw _exceptionHelper.CriticalException("{8D077C1C-F05D-4725-B662-F8CF918619FD}");

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
                    _searchFunctions.FindFunctionMatches
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
                    _searchFunctions.FindFunctionMatches
                );
            }
        }

        protected override void GetSettings()
        {
            base.GetSettings();

            radCheckBoxMatchCase.Checked = Settings.Default.findFunctionMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findFunctionMatchWholeWord;
            radDropDownListText.Text = Settings.Default.functionFindWhat;
        }

        protected override void InitializeControls()
        {
            this.Text = Strings.findFunctionFormText;
            this.radGroupBoxText.Text = Strings.findFunctionFormText;

            IList<string> functions = _configurationService.FunctionList.Functions.Keys.OrderBy(k => k).ToArray();

            if (functions.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListText, functions, RadDropDownStyle.DropDown);
            }
        }

        protected override void SaveSettings()
        {
            Settings.Default.findCurrentRow = radRadioButtonCurrentRow.IsChecked;
            Settings.Default.findFunctionMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findFunctionMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.functionFindWhat = radDropDownListText.Text;
        }
    }
}
