using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Office.Interop.Visio;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindConfiguredItemInShapeBase : Telerik.WinControls.UI.RadForm
    {
        protected IConfigurationService _configurationService;
        protected IExceptionHelper _exceptionHelper;
        protected IFindAndReplaceHelper _findAndReplaceHelper;
        protected IFormInitializer _formInitializer;
        protected IRadDropDownListHelper _radDropDownListHelper;
        protected ISearchFunctions _searchFunctions;

        public FindConfiguredItemInShapeBase(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFindAndReplaceHelper findAndReplaceHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            ISearchFunctions searchFunctions)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _findAndReplaceHelper = findAndReplaceHelper;
            _formInitializer = formInitializer;
            _radDropDownListHelper = radDropDownListHelper;
            _searchFunctions = searchFunctions;
            InitializeComponent();
            Initialize();
        }

        #region Variables
        protected Document? visioDocument;
        protected int searchPageIndex = 1;
        protected int searchShapeIndex = 1;
        #endregion Variables

        #region Methods
        public void Setup(Document visioDocument)
        {
            this.visioDocument = visioDocument;
        }

        private void Initialize()
        {
            radDropDownListText.Anchor = AnchorConstants.AnchorsLeftTopRight;
            _formInitializer.SetFormDefaults(this, 624);

            this.AcceptButton = radButtonFindNext;

            InitializeControls();
            GetSettings();

            radButtonCancel.DialogResult = DialogResult.Cancel;
            ValidateOk();
        }

        protected virtual void DoFind()
        {
            throw new NotImplementedException();
        }

        protected virtual void GetSettings()
        {
            radRadioButtonCurrentPage.IsChecked = true;
            /*We'll use the CurrentPage's CheckStateChanged for ResetSearchIndexes() - only one is required*/
            if (Settings.Default.findCurrentPage == false)
                radRadioButtonAllPages.IsChecked = true;
        }

        protected virtual void InitializeControls()
        {
            throw new NotImplementedException();
        }

        protected virtual void SaveSettings()
        {
            throw new NotImplementedException();
        }

        private void FindNext()
        {
            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
            DoFind();
        }

        private void ResetSearchIndexes()
        {
            searchPageIndex = 1;
            searchShapeIndex = 1;
            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
        }

        private void ValidateOk()
        {
            radButtonFindNext.Enabled = radDropDownListText.Text.Length > 0;
        }
        #endregion Methods

        #region Event Handlers
        private void RadDropDownListText_TextChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
            ValidateOk();
        }

        private void RadButtonFindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void RadRadioButtonCurrentPage_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void RadRadioButtonAllPages_CheckStateChanged(object sender, EventArgs e)
        {
            //ResetSearchIndexes();
        }

        private void RadCheckBoxMatchCase_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void RadCheckBoxMatchWholeWord_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void FindConfiguredItemInShapeBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
        #endregion Event Handlers
    }
}
