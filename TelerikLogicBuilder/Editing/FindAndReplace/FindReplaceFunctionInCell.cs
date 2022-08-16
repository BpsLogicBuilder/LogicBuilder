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
    internal class FindReplaceFunctionInCell : FindReplaceConfiguredItemInCellBase
    {
        public FindReplaceFunctionInCell(
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
            if (this.dataGridView == null)
                throw _exceptionHelper.CriticalException("{AE178CD5-5C1B-4231-A0B8-A4E0DAB2AFE7}");

            if (radRadioButtonCurrentRow.IsChecked)
            {
                dataGridViewCell = _findAndReplaceHelper.FindItemCurrentRow
                (
                    this.dataGridView,
                    radListOccurrences,
                    radGroupBoxOccurrences,
                    ref searchCellIndex,
                    radDropDownListFind.Text,
                    radCheckBoxMatchCase.Checked,
                    radCheckBoxMatchWholeWord.Checked,
                    _searchFunctions.FindFunctionMatches
                );
            }
            else
            {
                dataGridViewCell = _findAndReplaceHelper.FindItemAllRows
                (
                    this.dataGridView,
                    radListOccurrences,
                    radGroupBoxOccurrences,
                    ref searchRowIndex,
                    ref searchCellIndex,
                    radDropDownListFind.Text,
                    radCheckBoxMatchCase.Checked,
                    radCheckBoxMatchWholeWord.Checked,
                    _searchFunctions.FindFunctionMatches
                );
            }
        }

        protected override void DoReplace()
        {
            if (this.dataGridViewCell == null)
                throw _exceptionHelper.CriticalException("{EACE0A95-86E1-4A25-914B-80FDCA5A3DF6}");

            if (this.dataSet == null)
                throw _exceptionHelper.CriticalException("{E88BECFC-C81D-4E2C-84A5-38756FBDC3E9}");

            _findAndReplaceHelper.ReplaceCellItem
            (
                this.dataGridViewCell,
                this.dataSet,
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
            Settings.Default.findCurrentRow = radRadioButtonCurrentRow.IsChecked;
            Settings.Default.findFunctionMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findFunctionMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.functionFindWhat = radDropDownListFind.Text;
            Settings.Default.functionReplaceWith = radDropDownListReplace.Text;
            Settings.Default.Save();
        }
        #endregion Methods
    }
}
