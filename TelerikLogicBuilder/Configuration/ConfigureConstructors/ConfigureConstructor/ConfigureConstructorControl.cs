using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor.Factories;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors.ConfigureConstructor
{
    internal partial class ConfigureConstructorControl : UserControl, IConfigureConstructorControl
    {
        private readonly IConfigureConstructorControlCommandFactory _configureConstructorControlCommandFactory;
        private readonly IConfigureConstructorsStateImageSetter _configureConstructorsStateImageSetter;
        private readonly IConstructorControlsValidator _constructorControlsValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureConstructorsForm configureConstructorsForm;

        public ConfigureConstructorControl(
            IConfigureConstructorControlCommandFactory configureConstructorControl,
            IConfigureConstructorsStateImageSetter configureConstructorsStateImageSetter,
            IConstructorControlValidatorFactory constructorControlValidatorFactory,
            IExceptionHelper exceptionHelper,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureConstructorsForm configureConstructorsForm)
        {
            InitializeComponent();
            _configureConstructorControlCommandFactory = configureConstructorControl;
            _configureConstructorsStateImageSetter = configureConstructorsStateImageSetter;
            _constructorControlsValidator = constructorControlValidatorFactory.GetConstructorControlsValidator(this);
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureConstructorsForm = configureConstructorsForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public IDictionary<string, Constructor> ConstructorsDictionary => configureConstructorsForm.ConstructorsDictionary;
        public IConfigureConstructorsForm Form => configureConstructorsForm;
        public ConstructorHelperStatus? HelperStatus => configureConstructorsForm.HelperStatus;
        public RadLabel LblConstructorName => lblConstructorName;
        public RadTextBox TxtConstructorName => txtConstructorName;
        public RadTreeView TreeView => configureConstructorsForm.TreeView;
        public XmlDocument XmlDocument => configureConstructorsForm.XmlDocument;

        public void ClearMessage() 
            => configureConstructorsForm.ClearMessage();

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsConstructorNode(treeNode))
                throw _exceptionHelper.CriticalException("{84E5CDDA-B017-44B6-9832-74BD011B9057}");

            RemoveEventHandlers();

            XmlElement constructorElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(constructorElement).ToDictionary(e => e.Name);

            txtConstructorName.Text = constructorElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
            txtConstructorName.Select();
            txtConstructorName.SelectAll();
            txtConstructorTypeName.Text = elements[XmlDataConstants.TYPENAMEELEMENT].InnerText;
            txtConstructorSummary.Text = elements[XmlDataConstants.SUMMARYELEMENT].InnerText;

            AddEventHandlers();
        }

        public void SetErrorMessage(string message) 
            => configureConstructorsForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "")
            => configureConstructorsForm.SetMessage(message, title);

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsConstructorNode(treeNode))
                throw _exceptionHelper.CriticalException("{BC2AF6D4-FA0E-43D0-BC1F-58600B31305B}");

            _constructorControlsValidator.ValidateInputBoxes();

            XmlElement constructorElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(constructorElement).ToDictionary(e => e.Name);
            string newNameAttributeValue = txtConstructorName.Text.Trim();

            elements[XmlDataConstants.SUMMARYELEMENT].InnerText = txtConstructorSummary.Text.Trim();
            constructorElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;

            configureConstructorsForm.ValidateXmlDocument();

            if (treeNode.Expanded && configureConstructorsForm.ExpandedNodes.ContainsKey(treeNode.Name))
                configureConstructorsForm.ExpandedNodes.Remove(treeNode.Name);

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.CONSTRUCTORELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;

            if (treeNode.Expanded)
                configureConstructorsForm.ExpandedNodes.Add(treeNode.Name, newNameAttributeValue);

            _configureConstructorsStateImageSetter.SetImage(constructorElement, (StateImageRadTreeNode)treeNode, configureConstructorsForm.Application);
            configureConstructorsForm.RenameChildNodes(treeNode);
        }

        public void ValidateFields()
        {
            _constructorControlsValidator.ValidateInputBoxes();
        }

        public void ValidateXmlDocument()
        {
            configureConstructorsForm.ValidateXmlDocument();
        }

        private void AddEventHandlers()
        {
            txtConstructorName.TextChanged += TxtConstructorName_TextChanged;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            radPanelConstructor.VerticalScrollBarState = ScrollState.AlwaysShow;
            InitializeTableLayoutPanel();
            CollapsePanelBorder(radPanelConstructor);
            CollapsePanelBorder(radPanelTableParent);
            InitializeConstructorControls();
            InitializeClickCommands();
        }

        private void InitializeClickCommands()
        {
            InitializeHelperButtonCommand
            (
                txtConstructorTypeName,
                _configureConstructorControlCommandFactory.GetEditConstructorTypeNameCommand(this)
            );

            InitializeHelperButtonCommand
            (
                txtConstructorGenericArguments,
                _configureConstructorControlCommandFactory.GetEditGenericArgumentsCommand(this)
            );
        }

        private static void InitializeHelperButtonCommand(HelperButtonTextBox helperButtonTextBox, IClickCommand command)
        {
            helperButtonTextBox.ButtonClick += (sender, args) => command.Execute();
        }

        private void InitializeConstructorControls()
        {
            InitializeReadOnlyTextBox(txtConstructorTypeName, string.Empty);
            InitializeReadOnlyTextBox(txtConstructorGenericArguments, Strings.configurationFormIndicatorText);
            helpProvider.SetHelpString(this.txtConstructorName, Strings.constrConfigConstructorNameHelp1);
            helpProvider.SetHelpString(this.txtConstructorTypeName, Strings.constrConfigConstructorTypeNameHelp);
            helpProvider.SetHelpString(this.txtConstructorGenericArguments, Strings.constrConfigGenericArgumentsHelp);
            helpProvider.SetHelpString(this.txtConstructorSummary, Strings.constrConfigSummaryHelp);
            toolTip.SetToolTip(this.lblConstructorName, Strings.constrConfigConstructorNameHelp1);
            toolTip.SetToolTip(this.lblConstructorTypeName, Strings.constrConfigConstructorTypeNameHelp);
            toolTip.SetToolTip(this.lblConstructorGenericArguments, Strings.constrConfigGenericArgumentsHelp);
            toolTip.SetToolTip(this.lblConstructorSummary, Strings.constrConfigSummaryHelp);
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
            float size_20 = 20F / 184 * 100;
            float size_30 = 30F / 184 * 100;
            float size_6 = 6F / 184 * 100;

            ((ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();

            tableLayoutPanel.RowStyles[0] = new RowStyle(SizeType.Percent, size_20);
            tableLayoutPanel.RowStyles[1] = new RowStyle(SizeType.Percent, size_30);
            tableLayoutPanel.RowStyles[2] = new RowStyle(SizeType.Percent, size_6);
            tableLayoutPanel.RowStyles[3] = new RowStyle(SizeType.Percent, size_30);
            tableLayoutPanel.RowStyles[4] = new RowStyle(SizeType.Percent, size_6);
            tableLayoutPanel.RowStyles[5] = new RowStyle(SizeType.Percent, size_30);
            tableLayoutPanel.RowStyles[6] = new RowStyle(SizeType.Percent, size_6);
            tableLayoutPanel.RowStyles[7] = new RowStyle(SizeType.Percent, size_30);
            tableLayoutPanel.RowStyles[8] = new RowStyle(SizeType.Percent, size_6);
            tableLayoutPanel.RowStyles[9] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(true);
        }

        private void RemoveEventHandlers()
        {
            txtConstructorName.TextChanged -= TxtConstructorName_TextChanged;
        }

        private void TxtConstructorName_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                UpdateXmlDocument(TreeView.SelectedNode);
            }
            catch (XmlException ex)
            {
                SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                SetErrorMessage(ex.Message);
            }
        }
    }
}
