using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class FindConstructorInShape : FindConfiguredItemInShapeBase, IFindConstructorInShape
    {
        public FindConstructorInShape(
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
                throw _exceptionHelper.CriticalException("{A130F223-9E37-45E8-B132-6B26FE9B148D}");

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
                    _searchFunctions.FindConstructorMatches
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
                    _searchFunctions.FindConstructorMatches
                );
            }
        }

        protected override void GetSettings()
        {
            base.GetSettings();

            radCheckBoxMatchCase.Checked = Settings.Default.findConstructorMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findConstructorMatchWholeWord;
            radDropDownListText.Text = Settings.Default.constructorFindWhat;
        }

        protected override void InitializeControls()
        {
            this.Text = Strings.findConstructorFormText;
            this.radGroupBoxText.Text = Strings.findConstructorFormText;

            IList<string> constructors = _configurationService.ConstructorList.Constructors.Keys.OrderBy(k => k).ToArray();

            if (constructors.Count > 0)
            {
                _radDropDownListHelper.LoadTextItems(radDropDownListText, constructors, RadDropDownStyle.DropDown);
            }
        }

        protected override void SaveSettings()
        {
            Settings.Default.findCurrentPage = radRadioButtonCurrentPage.IsChecked;
            Settings.Default.findConstructorMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findConstructorMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.constructorFindWhat = radDropDownListText.Text;
            Settings.Default.Save();
        }
    }
}
