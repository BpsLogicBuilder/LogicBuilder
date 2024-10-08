﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Properties;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal partial class FindConfiguredItemInCellBase : RadForm
    {
        protected IConfigurationService _configurationService;
        protected IExceptionHelper _exceptionHelper;
        protected IFindAndReplaceHelper _findAndReplaceHelper;
        protected IFormInitializer _formInitializer;
        protected IRadDropDownListHelper _radDropDownListHelper;
        protected ISearchFunctions _searchFunctions;

        public FindConfiguredItemInCellBase(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFindAndReplaceHelper findAndReplaceHelper,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            ISearchFunctions searchFunctions)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _findAndReplaceHelper = findAndReplaceHelper;
            _formInitializer = formInitializer;
            _radDropDownListHelper = radDropDownListHelper;
            _searchFunctions = searchFunctions;
            InitializeComponent();
            Initialize();
        }

        #region Variables
        protected RadGridView? dataGridView;
        protected int searchRowIndex;
        protected int searchCellIndex;
        #endregion Variables

        #region Methods
        public void Setup(RadGridView dataGridView)
        {
            this.dataGridView = dataGridView;
        }

        protected virtual void DoFind()
        {
            throw new NotImplementedException();
        }

        protected virtual void GetSettings()
        {
            radRadioButtonCurrentRow.IsChecked = true;
            /*We'll use the CurrentRow's CheckStateChanged for ResetSearchIndexes() - only one is required*/
            if (Settings.Default.findCurrentRow == false)
                radRadioButtonAllRows.IsChecked = true;
        }

        protected virtual void InitializeControls()
        {
            throw new NotImplementedException();
        }

        protected virtual void SaveSettings()
        {
            throw new NotImplementedException();
        }

        private void FindNext()
        {
            radGroupBoxOccurrences.Text = string.Empty;
            radListOccurrences.Items.Clear();
            DoFind();
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, PerFontSizeConstants.FindInShapeOrCellFormMinimumHeight);

            this.AcceptButton = radButtonFindNext;

            ControlsLayoutUtility.SetLabelMargin(radRadioButtonAllRows);
            ControlsLayoutUtility.SetLabelMargin(radRadioButtonCurrentRow);
            ControlsLayoutUtility.SetLabelMargin(radCheckBoxMatchCase);
            ControlsLayoutUtility.SetLabelMargin(radCheckBoxMatchWholeWord);
            radPanelCommandButtons.Padding = new Padding(1);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelFill);
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelCommandButtons, tableLayoutPanelButtons);
            ControlsLayoutUtility.LayoutGroupBox(radPanelFill, radGroupBoxOccurrences);
            InitializeControls();
            GetSettings();

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
            radButtonFindNext.Enabled = radDropDownListText.Text.Length > 0;
        }
        #endregion Methods

        #region Event Handlers
        private void RadDropDownListText_TextChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
            ValidateOk();
        }

        private void RadButtonFindNext_Click(object sender, EventArgs e)
        {
            FindNext();
        }

        private void RadRadioButtonCurrentRow_CheckStateChanged(object sender, EventArgs e)
        {
            ResetSearchIndexes();
        }

        private void RadRadioButtonAllRows_CheckStateChanged(object sender, EventArgs e)
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

        private void FindConfiguredItemInCellBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
        }
        #endregion Event Handlers
    }
}
