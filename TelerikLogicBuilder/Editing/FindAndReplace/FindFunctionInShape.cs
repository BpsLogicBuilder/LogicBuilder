using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindFunctionInShape : FindConfiguredItemInShapeBase
    {
        public FindFunctionInShape(
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
                throw _exceptionHelper.CriticalException("{8C63346F-9522-4988-8B05-F38C8A794A42}");

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
                    _searchFunctions.FindFunctionMatches
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
            Settings.Default.findCurrentPage = radRadioButtonCurrentPage.IsChecked;
            Settings.Default.findFunctionMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findFunctionMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.functionFindWhat = radDropDownListText.Text;
            Settings.Default.Save();
        }
    }
}
