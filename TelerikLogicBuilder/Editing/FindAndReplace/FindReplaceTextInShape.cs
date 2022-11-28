using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindReplaceTextInShape : Telerik.WinControls.UI.RadForm, IFindReplaceTextInShape
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFindAndReplaceHelper _findAndReplaceHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly ISearchFunctions _searchFunctions;

        public FindReplaceTextInShape(
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
        private Document? visioDocument;
        private Shape? selectedShape;
        private int searchPageIndex = 1;
        private int searchShapeIndex = 1;
        #endregion Variables

        private bool CanReplace => radTextBoxFind.Text.Length > 0
                                    && radTextBoxReplace.Text.Length > 0
                                    && selectedShape != null;

        public void Setup(Document visioDocument)
        {
            this.visioDocument = visioDocument;
        }

        private void FindNext()
        {
            if (this.visioDocument == null)
                throw _exceptionHelper.CriticalException("{1FF0881F-8947-4A57-9600-9B681706A55C}");

            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
            if (radRadioButtonCurrentPage.IsChecked)
                selectedShape = _findAndReplaceHelper.FindItemCurrentPage(this.visioDocument, radListOccurrences, radGroupBoxOccurrences, ref searchShapeIndex, radTextBoxFind.Text, radCheckBoxMatchCase.Checked, radCheckBoxMatchWholeWord.Checked, _searchFunctions.FindTextMatches);
            else
                selectedShape = _findAndReplaceHelper.FindItemAllPages(this.visioDocument, radListOccurrences, radGroupBoxOccurrences, ref searchPageIndex, ref searchShapeIndex, radTextBoxFind.Text, radCheckBoxMatchCase.Checked, radCheckBoxMatchWholeWord.Checked, _searchFunctions.FindTextMatches);

            radButtonReplace.Enabled = CanReplace;
        }

        private void Initialize()
        {
            radTextBoxFind.Anchor = AnchorConstants.AnchorsLeftTopRight;
            radTextBoxReplace.Anchor = AnchorConstants.AnchorsLeftTopRight;
            _formInitializer.SetFormDefaults(this, 724);

            this.AcceptButton = radButtonFindNext;

            this.Text = Strings.replaceTextFormText;

            radRadioButtonCurrentPage.IsChecked = true;
            /*We'll use the CurrentPages CheckStateChanged for ResetSearchIndexes() - only one is required*/
            if (Settings.Default.findCurrentPage == false)
                radRadioButtonAllPages.IsChecked = true;

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
            selectedShape = null;
            searchPageIndex = 1;
            searchShapeIndex = 1;
            radButtonReplace.Enabled = CanReplace;
            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
        }

        private void ValidateOk()
        {
            radButtonFindNext.Enabled = radTextBoxFind.Text.Length > 0;
            radButtonReplace.Enabled = CanReplace;
        }

        private void RadTextBoxFind_TextChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
            ValidateOk();
        }

        private void RadTextBoxReplace_TextChanged(object sender, EventArgs e)
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
        {
            FindNext();
        }

        private void RadButtonReplace_Click(object sender, EventArgs e)
        {
            if (this.selectedShape == null)
                throw _exceptionHelper.CriticalException("{4C6F561C-345A-4351-99E8-F993E2CA097D}");

            _findAndReplaceHelper.ReplaceShapeItem
           (
               this.selectedShape,
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

        private void FindReplaceTextInShape_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.findCurrentPage = radRadioButtonCurrentPage.IsChecked;
            Settings.Default.findTextMatchCase = radCheckBoxMatchCase.Checked;
            Settings.Default.findTextMatchWholeWord = radCheckBoxMatchWholeWord.Checked;
            Settings.Default.textFindWhat = radTextBoxFind.Text;
            Settings.Default.textReplaceWith = radTextBoxReplace.Text;
            Settings.Default.Save();
        }
    }
}
