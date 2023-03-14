﻿using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties
{
    internal partial class ApplicationControl : UserControl, IApplicationControl
    {
        private readonly IApplicationControlCommandFactory _applicationControlCommandFactory;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureProjectPropertiesForm configureProjectProperties;

        public ApplicationControl(
            IApplicationControlCommandFactory applicationControlCommandFactory,
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureProjectPropertiesForm configureProjectProperties)
        {
            _applicationControlCommandFactory = applicationControlCommandFactory;
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _radDropDownListHelper = radDropDownListHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureProjectProperties = configureProjectProperties;

            InitializeComponent();
            Initialize();
        }

        #region Properties
        public RadTreeView TreeView => this.configureProjectProperties.TreeView;
        public XmlDocument XmlDocument => this.configureProjectProperties.XmlDocument;
        public HelperButtonTextBox TxtActivityAssembly => this.txtActivityAssembly;
        public HelperButtonTextBox TxtActivityAssemblyPath => this.txtActivityAssemblyPath;
        public HelperButtonTextBox TxtResourceFilesDeployment => this.txtResourceFilesDeployment;
        public HelperButtonTextBox TxtRulesDeployment => this.txtRulesDeployment;
        #endregion Properties

        #region Methods
        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsApplicationNode(treeNode))
                throw new ArgumentException($"{nameof(treeNode)}: {{C0E87DC3-E9B3-4F3E-BCD9-4D738107955C}}");

            XmlElement applicationElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> appElements = _xmlDocumentHelpers.GetChildElements(applicationElement).ToDictionary(e => e.Name);
            RemoveEventHandlers();
            txtName.Text = applicationElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtNickname.Text = applicationElement.GetAttribute(XmlDataConstants.NICKNAMEATTRIBUTE);
            txtActivityAssembly.Text = appElements[XmlDataConstants.ACTIVITYASSEMBLYELEMENT].InnerText;
            txtActivityAssemblyPath.Text = appElements[XmlDataConstants.ACTIVITYASSEMBLYPATHELEMENT].InnerText;
            cmbRuntime.SelectedValue = _enumHelper.ParseEnumText<RuntimeType>(appElements[XmlDataConstants.RUNTIMEELEMENT].InnerText);
            txtActivityClass.Text = appElements[XmlDataConstants.ACTIVITYCLASSELEMENT].InnerText;
            txtResourceFilesDeployment.Text = appElements[XmlDataConstants.RESOURCEFILEDEPLOYMENTPATHELEMENT].InnerText;
            txtRulesDeployment.Text = appElements[XmlDataConstants.RULESDEPLOYMENTPATHELEMENT].InnerText;
            AddEventHandlers();
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsApplicationNode(treeNode))
                throw _exceptionHelper.CriticalException("{73CAC96C-CCBD-44AB-9B7A-B6C368AA626D}");

            List<string> errors = ValidateFields();
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));

            XmlElement applicationElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> appElements = _xmlDocumentHelpers.GetChildElements(applicationElement).ToDictionary(e => e.Name);

            applicationElement.Attributes[XmlDataConstants.NICKNAMEATTRIBUTE]!.Value = txtNickname.Text.Trim();
            appElements[XmlDataConstants.ACTIVITYASSEMBLYELEMENT].InnerText = txtActivityAssembly.Text.Trim();
            appElements[XmlDataConstants.ACTIVITYASSEMBLYPATHELEMENT].InnerText = txtActivityAssemblyPath.Text.Trim();
            appElements[XmlDataConstants.RUNTIMEELEMENT].InnerText = Enum.GetName(typeof(RuntimeType), cmbRuntime.SelectedValue) ?? throw new ArgumentException($"{nameof(cmbRuntime)}: {{25BFBCAF-5718-44D2-8FF3-665BF0AC5442}}");
            appElements[XmlDataConstants.ACTIVITYCLASSELEMENT].InnerText = txtActivityClass.Text.Trim();
            appElements[XmlDataConstants.RESOURCEFILEDEPLOYMENTPATHELEMENT].InnerText = txtResourceFilesDeployment.Text.Trim();
            appElements[XmlDataConstants.RULESDEPLOYMENTPATHELEMENT].InnerText = txtRulesDeployment.Text.Trim();

            this.configureProjectProperties.ValidateXmlDocument();
        }

        private void AddEventHandlers()
        {
            txtNickname.Validating += TxtNickname_Validating;
            txtActivityAssembly.Validating += TxtActivityAssembly_Validating;
            txtActivityAssemblyPath.Validating += TxtActivityAssemblyPath_Validating;
            txtActivityClass.Validating += TxtActivityClass_Validating;
            txtResourceFilesDeployment.Validating += TxtResourceFileDeployment_Validating;
            txtRulesDeployment.Validating += TxtRulesDeployment_Validating;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            radPanelApplication.VerticalScrollBarState = ScrollState.AlwaysShow;
            InitializeTableLayoutPanel();
            txtName.ReadOnly = true;
            CollapsePanelBorder(radPanelApplication);
            CollapsePanelBorder(radPanelTableParent);

            InitializeReadOnlyTextBox(txtLoadAssemblyPaths, Strings.collectionIndicatorText);
            InitializeReadOnlyTextBox(txtExcludedModules, Strings.collectionIndicatorText);
            InitializeReadOnlyTextBox(txtWebApiDeployment, Strings.configurationFormIndicatorText);

            LoadRuntimeComboBox();
            InitializeClickCommands();
        }

        private void InitializeClickCommands()
        {
            InitializeHelperButtonCommand
            (
                txtActivityAssembly,
                _applicationControlCommandFactory.GetEditActivityAssemblyCommand(this)
            );
            InitializeHelperButtonCommand
            (
                txtActivityAssemblyPath,
                _applicationControlCommandFactory.GetEditActivityAssemblyPathCommand(this)
            );
            InitializeHelperButtonCommand
            (
                txtExcludedModules,
                _applicationControlCommandFactory.GetEditExcludedModulesCommand(this)
            );
            InitializeHelperButtonCommand
            (
                txtLoadAssemblyPaths,
                _applicationControlCommandFactory.GetEditLoadAssemblyPathsCommand(this)
            );
            InitializeHelperButtonCommand
            (
                txtResourceFilesDeployment,
                _applicationControlCommandFactory.GetEditResourceFilesDeploymentCommand(this)
            );
            InitializeHelperButtonCommand
            (
                txtRulesDeployment,
                _applicationControlCommandFactory.GetEditRulesDeploymentCommand(this)
            );
            InitializeHelperButtonCommand
            (
                txtWebApiDeployment,
                _applicationControlCommandFactory.GetEditWebApiDeploymentCommand(this)
            );
        }

        private static void InitializeHelperButtonCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }

        private static void InitializeReadOnlyTextBox(HelperButtonTextBox helperButtonTextBox, string text)
        {
            helperButtonTextBox.Font = new Font
            (
                ForeColorUtility.GetDefaultFont(ThemeResolutionService.ApplicationThemeName),
                FontStyle.Bold
            );
            helperButtonTextBox.ReadOnly = true;
            helperButtonTextBox.Text = text;
            helperButtonTextBox.SetPaddingType(HelperButtonTextBox.PaddingType.Bold);
        }

        private void InitializeTableLayoutPanel()
        {
            float size_20 = 20F / 436 * 100;
            float size_30 = 30F / 436 * 100;
            float size_6 = 6F / 436 * 100;
            ((ISupportInitialize)this.radPanelTableParent).BeginInit();
            this.radPanelTableParent.SuspendLayout();

            this.tableLayoutPanel.RowStyles[0] = new RowStyle(SizeType.Percent, size_20);
            this.tableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[4] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[5] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[6] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[7] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[8] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[9] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[10] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[11] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[12] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[13] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[14] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[15] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[16] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[17] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[18] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[19] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[20] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[21] = new RowStyle(SizeType.Percent, size_30);
            this.tableLayoutPanel.RowStyles[22] = new RowStyle(SizeType.Percent, size_6);
            this.tableLayoutPanel.RowStyles[23] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            this.radPanelTableParent.ResumeLayout(true);
        }

        private void LoadRuntimeComboBox()
        {
            _radDropDownListHelper.LoadComboItems<RuntimeType>(cmbRuntime);
        }

        private void RemoveEventHandlers()
        {
            txtNickname.Validating -= TxtNickname_Validating;
            txtActivityAssembly.Validating -= TxtActivityAssembly_Validating;
            txtActivityAssemblyPath.Validating -= TxtActivityAssemblyPath_Validating;
            txtActivityClass.Validating -= TxtActivityClass_Validating;
            txtResourceFilesDeployment.Validating -= TxtResourceFileDeployment_Validating;
            txtRulesDeployment.Validating -= TxtRulesDeployment_Validating;
        }

        private void UpdateErrorControl(IList<string> errors, CancelEventArgs e)
        {
            if (errors.Count > 0)
            {
                configureProjectProperties.SetErrorMessage(string.Join(Environment.NewLine, errors));
                e.Cancel = true;
            }
            else
            {
                configureProjectProperties.ClearMessage();
            }
        }

        private void ValidateActivityAssembly(List<string> errors)
        {
            if (!Regex.IsMatch(txtActivityAssembly.Text, RegularExpressions.FILENAME))
            {
                this.ActiveControl = txtActivityAssembly;
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidFileNameMessageFormat, lblActivityAssembly.Text));
            }
        }

        private void ValidateActivityAssemblyPath(List<string> errors)
        {
            if (!Regex.IsMatch(txtActivityAssemblyPath.Text, RegularExpressions.FILEPATH))
            {
                this.ActiveControl = txtActivityAssemblyPath;
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidFilePathMessageFormat, lblActivityAssemblyPath.Text));
            }
        }

        private void ValidateActivityClass(List<string> errors)
        {
            if (!Regex.IsMatch(txtActivityClass.Text, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
            {
                this.ActiveControl = txtActivityClass;
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, lblActivityClass.Text));
            }
        }

        private List<string> ValidateFields()
        {
            List<string> errors = new();
            ValidateNickname(errors);
            ValidateActivityAssembly(errors);
            ValidateActivityAssemblyPath(errors);
            ValidateActivityClass(errors);
            ValidateResourceFileDeployment(errors);
            ValidateRulesDeployment(errors);
            return errors;
        }

        private void ValidateNickname(List<string> errors)
        {
            if (!Regex.IsMatch(txtNickname.Text, RegularExpressions.XMLNAMEATTRIBUTE))
            {
                this.ActiveControl = txtNickname;
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidTxtNameTextFormat, lblNickname.Text));
            }
        }

        private void ValidateResourceFileDeployment(List<string> errors)
        {
            if (!Regex.IsMatch(txtResourceFilesDeployment.Text, RegularExpressions.FILEPATH))
            {
                this.ActiveControl = txtResourceFilesDeployment;
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidFilePathMessageFormat, lblResourceFileDeployment.Text));
            }
        }

        private void ValidateRulesDeployment(List<string> errors)
        {
            if (!Regex.IsMatch(txtRulesDeployment.Text, RegularExpressions.FILEPATH))
            {
                this.ActiveControl = txtRulesDeployment;
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidFilePathMessageFormat, lblRulesDeployment.Text));
            }
        }
        #endregion Methods

        #region Event Handlers
        private void TxtActivityAssembly_Validating(object? sender, CancelEventArgs e)
        {
            List<string> errors = new();
            ValidateActivityAssembly(errors);
            UpdateErrorControl(errors, e);
        }

        private void TxtActivityAssemblyPath_Validating(object? sender, CancelEventArgs e)
        {
            List<string> errors = new();
            ValidateActivityAssemblyPath(errors);
            UpdateErrorControl(errors, e);
        }

        private void TxtActivityClass_Validating(object? sender, CancelEventArgs e)
        {
            List<string> errors = new();
            ValidateActivityClass(errors);
            UpdateErrorControl(errors, e);
        }

        private void TxtNickname_Validating(object? sender, CancelEventArgs e)
        {
            List<string> errors = new();
            ValidateNickname(errors);
            UpdateErrorControl(errors, e);
        }

        private void TxtResourceFileDeployment_Validating(object? sender, CancelEventArgs e)
        {
            List<string> errors = new();
            ValidateResourceFileDeployment(errors);
            UpdateErrorControl(errors, e);
        }

        private void TxtRulesDeployment_Validating(object? sender, CancelEventArgs e)
        {
            List<string> errors = new();
            ValidateRulesDeployment(errors);
            UpdateErrorControl(errors, e);
        }
        #endregion Event Handlers
    }
}
