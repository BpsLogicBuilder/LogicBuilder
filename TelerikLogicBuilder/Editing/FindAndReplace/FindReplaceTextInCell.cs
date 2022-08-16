using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindReplaceTextInCell : RadForm
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFindAndReplaceHelper _findAndReplaceHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly ISearchFunctions _searchFunctions;

        public FindReplaceTextInCell(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFindAndReplaceHelper findAndReplaceHelper,
            IFormInitializer formInitializer,
            ISearchFunctions searchFunctions)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _findAndReplaceHelper = findAndReplaceHelper;
            _formInitializer = formInitializer;
            _searchFunctions = searchFunctions;
            InitializeComponent();
            Initialize();
        }

        #region Variables
        private RadGridView? dataGridView;
        private DataSet? dataSet;
        private GridViewCellInfo? dataGridViewCell;
        private int searchRowIndex;
        private int searchCellIndex;
        #endregion Variables

        private bool CanReplace => radTextBoxFind.Text.Length > 0 
                                    && radTextBoxReplace.Text.Length > 0 
                                    && dataGridViewCell != null;

        public void Setup(RadGridView dataGridView, DataSet dataSet)
        {
            this.dataGridView = dataGridView;
            this.dataSet = dataSet;
        }

        private void FindNext()
        {
            if (this.dataGridView == null)
                throw _exceptionHelper.CriticalException("{E098B7A8-B4F0-4B02-A826-39167C9BE828}");

            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
            if (radRadioButtonCurrentRow.IsChecked)
                dataGridViewCell = _findAndReplaceHelper.FindItemCurrentRow(this.dataGridView, radListOccurrences, radGroupBoxOccurrences, ref searchCellIndex, radTextBoxFind.Text, radCheckBoxMatchCase.Checked, radCheckBoxMatchWholeWord.Checked, _searchFunctions.FindTextMatches);
            else
                dataGridViewCell = _findAndReplaceHelper.FindItemAllRows(this.dataGridView, radListOccurrences, radGroupBoxOccurrences, ref searchRowIndex, ref searchCellIndex, radTextBoxFind.Text, radCheckBoxMatchCase.Checked, radCheckBoxMatchWholeWord.Checked, _searchFunctions.FindTextMatches);

            radButtonReplace.Enabled = CanReplace;
        }

        private void Initialize()
        {
            radTextBoxFind.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radTextBoxReplace.Anchor = AnchorConstants.AnchorsLeftTopRight;
            _formInitializer.SetFormDefaults(this, 724);

            this.AcceptButton = radButtonFindNext;

            radRadioButtonCurrentRow.IsChecked = true;
            /*We'll use the CurrentRows CheckStateChanged for ResetSearchIndexes() - only one is required*/
            if (Settings.Default.findCurrentRow == false)
                radRadioButtonAllRows.IsChecked = true;

            radCheckBoxMatchCase.Checked = Settings.Default.findTextMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findTextMatchWholeWord;
            radTextBoxFind.Text = Settings.Default.textFindWhat;
            radTextBoxReplace.Text = Settings.Default.textReplaceWith;

            radButtonReplace.Enabled = false;
            radButtonClose.DialogResult = DialogResult.Cancel;
            ValidateOk();
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
            radButtonFindNext.Enabled = radTextBoxFind.Text.Length > 0;
            radButtonReplace.Enabled = CanReplace;
        }

        #region Event Handlers
        private void RadTextBoxFind_TextChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
            ValidateOk();
        }

        private void RadTextBoxReplace_TextChanged(object sender, EventArgs e)
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
        {
            FindNext();
        }

        private void RadButtonReplace_Click(object sender, EventArgs e)
        {
            if (this.dataGridViewCell == null)
                throw _exceptionHelper.CriticalException("{30B46B95-289F-4334-AFF2-974A2BD8B05F}");

            if (this.dataSet == null)
                throw _exceptionHelper.CriticalException("{37352948-8EB1-41D3-8F2C-1EDF95953F31}");

            _findAndReplaceHelper.ReplaceCellItem
            (
                this.dataGridViewCell,
                this.dataSet,
                radTextBoxFind.Text,
                radTextBoxReplace.Text,
                radCheckBoxMatchCase.Checked,
                radCheckBoxMatchWholeWord.Checked,
                _searchFunctions.ReplaceTextMatches,
                _applicationTypeInfoManager.GetApplicationTypeInfo
                (
                    _configurationService.GetSelectedApplication().Name
                )
            );

            FindNext();
        }

        private void FindReplaceTextInCell_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.findCurrentPage = radRadioButtonCurrentRow.IsChecked;
            Settings.Default.findTextMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findTextMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.textFindWhat = radTextBoxFind.Text;
            Settings.Default.textReplaceWith = radTextBoxReplace.Text;
            Settings.Default.Save();
        }
        #endregion Event Handlers
    }
}
