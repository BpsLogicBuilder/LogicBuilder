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
    internal class FindReplaceConstructorInShape : FindReplaceConfiguredItemInShapeBase, IFindReplaceConstructorInShape
    {
        public FindReplaceConstructorInShape(
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
                throw _exceptionHelper.CriticalException("{F20F841B-5B4C-4DF9-AEAE-E1884C36F738}");

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
                    _searchFunctions.FindConstructorMatches
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
                    _searchFunctions.FindConstructorMatches
                );
            }
        }

        protected override void DoReplace()
        {
            if (this.selectedShape == null)
                throw _exceptionHelper.CriticalException("{7A8B614F-7789-4730-9D51-F9BAEE5BE895}");

            _findAndReplaceHelper.ReplaceShapeItem
            (
                this.selectedShape,
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
            Settings.Default.findCurrentPage = radRadioButtonCurrentPage.IsChecked;
            Settings.Default.findConstructorMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findConstructorMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.constructorFindWhat = radDropDownListFind.Text;
            Settings.Default.constructorReplaceWith = radDropDownListReplace.Text;
            Settings.Default.Save();
        }
        #endregion Methods
    }
}
