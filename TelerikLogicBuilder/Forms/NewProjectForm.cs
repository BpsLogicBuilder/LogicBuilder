using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Forms.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    internal partial class NewProjectForm : Telerik.WinControls.UI.RadForm, INewProjectForm
    {
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IFormInitializer _formInitializer;
        private readonly INewProjectFormCommandFactory _newProjectFormCommandFactory;
        
        public NewProjectForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IFormInitializer formInitializer,
            INewProjectFormCommandFactory newProjectFormCommandFactory)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;
            _formInitializer = formInitializer;
            _newProjectFormCommandFactory = newProjectFormCommandFactory;
            Initialize();
        }

        public string ProjectName => txtProjectName.Text.Trim();

        public string ProjectPath => txtProjectPath.Text.Trim();

        public HelperButtonTextBox TxtProjectPath => txtProjectPath;

        private static void AddClickCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeTableLayoutPanel();

            FormClosing += NewProjectForm_FormClosing;

            Padding groupBoxPadding = PerFontSizeConstants.GroupBoxPadding;
            _formInitializer.SetFormDefaults
            (
                this,
                this.Size.Height - this.ClientSize.Height
                    + groupBoxPadding.Top
                    + groupBoxPadding.Bottom
                    + (int)(2 * PerFontSizeConstants.BoundarySize)
                    + (int)(2 * PerFontSizeConstants.SeparatorLineHeight)
                    + (int)(2 * PerFontSizeConstants.SingleLineHeight)
                    + (int)PerFontSizeConstants.BottomPanelHeight
            );

            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;

            ControlsLayoutUtility.CollapsePanelBorder(radPanelTableParent);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelNewProject);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelBottom);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelButtons);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelMessages);

            AddClickCommand(TxtProjectPath, _newProjectFormCommandFactory.GetSelectFolderCommand(this));
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeTableLayoutPanel()
        {
            ControlsLayoutUtility.LayoutControls
            (
                radGroupBoxNewProject,
                radPanelNewProject,
                radPanelTableParent,
                tableLayoutPanel,
                2
            );
        }

        private void ValidateFields()
        {
            List<string> errors = new();
            errors.AddRange(ValidateProjectName());
            errors.AddRange(ValidateProjectPath());
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));  
        }

        private IList<string> ValidateProjectName()
        {
            if (!FileNameRegex().IsMatch(txtProjectName.Text))
                return new string[] { string.Format(CultureInfo.CurrentCulture, Strings.invalidFileNameMessageFormat, lblProjectName.Text) };

            return Array.Empty<string>();
        }

        private IList<string> ValidateProjectPath()
        {
            if (!FilePathRegex().IsMatch(txtProjectPath.Text))
                return new string[] { string.Format(CultureInfo.CurrentCulture, Strings.invalidFilePathMessageFormat, lblFolder.Text) };

            return Array.Empty<string>();
        }

        #region Event Handlers
        private void NewProjectForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                if (DialogResult == DialogResult.OK)
                {
                    ValidateFields();
                }
            }
            catch (LogicBuilderException ex)
            {
                _dialogFormMessageControl.SetErrorMessage(ex.Message);
                e.Cancel = true;
            }
        }

        [GeneratedRegex(RegularExpressions.FILENAME)]
        private static partial Regex FileNameRegex();
        [GeneratedRegex(RegularExpressions.FILEPATH)]
        private static partial Regex FilePathRegex();
        #endregion Event Handlers
    }
}
