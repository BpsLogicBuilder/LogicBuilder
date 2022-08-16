using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindReplaceConfiguredItemInCellBase : RadForm
    {
        protected readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        protected readonly IConfigurationService _configurationService;
        protected readonly IExceptionHelper _exceptionHelper;
        protected readonly IFindAndReplaceHelper _findAndReplaceHelper;
        protected readonly IFormInitializer _formInitializer;
        protected readonly IRadDropDownListHelper _radDropDownListHelper;
        protected readonly ISearchFunctions _searchFunctions;

        public FindReplaceConfiguredItemInCellBase(
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
        protected RadGridView? dataGridView;
        protected DataSet? dataSet;
        protected GridViewCellInfo? dataGridViewCell;
        protected int searchRowIndex;
        protected int searchCellIndex;
        #endregion Variables

        private bool CanReplace => radDropDownListFind.Text.Length > 0
                                    && radDropDownListReplace.Text.Length > 0
                                    && dataGridViewCell != null;

        #region Methods
        public void Setup(RadGridView dataGridView, DataSet dataSet)
        {
            this.dataGridView = dataGridView;
            this.dataSet = dataSet;
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
            radRadioButtonCurrentRow.IsChecked = true;
            /*We'll use the CurrentRow's CheckStateChanged for ResetSearchIndexes() - only one is required*/
            if (Settings.Default.findCurrentRow == false)
                radRadioButtonAllRows.IsChecked = true;
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
            radDropDownListFind.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radDropDownListReplace.Anchor = AnchorConstants.AnchorsLeftTopRight;
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
            if (dataGridViewCell == null)
                throw _exceptionHelper.CriticalException("{4CC3EEF5-E6B8-418C-BE7F-48255320F5F7}");

            DoReplace();
            FindNext();
        }

        private void ResetSearchIndexes()
        {
            dataGridViewCell = null;
            searchRowIndex = 0;
            searchCellIndex = 0;
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

        private void RadRadioButtonCurrentRow_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void RadRadioButtonAllRows_CheckStateChanged(object sender, EventArgs e)
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

        private void FindReplaceConfiguredItemInCellBase_FormClosing(object sender, FormClosingEventArgs e) 
            => SaveSettings();
        #endregion Event Handlers
    }
}
