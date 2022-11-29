using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindConfiguredItemInFilesBase : Telerik.WinControls.UI.RadForm, IFindInFilesForm
    {
        protected readonly IConfigurationService _configurationService;
        protected readonly IExceptionHelper _exceptionHelper;
        protected readonly IFormInitializer _formInitializer;
        protected readonly IRadDropDownListHelper _radDropDownListHelper;

        public FindConfiguredItemInFilesBase(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper)
        {
            _configurationService = configurationService;
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
                    throw _exceptionHelper.CriticalException("{262C7DD9-9494-46BA-BB3F-1F4A4B7C2FDF}");
            }
        }

        public string SearchPattern => radDropDownListFileTypes.Text;

        public string SearchString => radDropDownListText.Text;

        public bool MatchCase => radCheckBoxMatchCase.Checked;

        public bool MatchWholeWord => radCheckBoxMatchWholeWord.Checked;

        protected virtual void GetSettings()
        {
            throw new NotImplementedException();
        }

        protected virtual void InitializeControls()
        {
            radDropDownListFileTypes.MaxLength = COMBOFILETYPESMAXLENGTH;
            _radDropDownListHelper.LoadTextItems(radDropDownListFileTypes, GetFileTypesItemsSource(), RadDropDownStyle.DropDown);
            radDropDownListFileTypes.SelectedIndex = 0;
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

        protected virtual void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 535);

            radRadioButtonAllDocuments.IsChecked = true;

            InitializeControls();
            GetSettings();

            radButtonFindAll.DialogResult = DialogResult.OK;

            ValidateOk();
        }

        protected void ValidateOk()
        {
            radButtonFindAll.Enabled = radDropDownListText.Text.Trim().Length > 0
                && radDropDownListFileTypes.Text.Trim().Length > 0
                && IsPatternValid();
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

        #region Event Handlers
        private void RadDropDownListText_TextChanged(object sender, EventArgs e)
        {
            ValidateOk();
        }

        private void RadDropDownListFileTypes_TextChanged(object sender, EventArgs e)
        {
            ValidateOk();
        }

        private void RadRadioButtonAllDocuments_CheckStateChanged(object sender, EventArgs e)
        {
            if (radRadioButtonOpenDocument.IsChecked)
            {
                radDropDownListFileTypes.SelectedIndex = 0;
            }
        }
        #endregion Event Handlers
    }
}
