using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Office.Interop.Visio;
using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindReplaceConfiguredItemInShapeBase : RadForm
    {
        protected readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        protected readonly IConfigurationService _configurationService;
        protected readonly IExceptionHelper _exceptionHelper;
        protected readonly IFindAndReplaceHelper _findAndReplaceHelper;
        protected readonly IFormInitializer _formInitializer;
        protected readonly IRadDropDownListHelper _radDropDownListHelper;
        protected readonly ISearchFunctions _searchFunctions;

        public FindReplaceConfiguredItemInShapeBase(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFindAndReplaceHelper findAndReplaceHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            ISearchFunctions searchFunctions)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
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
        protected Shape? selectedShape;
        protected int searchPageIndex = 1;
        protected int searchShapeIndex = 1;
        #endregion Variables

        private bool CanReplace => radDropDownListFind.Text.Length > 0
                                    && radDropDownListReplace.Text.Length > 0
                                    && selectedShape != null;

        #region Methods
        public void Setup(Document visioDocument)
        {
            this.visioDocument = visioDocument;
        }

        protected virtual void DoFind()
        {
            throw new NotImplementedException();
        }

        protected virtual void DoReplace()
        {
            throw new NotImplementedException();
        }

        protected virtual void GetSettings()
        {
            radRadioButtonCurrentPage.IsChecked = true;
            /*We'll use the CurrentPage's CheckStateChanged for ResetSearchIndexes() - only one is required*/
            if (Settings.Default.findCurrentRow == false)
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
            radButtonReplace.Enabled = CanReplace;
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 724);

            this.AcceptButton = radButtonFindNext;

            InitializeControls();
            GetSettings();

            radButtonReplace.Enabled = false;
            radButtonClose.DialogResult = DialogResult.Cancel;
            ValidateOk();
        }

        private void Replace()
        {
            if (selectedShape == null)
                throw _exceptionHelper.CriticalException("{0A0553B3-2FD9-4CAC-8552-23212FA14632}");

            DoReplace();
            FindNext();
        }

        private void ResetSearchIndexes()
        {
            selectedShape = null;
            searchPageIndex = 1;
            searchShapeIndex = 1;
            radButtonReplace.Enabled = CanReplace;
            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
        }

        private void ValidateOk()
        {
            radButtonFindNext.Enabled = radDropDownListFind.Text.Length > 0;
            radButtonReplace.Enabled = CanReplace;
        }
        #endregion Methods

        #region Event Handlers
        private void RadDropDownListFind_TextChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
            ValidateOk();
        }

        private void RadDropDownListReplace_TextChanged(object sender, EventArgs e)
        {
            radButtonReplace.Enabled = CanReplace;
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

        private void RadButtonFindNext_Click(object sender, EventArgs e) 
            => FindNext();

        private void RadButtonReplace_Click(object sender, EventArgs e) 
            => Replace();

        private void FindReplaceConfiguredItemInShapeBase_FormClosing(object sender, FormClosingEventArgs e) 
            => SaveSettings();
        #endregion Event Handlers
    }
}
