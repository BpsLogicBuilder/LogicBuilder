using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindTextInCell : RadForm
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFindAndReplaceHelper _findAndReplaceHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly ISearchFunctions _searchFunctions;

        public FindTextInCell(
            IExceptionHelper exceptionHelper,
            IFindAndReplaceHelper findAndReplaceHelper,
            IFormInitializer formInitializer,
            ISearchFunctions searchFunctions)
        {
            _exceptionHelper = exceptionHelper;
            _findAndReplaceHelper = findAndReplaceHelper;
            _formInitializer = formInitializer;
            _searchFunctions = searchFunctions;
            InitializeComponent();
            Initialize();
        }

        #region Variables
        private RadGridView? dataGridView;
        private int searchRowIndex;
        private int searchCellIndex;
        #endregion Variables

        #region Methods
        public void Setup(RadGridView dataGridView)
        {
            this.dataGridView = dataGridView;
        }

        private void FindNext()
        {
            if (this.dataGridView == null)
                throw _exceptionHelper.CriticalException("{8403FA68-0C9D-4191-AF56-BF40A60B7572}");

            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
            if (radRadioButtonCurrentRow.IsChecked)
                _findAndReplaceHelper.FindItemCurrentRow(this.dataGridView, radListOccurrences, radGroupBoxOccurrences, ref searchCellIndex, radTextBoxText.Text, radCheckBoxMatchCase.Checked, radCheckBoxMatchWholeWord.Checked, _searchFunctions.FindTextMatches);
            else
                _findAndReplaceHelper.FindItemAllRows(this.dataGridView, radListOccurrences, radGroupBoxOccurrences, ref searchRowIndex, ref searchCellIndex, radTextBoxText.Text, radCheckBoxMatchCase.Checked, radCheckBoxMatchWholeWord.Checked, _searchFunctions.FindTextMatches);
        }

        private void Initialize()
        {
            radTextBoxText.Anchor = AnchorConstants.AnchorsLeftTopRight;
            _formInitializer.SetFormDefaults(this, 624);

            this.AcceptButton = radButtonFindNext;

            if (Settings.Default.findCurrentRow)
                radRadioButtonCurrentRow.IsChecked = true;
            else
                radRadioButtonAllRows.IsChecked = true;

            radCheckBoxMatchCase.Checked = Settings.Default.findTextMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findTextMatchWholeWord;
            radTextBoxText.Text = Settings.Default.textFindWhat;

            radButtonCancel.DialogResult = DialogResult.Cancel;

            ValidateOk();
        }

        private void ResetSearchIndexes()
        {
            searchRowIndex = 0;
            searchCellIndex = 0;
            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
        }

        private void ValidateOk()
        {
            radButtonFindNext.Enabled = radTextBoxText.Text.Length > 0;
        }
        #endregion Methods

        #region Event Handlers
        private void RadTextBoxText_TextChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
            ValidateOk();
        }

        private void RadButtonFindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void RadCheckBoxMatchCase_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void RadCheckBoxMatchWholeWord_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void RadRadioButtonCurrentRow_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void RadRadioButtonAllRows_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void FindTextInCell_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.findCurrentRow = radRadioButtonCurrentRow.IsChecked;
            Settings.Default.findTextMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findTextMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.textFindWhat = radTextBoxText.Text;
            Settings.Default.Save();
        }
        #endregion Event Handlers
    }
}
