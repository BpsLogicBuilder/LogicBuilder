using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindVariableInCell : FindConfiguredItemInCellBase, IFindVariableInCell
    {
        public FindVariableInCell(
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
                throw _exceptionHelper.CriticalException("{329B0D1E-3961-417E-8351-E054C15A4371}");

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
                    _searchFunctions.FindVariableMatches
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
                    _searchFunctions.FindVariableMatches
                );
            }
        }

        protected override void GetSettings()
        {
            base.GetSettings();

            radCheckBoxMatchCase.Checked = Settings.Default.findVariableMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findVariableMatchWholeWord;
            radDropDownListText.Text = Settings.Default.variableFindWhat;
        }

        protected override void InitializeControls()
        {
            this.Text = Strings.findVariableFormText;
            this.radGroupBoxText.Text = Strings.findVariableFormText;

            IList<string> variables = _configurationService.VariableList.Variables.Keys.OrderBy(k => k).ToArray();

            if (variables.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListText, variables, RadDropDownStyle.DropDown);
            }
        }

        protected override void SaveSettings()
        {
            Settings.Default.findCurrentRow = radRadioButtonCurrentRow.IsChecked;
            Settings.Default.findVariableMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findVariableMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.variableFindWhat = radDropDownListText.Text;
            Settings.Default.Save();
        }
    }
}
