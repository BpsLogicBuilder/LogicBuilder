using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
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

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragment
{
    internal partial class ConfigureFragmentControl : UserControl, IConfigureFragmentControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly RichTextBoxPanel _richTextBoxPanelDescription;
        private readonly RichTextBoxPanel _richTextBoxPanelFragment;
        private readonly ITreeViewService _treeViewService;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureFragmentsForm configureFragmentsForm;

        public ConfigureFragmentControl(
            IExceptionHelper exceptionHelper,
            RichTextBoxPanel richTextBoxPanelDescription,
            RichTextBoxPanel richTextBoxPanelFragment,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureFragmentsForm configureFragmentsForm)
        {
            InitializeComponent();
            _exceptionHelper = exceptionHelper;
            _richTextBoxPanelDescription = richTextBoxPanelDescription;
            _richTextBoxPanelFragment = richTextBoxPanelFragment;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureFragmentsForm = configureFragmentsForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        private RadTreeView TreeView => configureFragmentsForm.TreeView;
        private XmlDocument XmlDocument => configureFragmentsForm.XmlDocument;

        public void SetControlValues(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsFileNode(treeNode))
                throw _exceptionHelper.CriticalException("{D5D262FD-AE37-401D-9F62-218D086F7187}");

            RemoveEventHandlers();
            XmlElement fragmentElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(fragmentElement).ToDictionary(e => e.Name);

            txtFragmentName.Text = fragmentElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value;
            txtFragmentName.Select();
            txtFragmentName.SelectAll();
            _richTextBoxPanelDescription.Lines = fragmentElement
                                                    .GetAttribute(XmlDataConstants.DESCRIPTIONATTRIBUTE)/*attribute may be null hence GetAttribute*/
                                                    .Split(Environment.NewLine, StringSplitOptions.None);
            _richTextBoxPanelFragment.Lines = new string[]
            {
                _xmlDocumentHelpers.GetXmlString
                (
                    _xmlDocumentHelpers.ToXmlDocument
                    (
                        _xmlDocumentHelpers.GetSingleChildElement(fragmentElement)
                    )
                )
            };
            AddEventHandlers();
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (TreeView.SelectedNode == null)
                return;

            if (!_treeViewService.IsFileNode(treeNode))
                throw _exceptionHelper.CriticalException("{9DBFAECE-12B3-4F26-B666-A428219881F5}");

            ValidateInputBoxes();

            XmlElement fragmentElement = _xmlDocumentHelpers.SelectSingleElement(XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(fragmentElement).ToDictionary(e => e.Name);
            string newNameAttributeValue = txtFragmentName.Text.Trim();

            fragmentElement.InnerXml = string.Join(Environment.NewLine, _richTextBoxPanelFragment.Lines);
            if (fragmentElement.Attributes[XmlDataConstants.DESCRIPTIONATTRIBUTE] == null)
                fragmentElement.Attributes.Append(_xmlDocumentHelpers.MakeAttribute(XmlDocument, XmlDataConstants.DESCRIPTIONATTRIBUTE, string.Join(Environment.NewLine, _richTextBoxPanelDescription.Lines)));
            else
                fragmentElement.Attributes[XmlDataConstants.DESCRIPTIONATTRIBUTE]!.Value = string.Join(Environment.NewLine, _richTextBoxPanelDescription.Lines);

            fragmentElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = newNameAttributeValue;

            configureFragmentsForm.ValidateXmlDocument();

            treeNode.Name = $"{treeNode.Parent.Name}/{XmlDataConstants.FRAGMENTELEMENT}[@{XmlDataConstants.NAMEATTRIBUTE}=\"{newNameAttributeValue}\"]";
            treeNode.Text = newNameAttributeValue;
        }

        private void AddEventHandlers()
        {
            txtFragmentName.TextChanged += TxtFragmentName_TextChanged;
            _richTextBoxPanelDescription.TextChanged += RichTextBoxPanelDescription_TextChanged;
            _richTextBoxPanelFragment.TextChanged += RichTextBoxPanelFragment_TextChanged;
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            ResetGroupBoxes();
            InitializeRichTextBoxPanelDescriptionControl();
            InitializeRichTextBoxPanelFragmentControl();
            //AddEventHandlers();
            //_richTextBoxPanelFragment.TextChanged runs when ConfigureFragmentControl (this control) is dynamically added
            //Event handlers will be added at the end of SetControlValues(RadTreeNode treeNode)
            CollapsePanelBorder(radPanelName);
            CollapsePanelBorder(radPanelContent);
            InitializeFragmentControls();
        }

        private void ResetGroupBoxes()
        {
            ((ISupportInitialize)radGroupBoxXml).BeginInit();
            radGroupBoxXml.SuspendLayout();
            ((ISupportInitialize)groupBoxName).BeginInit();
            groupBoxName.SuspendLayout();
            ((ISupportInitialize)groupBoxDescription).BeginInit();
            groupBoxDescription.SuspendLayout();
            ((ISupportInitialize)radPanelContent).BeginInit();
            radPanelContent.SuspendLayout();
            ((ISupportInitialize)radPanelName).BeginInit();
            radPanelName.SuspendLayout();
            ((ISupportInitialize)radPanelDescription).BeginInit();
            radPanelDescription.SuspendLayout();
            ((ISupportInitialize)groupBoxFragment).BeginInit();
            groupBoxFragment.SuspendLayout();
            SuspendLayout();

            radPanelName.Size = new Size(radPanelName.Width, PerFontSizeConstants.SingleRowGroupBoxHeight);
            groupBoxName.Padding = PerFontSizeConstants.SingleRowGroupBoxPadding;
            radPanelDescription.Size = new Size(radPanelDescription.Width, PerFontSizeConstants.MultiLineTextGroupBoxHeight);
            groupBoxDescription.Padding = PerFontSizeConstants.GroupBoxPadding;
            radGroupBoxXml.Padding = PerFontSizeConstants.GroupBoxPadding;
            groupBoxFragment.Padding = PerFontSizeConstants.ParentGroupBoxPadding;

            ((ISupportInitialize)groupBoxName).EndInit();
            groupBoxName.ResumeLayout(false);
            ((ISupportInitialize)groupBoxDescription).EndInit();
            groupBoxDescription.ResumeLayout(false);
            ((ISupportInitialize)radGroupBoxXml).EndInit();
            radGroupBoxXml.ResumeLayout(false);
            ((ISupportInitialize)radPanelContent).EndInit();
            radPanelContent.ResumeLayout(false);
            ((ISupportInitialize)radPanelName).EndInit();
            radPanelName.ResumeLayout(false);
            ((ISupportInitialize)radPanelDescription).EndInit();
            radPanelDescription.ResumeLayout(false);
            ((ISupportInitialize)groupBoxFragment).EndInit();
            groupBoxFragment.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        private void InitializeRichTextBoxPanelDescriptionControl()
        {
            ((ISupportInitialize)this.groupBoxDescription).BeginInit();
            this.groupBoxDescription.SuspendLayout();

            _richTextBoxPanelDescription.Dock = DockStyle.Fill;
            _richTextBoxPanelDescription.Location = new Point(0, 0);
            this.groupBoxDescription.Controls.Add(_richTextBoxPanelDescription);

            ((ISupportInitialize)this.groupBoxDescription).EndInit();
            this.groupBoxDescription.ResumeLayout(true);
        }

        private void InitializeRichTextBoxPanelFragmentControl()
        {
            ((ISupportInitialize)this.radGroupBoxXml).BeginInit();
            this.radGroupBoxXml.SuspendLayout();

            _richTextBoxPanelFragment.Dock = DockStyle.Fill;
            _richTextBoxPanelFragment.Location = new Point(0, 0);
            this.radGroupBoxXml.Controls.Add(_richTextBoxPanelFragment);

            ((ISupportInitialize)this.radGroupBoxXml).EndInit();
            this.radGroupBoxXml.ResumeLayout(true);
        }

        private void InitializeFragmentControls()
        {
            helpProvider.SetHelpString(txtFragmentName, Strings.fragmentConfigNameHelp);
            helpProvider.SetHelpString(_richTextBoxPanelDescription, Strings.fragmentConfigDescriptionHelp);
            helpProvider.SetHelpString(_richTextBoxPanelFragment, Strings.fragmentConfigXmlNameHelp);
            toolTip.SetToolTip(groupBoxName, Strings.fragmentConfigNameHelp);
            toolTip.SetToolTip(groupBoxDescription, Strings.fragmentConfigDescriptionHelp);
            toolTip.SetToolTip(radGroupBoxXml, Strings.fragmentConfigXmlNameHelp);
        }

        private void RemoveEventHandlers()
        {
            txtFragmentName.TextChanged -= TxtFragmentName_TextChanged;
            _richTextBoxPanelDescription.TextChanged -= RichTextBoxPanelDescription_TextChanged;
            _richTextBoxPanelFragment.TextChanged -= RichTextBoxPanelFragment_TextChanged;
        }

        private void TryUpdateXmlDocument()
        {
            try
            {
                UpdateXmlDocument(TreeView.SelectedNode);
            }
            catch (XmlException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                configureFragmentsForm.SetErrorMessage(ex.Message);
            }
        }

        private void ValidateInputBoxes()
        {
            configureFragmentsForm.ClearMessage();
            ValidateDescription();
            ValidateFragmentName();
            ValidateFragment();
        }

        private static void ValidateDescription()
        {
        }

        private void ValidateFragment()
        {
            try
            {
                new XmlDocument().LoadXml(string.Join(Environment.NewLine, _richTextBoxPanelFragment.Lines));
            }
            catch (XmlException)
            {
                throw new LogicBuilderException(Strings.fragmentConfigInvalidXmlFragment);
            }
        }

        private void ValidateFragmentName()
        {
            if (!XmlNameAttributeRegex().IsMatch(txtFragmentName.Text))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidTxtNameTextFormat, groupBoxName.Text));
        }

        #region Event Handlers
        private void RichTextBoxPanelDescription_TextChanged(object? sender, EventArgs e) => TryUpdateXmlDocument();

        private void RichTextBoxPanelFragment_TextChanged(object? sender, EventArgs e) => TryUpdateXmlDocument();

        private void TxtFragmentName_TextChanged(object? sender, EventArgs e) => TryUpdateXmlDocument();
        [GeneratedRegex(RegularExpressions.XMLNAMEATTRIBUTE)]
        private static partial Regex XmlNameAttributeRegex();
        #endregion Event Handlers
    }
}
