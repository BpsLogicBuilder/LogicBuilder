using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindTextInFiles : Telerik.WinControls.UI.RadForm, IFindTextInFiles
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IRadDropDownListHelper _radDropDownListHelper;

        public FindTextInFiles(
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper)
        {
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _radDropDownListHelper = radDropDownListHelper;
            InitializeComponent();
            Initialize();
        }

        private const int COMBOFILETYPESMAXLENGTH = 50;
        private static readonly char[] invalidPatternChars = GetInvalidPatternChars();

        public SearchOptions SearchType
        {
            get
            {
                if (radRadioButtonAllDocuments.IsChecked)
                    return SearchOptions.All;
                else if (radRadioButtonOpenDocument.IsChecked)
                    return SearchOptions.OpenDocuments;
                else
                    throw _exceptionHelper.CriticalException("{696E5847-3335-4BFF-94B3-B6E2B29C10B5}");
            }
        }

        public string SearchPattern => radDropDownListFileTypes.Text;

        public string SearchString => radTextBoxText.Text;

        public bool MatchCase => radCheckBoxMatchCase.Checked;

        public bool MatchWholeWord => radCheckBoxMatchWholeWord.Checked;

        protected virtual void GetSettings()
        {
            radTextBoxText.Text = Settings.Default.textFindWhat;
        }

        protected virtual void InitializeControls()
        {
            this.Text = Strings.findTextInFilesFormText;

            radDropDownListFileTypes.MaxLength = COMBOFILETYPESMAXLENGTH;
            _radDropDownListHelper.LoadTextItems(radDropDownListFileTypes, GetFileTypesItemsSource(), RadDropDownStyle.DropDown);
            radDropDownListFileTypes.SelectedIndex = 0;
        }

        private static char[] GetInvalidPatternChars()
            => new string(Path.GetInvalidFileNameChars())
                .Replace(MiscellaneousConstants.WILDCARDSTRING, string.Empty)
                .Replace(MiscellaneousConstants.QUESTIONMARKSTRING, string.Empty)
                .ToCharArray();

        private static string[] GetFileTypesItemsSource()
        {
            return new string[]
            {
                $"{MiscellaneousConstants.WILDCARDSTRING}{FileExtensions.VISIOFILEEXTENSION}{MiscellaneousConstants.SEMICOLONSTRING}{MiscellaneousConstants.WILDCARDSTRING}{FileExtensions.VSDXFILEEXTENSION}{MiscellaneousConstants.SEMICOLONSTRING}{MiscellaneousConstants.WILDCARDSTRING}{FileExtensions.TABLEFILEEXTENSION}",
                $"{MiscellaneousConstants.WILDCARDSTRING}{FileExtensions.VISIOFILEEXTENSION}",
                $"{MiscellaneousConstants.WILDCARDSTRING}{FileExtensions.VSDXFILEEXTENSION}",
                $"{MiscellaneousConstants.WILDCARDSTRING}{FileExtensions.TABLEFILEEXTENSION}",
            };
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 535);
            
            radRadioButtonAllDocuments.IsChecked = true;

            InitializeControls();
            GetSettings();

            radButtonFindAll.DialogResult = DialogResult.OK;

            ValidateOk();
        }

        private bool IsPatternValid()
        {
            string[] patterns = radDropDownListFileTypes.Text.Split(new char[] { MiscellaneousConstants.SEMICOLONCHAR });
            foreach (string pattern in patterns)
            {
                if (pattern.EndsWith(MiscellaneousConstants.DOUBLEPERIODSTRING) 
                    || pattern.IndexOfAny(invalidPatternChars) != -1)
                {
                    return false;
                }
            }

            return true;
        }

        private void ValidateOk()
        {
            radButtonFindAll.Enabled = radTextBoxText.Text.Trim().Length > 0 
                && radDropDownListFileTypes.Text.Trim().Length > 0 
                && IsPatternValid();
        }

        private void RadTextBoxText_TextChanged(object sender, EventArgs e)
        {
            ValidateOk();
        }

        private void RadDropDownListFileTypes_TextChanged(object sender, EventArgs e)
        {
            ValidateOk();
        }

        private void FindTextInFiles_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.textFindWhat = radTextBoxText.Text;
            Settings.Default.findInAllDocuments = radRadioButtonAllDocuments.IsChecked;
            Settings.Default.Save();
        }

        private void RadRadioButtonAllDocuments_CheckStateChanged(object sender, EventArgs e)
        {
            if (radRadioButtonOpenDocument.IsChecked)
            {
                radDropDownListFileTypes.SelectedIndex = 0;
            }
        }
    }
}
