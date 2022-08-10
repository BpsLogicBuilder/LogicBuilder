using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindVariableInShape : FindConfiguredItemInShapeBase
    {
        public FindVariableInShape(
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
            if (this.visioDocument == null)
                throw _exceptionHelper.CriticalException("{09948DD5-BC8D-4E0A-8FE2-2E4717DBF9B1}");

            if (radRadioButtonCurrentPage.IsChecked)
            {
                _findAndReplaceHelper.FindItemCurrentPage
                (
                    this.visioDocument,
                    radListOccurrences,
                    radGroupBoxOccurrences,
                    ref searchShapeIndex,
                    radDropDownListText.Text,
                    radCheckBoxMatchCase.Checked,
                    radCheckBoxMatchWholeWord.Checked,
                    _searchFunctions.FindVariableMatches
                );
            }
            else
            {
                _findAndReplaceHelper.FindItemAllPages
                (
                    this.visioDocument,
                    radListOccurrences,
                    radGroupBoxOccurrences,
                    ref searchPageIndex,
                    ref searchShapeIndex,
                    radDropDownListText.Text,
                    radCheckBoxMatchCase.Checked,
                    radCheckBoxMatchWholeWord.Checked,
                    _searchFunctions.FindVariableMatches
                );
            }
        }

        protected override void GetSettings()
        {
            if (Settings.Default.findCurrentPage)
                radRadioButtonCurrentPage.IsChecked = true;
            else
                radRadioButtonAllPages.IsChecked = true;

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
            Settings.Default.findCurrentPage = radRadioButtonCurrentPage.IsChecked;
            Settings.Default.findVariableMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findVariableMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.variableFindWhat = radDropDownListText.Text;
        }
    }
}
