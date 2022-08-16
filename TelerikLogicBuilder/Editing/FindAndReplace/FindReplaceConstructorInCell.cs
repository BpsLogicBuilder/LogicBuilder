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
    internal class FindReplaceConstructorInCell : FindReplaceConfiguredItemInCellBase
    {
        public FindReplaceConstructorInCell(
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
                throw _exceptionHelper.CriticalException("{FE60F3D9-EF20-4E65-A27A-F5EE3C6ED131}");

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
                    _searchFunctions.FindConstructorMatches
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
                    _searchFunctions.FindConstructorMatches
                );
            }
        }

        protected override void DoReplace()
        {
            if (this.dataGridViewCell == null)
                throw _exceptionHelper.CriticalException("{317EABA5-5E75-48B5-8B1B-B292377C8025}");

            if (this.dataSet == null)
                throw _exceptionHelper.CriticalException("{7284529A-984F-444A-93CF-B854143D11A8}");

            _findAndReplaceHelper.ReplaceCellItem
            (
                this.dataGridViewCell,
                this.dataSet,
                radDropDownListFind.Text,
                radDropDownListReplace.Text,
                radCheckBoxMatchCase.Checked,
                radCheckBoxMatchWholeWord.Checked,
                _searchFunctions.ReplaceConstructorMatches,
                _applicationTypeInfoManager.GetApplicationTypeInfo
                (
                    _configurationService.GetSelectedApplication().Name
                )
            );
        }

        protected override void GetSettings()
        {
            base.GetSettings();

            radCheckBoxMatchCase.Checked = Settings.Default.findConstructorMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findConstructorMatchWholeWord;
            radDropDownListFind.Text = Settings.Default.constructorFindWhat;
            radDropDownListReplace.Text = Settings.Default.constructorReplaceWith;
        }

        protected override void InitializeControls()
        {
            this.radDropDownListFind.Text = Strings.findConstructorFormText;
            this.Text = Strings.replaceConstructorFormText;

            IList<string> constructors = _configurationService.ConstructorList.Constructors.Keys.OrderBy(k => k).ToArray();
            if (constructors.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListFind, constructors, RadDropDownStyle.DropDown);
                _radDropDownListHelper.LoadTextItems(radDropDownListReplace, constructors, RadDropDownStyle.DropDown);
            }
        }

        protected override void SaveSettings()
        {
            Settings.Default.findCurrentRow = radRadioButtonCurrentRow.IsChecked;
            Settings.Default.findConstructorMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findConstructorMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.constructorFindWhat = radDropDownListFind.Text;
            Settings.Default.constructorReplaceWith = radDropDownListReplace.Text;
            Settings.Default.Save();
        }
        #endregion Methods
    }
}
