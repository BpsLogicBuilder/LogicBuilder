using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindReplaceFunctionInShape : FindReplaceConfiguredItemInShapeBase
    {
        public FindReplaceFunctionInShape(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFindAndReplaceHelper findAndReplaceHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            ISearchFunctions searchFunctions) : base(
                applicationTypeInfoManager,
                configurationService,
                exceptionHelper,
                findAndReplaceHelper,
                formInitializer,
                radDropDownListHelper,
                searchFunctions)
        {
        }

        #region Methods
        protected override void DoFind()
        {
            if (this.visioDocument == null)
                throw _exceptionHelper.CriticalException("{63B084EB-DE6E-4216-846D-40527681EF43}");

            if (radRadioButtonCurrentPage.IsChecked)
            {
                selectedShape = _findAndReplaceHelper.FindItemCurrentPage
                (
                    this.visioDocument,
                    radListOccurrences,
                    radGroupBoxOccurrences,
                    ref searchShapeIndex,
                    radDropDownListFind.Text,
                    radCheckBoxMatchCase.Checked,
                    radCheckBoxMatchWholeWord.Checked,
                    _searchFunctions.FindFunctionMatches
                );
            }
            else
            {
                selectedShape = _findAndReplaceHelper.FindItemAllPages
                (
                    this.visioDocument,
                    radListOccurrences,
                    radGroupBoxOccurrences,
                    ref searchPageIndex,
                    ref searchShapeIndex,
                    radDropDownListFind.Text,
                    radCheckBoxMatchCase.Checked,
                    radCheckBoxMatchWholeWord.Checked,
                    _searchFunctions.FindFunctionMatches
                );
            }
        }

        protected override void DoReplace()
        {
            if (this.selectedShape == null)
                throw _exceptionHelper.CriticalException("{2E79B4BA-2F8F-441E-A71C-21F652A6EE24}");

            _findAndReplaceHelper.ReplaceShapeItem
            (
                this.selectedShape,
                radDropDownListFind.Text,
                radDropDownListReplace.Text,
                radCheckBoxMatchCase.Checked,
                radCheckBoxMatchWholeWord.Checked,
                _searchFunctions.ReplaceFunctionMatches,
                _applicationTypeInfoManager.GetApplicationTypeInfo
                (
                    _configurationService.GetSelectedApplication().Name
                )
            );
        }

        protected override void GetSettings()
        {
            base.GetSettings();

            radCheckBoxMatchCase.Checked = Settings.Default.findFunctionMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findFunctionMatchWholeWord;
            radDropDownListFind.Text = Settings.Default.functionFindWhat;
            radDropDownListReplace.Text = Settings.Default.functionReplaceWith;
        }

        protected override void InitializeControls()
        {
            this.radDropDownListFind.Text = Strings.findFunctionFormText;
            this.Text = Strings.replaceFunctionFormText;

            IList<string> functions = _configurationService.FunctionList.Functions.Keys.OrderBy(k => k).ToArray();
            if (functions.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListFind, functions, RadDropDownStyle.DropDown);
                _radDropDownListHelper.LoadTextItems(radDropDownListReplace, functions, RadDropDownStyle.DropDown);
            }
        }

        protected override void SaveSettings()
        {
            Settings.Default.findCurrentPage = radRadioButtonCurrentPage.IsChecked;
            Settings.Default.findFunctionMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findFunctionMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.functionFindWhat = radDropDownListFind.Text;
            Settings.Default.functionReplaceWith = radDropDownListReplace.Text;
            Settings.Default.Save();
        }
        #endregion Methods
    }
}
