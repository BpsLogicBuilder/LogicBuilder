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
    internal class FindReplaceVariableInShape : FindReplaceConfiguredItemInShapeBase, IFindReplaceVariableInShape
    {
        public FindReplaceVariableInShape(
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
                throw _exceptionHelper.CriticalException("{BB491AD2-9265-4970-9F13-4F70B4990011}");

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
                    _searchFunctions.FindVariableMatches
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
                    _searchFunctions.FindVariableMatches
                );
            }
        }

        protected override void DoReplace()
        {
            if (this.selectedShape == null)
                throw _exceptionHelper.CriticalException("{7A805152-C5B1-48F1-94EA-CDF637A60893}");

            _findAndReplaceHelper.ReplaceShapeItem
            (
                this.selectedShape,
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
            Settings.Default.findCurrentPage = radRadioButtonCurrentPage.IsChecked;
            Settings.Default.findVariableMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findVariableMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.variableFindWhat = radDropDownListFind.Text;
            Settings.Default.variableReplaceWith = radDropDownListReplace.Text;
            Settings.Default.Save();
        }
        #endregion Methods
    }
}
