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
    internal class FindReplaceVariableInCell : FindReplaceConfiguredItemInCellBase, IFindReplaceVariableInCell
    {
        public FindReplaceVariableInCell(
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
                throw _exceptionHelper.CriticalException("{D0C06CB0-816E-4830-81F3-3AC519087BC1}");

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
                    _searchFunctions.FindVariableMatches
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
                    _searchFunctions.FindVariableMatches
                );
            }
        }

        protected override void DoReplace()
        {
            if (this.dataGridViewCell == null)
                throw _exceptionHelper.CriticalException("{2BB9182C-20EC-42E8-8E22-C7DDE377178F}");

            if (this.dataSet == null)
                throw _exceptionHelper.CriticalException("{B3E74CF0-642D-4C85-888C-614DB07416F4}");

            _findAndReplaceHelper.ReplaceCellItem
            (
                this.dataGridViewCell,
                this.dataSet,
                radDropDownListFind.Text,
                radDropDownListReplace.Text,
                radCheckBoxMatchCase.Checked,
                radCheckBoxMatchWholeWord.Checked,
                _searchFunctions.ReplaceVariableMatches,
                _applicationTypeInfoManager.GetApplicationTypeInfo
                (
                    _configurationService.GetSelectedApplication().Name
                )
            );
        }

        protected override void GetSettings()
        {
            base.GetSettings();

            radCheckBoxMatchCase.Checked = Settings.Default.findVariableMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findVariableMatchWholeWord;
            radDropDownListFind.Text = Settings.Default.variableFindWhat;
            radDropDownListReplace.Text = Settings.Default.variableReplaceWith;
        }

        protected override void InitializeControls()
        {
            this.radDropDownListFind.Text = Strings.findVariableFormText;
            this.Text = Strings.replaceVariableFormText;

            IList<string> variables = _configurationService.VariableList.Variables.Keys.OrderBy(k => k).ToArray();
            if (variables.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListFind, variables, RadDropDownStyle.DropDown);
                _radDropDownListHelper.LoadTextItems(radDropDownListReplace, variables, RadDropDownStyle.DropDown);
            }
        }

        protected override void SaveSettings()
        {
            Settings.Default.findCurrentRow = radRadioButtonCurrentRow.IsChecked;
            Settings.Default.findVariableMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findVariableMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.variableFindWhat = radDropDownListFind.Text;
            Settings.Default.variableReplaceWith = radDropDownListReplace.Text;
            Settings.Default.Save();
        }
        #endregion Methods
    }
}
