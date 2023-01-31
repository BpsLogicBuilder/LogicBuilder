﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments.ConfigureGenericObjectArgument
{
    internal partial class ConfigureGenericObjectArgumentControl : UserControl, IConfigureGenericObjectArgumentControl
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly ITreeViewService _treeViewService;
        private readonly ITypeAutoCompleteManager _cmbCpObjectTypeTypeAutoCompleteManager;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureGenericArgumentsForm configureGenericArgumentsForm;

        public ConfigureGenericObjectArgumentControl(
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IServiceFactory serviceFactory,
            ITreeViewService treeViewService,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureGenericArgumentsForm configureGenericArgumentsForm)
        {
            InitializeComponent();
            _cmbCpObjectTypeTypeAutoCompleteManager = serviceFactory.GetTypeAutoCompleteManager
            (
                configureGenericArgumentsForm,
                cmbCpObjectType
            );
            _exceptionHelper = exceptionHelper;
            _treeViewService = treeViewService;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _radDropDownListHelper = radDropDownListHelper;
            this.configureGenericArgumentsForm = configureGenericArgumentsForm;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        #region Properties
        public RadTreeView TreeView => configureGenericArgumentsForm.TreeView;
        public XmlDocument XmlDocument => configureGenericArgumentsForm.XmlDocument;
        #endregion Properties

        public void SetControlValues(RadTreeNode treeNode)
        {
            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            cmbCpGenericArgumentName.SelectedValue = parameterElement.GetAttribute(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE);
            cmbCpObjectType.Text = elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText;
            cmbCpUseForEquality.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText);
            cmbCpUseForHashCode.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText);
            cmbCpUseForToString.SelectedValue = bool.Parse(elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText);
        }

        public void UpdateXmlDocument(RadTreeNode treeNode)
        {
            if (!_treeViewService.IsGenericArgumentParameterNode(treeNode))
                throw _exceptionHelper.CriticalException("{65658D01-62BD-49F8-B417-54D057EB8EAA}");

            List<string> errors = ValidateFields();
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));

            XmlElement parameterElement = _xmlDocumentHelpers.SelectSingleElement(this.XmlDocument, treeNode.Name);
            Dictionary<string, XmlElement> elements = _xmlDocumentHelpers.GetChildElements(parameterElement).ToDictionary(e => e.Name);

            elements[XmlDataConstants.OBJECTTYPEELEMENT].InnerText = cmbCpObjectType.Text;
            elements[XmlDataConstants.USEFOREQUALITYELEMENT].InnerText = bool.Parse(cmbCpUseForEquality.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFORHASHCODEELEMENT].InnerText = bool.Parse(cmbCpUseForHashCode.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
            elements[XmlDataConstants.USEFORTOSTRINGELEMENT].InnerText = bool.Parse(cmbCpUseForToString.SelectedValue.ToString()!).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();

            configureGenericArgumentsForm.ValidateXmlDocument();
        }

        private static List<string> ValidateFields()
        {
            return new List<string>();
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            radPanelParameter.VerticalScrollBarState = ScrollState.AlwaysShow;
            InitializeTableLayoutPanel();
            this.cmbCpGenericArgumentName.ReadOnly = true;
            radPanelParameter.AutoScroll = true;
            CollapsePanelBorder(radPanelParameter);
            CollapsePanelBorder(radPanelTableParent);
            InitializeParameterControls();
            LoadParameterDropDownLists();
            _cmbCpObjectTypeTypeAutoCompleteManager.Setup();
        }

        private void InitializeParameterControls()
        {
            helpProvider.SetHelpString(this.cmbCpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            helpProvider.SetHelpString(this.cmbCpObjectType, Strings.constrConfigObjectTypeHelp);
            helpProvider.SetHelpString(this.cmbCpUseForEquality, Strings.constrConfigUseForEqualityHelp);
            helpProvider.SetHelpString(this.cmbCpUseForHashCode, Strings.constrConfigUseForHashCodeHelp);
            helpProvider.SetHelpString(this.cmbCpUseForToString, Strings.constrConfigUseForToStringHelp);
            toolTip.SetToolTip(this.lblCpGenericArgumentName, Strings.constrConfigGenericArgumentNameHelp);
            toolTip.SetToolTip(this.lblCpObjectType, Strings.constrConfigObjectTypeHelp);
            toolTip.SetToolTip(this.lblCpUseForEquality, Strings.constrConfigUseForEqualityHelp);
            toolTip.SetToolTip(this.lblCpUseForHashCode, Strings.constrConfigUseForHashCodeHelp);
            toolTip.SetToolTip(this.lblCpUseForToString, Strings.constrConfigUseForToStringHelp);
        }

        private void InitializeTableLayoutPanel()
        {
            float size_20 = 20F / 220 * 100;
            float size_30 = 30F / 220 * 100;
            float size_6 = 6F / 220 * 100;

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
            this.tableLayoutPanel.RowStyles[11] = new RowStyle(SizeType.Percent, size_20);

            ((ISupportInitialize)this.radPanelTableParent).EndInit();
            this.radPanelTableParent.ResumeLayout(true);
        }

        private void LoadParameterDropDownLists()
        {
            _radDropDownListHelper.LoadTextItems(cmbCpGenericArgumentName, configureGenericArgumentsForm.ConfiguredGenericArgumentNames);
            _radDropDownListHelper.LoadBooleans(cmbCpUseForEquality);
            _radDropDownListHelper.LoadBooleans(cmbCpUseForHashCode);
            _radDropDownListHelper.LoadBooleans(cmbCpUseForToString);
        }
    }
}
