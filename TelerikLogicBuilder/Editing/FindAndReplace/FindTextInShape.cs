using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Office.Interop.Visio;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindTextInShape : Telerik.WinControls.UI.RadForm
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFindAndReplaceHelper _findAndReplaceHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly ISearchFunctions _searchFunctions;

        public FindTextInShape(
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
        private Document? visioDocument;
        private int searchPageIndex = 1;
        private int searchShapeIndex = 1;
        #endregion Variables

        #region Methods
        public void Setup(Document visioDocument)
        {
            this.visioDocument = visioDocument;
        }

        private void FindNext()
        {
            if (this.visioDocument == null)
                throw _exceptionHelper.CriticalException("{A1A33756-BC93-4DFA-9A64-A0D729097BB7}");

            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
            if (radRadioButtonCurrentPage.IsChecked)
                _findAndReplaceHelper.FindItemCurrentPage(this.visioDocument, radListOccurrences, radGroupBoxOccurrences, ref searchShapeIndex, radTextBoxText.Text, radCheckBoxMatchCase.Checked, radCheckBoxMatchWholeWord.Checked, _searchFunctions.FindTextMatches);
            else
                _findAndReplaceHelper.FindItemAllPages(this.visioDocument, radListOccurrences, radGroupBoxOccurrences, ref searchPageIndex, ref searchShapeIndex, radTextBoxText.Text, radCheckBoxMatchCase.Checked, radCheckBoxMatchWholeWord.Checked, _searchFunctions.FindTextMatches);
        }

        private void Initialize()
        {
            radTextBoxText.Anchor = AnchorConstants.AnchorsLeftTopRight;
            _formInitializer.SetFormDefaults(this, 624);

            this.AcceptButton = radButtonFindNext;

            radRadioButtonCurrentPage.IsChecked = true;
            /*We'll use the CurrentPages CheckStateChanged for ResetSearchIndexes() - only one is required*/
            if (Settings.Default.findCurrentPage == false)
                radRadioButtonAllPages.IsChecked = true;

            radCheckBoxMatchCase.Checked = Settings.Default.findTextMatchCase;
            radCheckBoxMatchWholeWord.Checked = Settings.Default.findTextMatchWholeWord;
            radTextBoxText.Text = Settings.Default.textFindWhat;

            radButtonCancel.DialogResult = DialogResult.Cancel;

            ValidateOk();
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

        private void RadRadioButtonCurrentPage_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void RadRadioButtonAllPages_CheckStateChanged(object sender, EventArgs e)
        {
            //ResetSearchIndexes();
        }

        private void FindTextInShape_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.findCurrentPage = radRadioButtonCurrentPage.IsChecked;
            Settings.Default.findTextMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findTextMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.textFindWhat = radTextBoxText.Text;
            Settings.Default.Save();
        }
        #endregion Event Handlers
    }
}
