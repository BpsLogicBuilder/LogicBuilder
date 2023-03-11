using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditStandardFunction
{
    internal partial class EditStandardFunctionControl : UserControl, IEditStandardFunctionControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditFunctionControlHelper _editFunctionControlHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionElementValidator _functionElementValidator;
        private readonly IFunctionHelper _functionHelper;
        private readonly IFunctionParameterControlSetValidator _functionParameterControlSetValidator;
        private readonly IFieldControlFactory _fieldControlFactory;
        private readonly IGenericFunctionHelper _genericFunctionHelper;
        private readonly ILoadParameterControlsDictionary _loadParameterControlsDictionary;
        private readonly IRadCheckBoxHelper _radCheckBoxHelper;
        private readonly ITableLayoutPanelHelper _tableLayoutPanelHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IDataGraphEditingForm dataGraphEditingForm;

        private readonly Type assignedTo;
        private readonly IDictionary<string, ParameterControlSet> editControlsSet = new Dictionary<string, ParameterControlSet>();
        private readonly XmlDocument xmlDocument;
        private readonly string? selectedParameter;

        private readonly RadGroupBox groupBoxFunction;
        private readonly RadScrollablePanel radPanelFunction;
        private readonly RadPanel radPanelTableParent;
        private readonly TableLayoutPanel tableLayoutPanel;
        private readonly RadCheckBox radCheckBoxNot;
        private readonly RadLabel lblFunction;
        private readonly RadLabel? lblGenericArguments;

        public EditStandardFunctionControl(
            IConfigurationService configurationService,
            IFunctionDataParser functionDataParser,
            IFunctionElementValidator functionElementValidator,
            IFunctionHelper functionHelper,
            IFunctionParameterControlSetValidator functionParameterControlSetValidator,
            IEditingControlHelperFactory editingControlHelperFactory,
            IFieldControlFactory fieldControlFactory,
            IGenericFunctionHelper genericFunctionHelper,
            IRadCheckBoxHelper radCheckBoxHelper,
            ITableLayoutPanelHelper tableLayoutPanelHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingForm dataGraphEditingForm,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _functionDataParser = functionDataParser;
            _functionElementValidator = functionElementValidator;
            _functionHelper = functionHelper;
            _functionParameterControlSetValidator = functionParameterControlSetValidator;
            _fieldControlFactory = fieldControlFactory;
            _genericFunctionHelper = genericFunctionHelper;
            _radCheckBoxHelper = radCheckBoxHelper;
            _tableLayoutPanelHelper = tableLayoutPanelHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingForm = dataGraphEditingForm;
            this.function = function;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );

            this.assignedTo = assignedTo;
            this.selectedParameter = selectedParameter;

            _editFunctionControlHelper = editingControlHelperFactory.GetEditFunctionControlHelper(this);
            _loadParameterControlsDictionary = editingControlHelperFactory.GetLoadParameterControlsDictionary(this, dataGraphEditingForm);

            this.groupBoxFunction = new RadGroupBox();
            this.radPanelFunction = new RadScrollablePanel();
            this.radPanelTableParent = new RadPanel();
            this.tableLayoutPanel = new TableLayoutPanel();
            this.lblFunction = new RadLabel();
            this.radCheckBoxNot = new();
            if (function.HasGenericArguments)
            {
                lblGenericArguments = new()
                {
                    Dock = DockStyle.Fill,
                    Location = new Point(0, 0),
                    Name = "lblGenericArguments",
                    Text = Strings.lblGenericArgumentsText
                };
            }

            InitializeControls();
        }

        private Function function;

        public bool DenySpecialCharacters => dataGraphEditingForm.DenySpecialCharacters;

        public bool DisplayNotCheckBox => dataGraphEditingForm.DisplayNotCheckBox;

        public Function Function => function;

        public XmlDocument XmlDocument => xmlDocument;

        public XmlElement XmlResult => _editFunctionControlHelper.GetXmlResult(editControlsSet);

        public ApplicationTypeInfo Application => dataGraphEditingForm.Application;

        public bool IsValid => throw new NotImplementedException();

        public string? SelectedParameter => selectedParameter;

        public void ClearMessage() => dataGraphEditingForm.ClearMessage();

        public void RequestDocumentUpdate() => dataGraphEditingForm.RequestDocumentUpdate(this);

        public void ResetControls()
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            InitializeControls();
            this.PerformLayout();
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        public void SetErrorMessage(string message) => dataGraphEditingForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => dataGraphEditingForm.SetMessage(message, title);

        public void ValidateFields()
        {
            List<string> errors = new();
            _functionParameterControlSetValidator.Validate(editControlsSet, function, Application, errors);
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));

            _functionElementValidator.Validate(XmlResult, assignedTo, Application, errors);
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private void InitializeControls()
        {
            ((ISupportInitialize)(this.groupBoxFunction)).BeginInit();
            this.groupBoxFunction.SuspendLayout();
            ((ISupportInitialize)(this.radPanelFunction)).BeginInit();
            this.radPanelFunction.PanelContainer.SuspendLayout();
            this.radPanelFunction.SuspendLayout();
            ((ISupportInitialize)(this.radPanelTableParent)).BeginInit();
            this.radPanelTableParent.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            ((ISupportInitialize)(this.lblFunction)).BeginInit();
            if (function.HasGenericArguments)
            {
                ((ISupportInitialize)(this.lblGenericArguments!)).BeginInit();
            }
            // 
            // groupBoxFunction
            // 
            this.groupBoxFunction.AccessibleRole = AccessibleRole.Grouping;
            this.groupBoxFunction.Controls.Add(this.radPanelFunction);
            this.groupBoxFunction.Dock = DockStyle.Fill;
            this.groupBoxFunction.HeaderText = Strings.editFunctionGroupBoxHeaderText;
            this.groupBoxFunction.Location = new Point(0, 0);
            this.groupBoxFunction.Name = "groupBoxFunction";
            this.groupBoxFunction.Size = new Size(855, 300);
            this.groupBoxFunction.TabIndex = 0;
            this.groupBoxFunction.Text = Strings.editFunctionGroupBoxHeaderText;
            // 
            // radPanelFunction
            // 
            this.radPanelFunction.Dock = DockStyle.Fill;
            this.radPanelFunction.Location = new Point(2, 18);
            this.radPanelFunction.Name = "radPanelFunction";
            // 
            // radPanelFunction.PanelContainer
            // 
            this.radPanelFunction.PanelContainer.Controls.Add(this.radPanelTableParent);
            this.radPanelFunction.PanelContainer.Size = new Size(849, 278);
            this.radPanelFunction.Size = new Size(851, 280);
            this.radPanelFunction.TabIndex = 0;
            //radPanelTableParent
            //tableLayoutPanel
            editControlsSet.Clear();
            tableLayoutPanel.Controls.Clear();

            int currentRow = 1;//function name row
            this.tableLayoutPanel.Controls.Add(this.radCheckBoxNot, 2, currentRow);
            this.tableLayoutPanel.Controls.Add(this.lblFunction, 3, currentRow);
            currentRow += 2;

            if (function.HasGenericArguments)
            {
                Control genericConfigurationControl = (Control)_fieldControlFactory.GetFunctionGenericParametersControl(this);
                genericConfigurationControl.Location = new Point(0, 0);
                genericConfigurationControl.Dock = DockStyle.Fill;
                this.tableLayoutPanel.Controls.Add(this.lblGenericArguments, 2, currentRow);
                this.tableLayoutPanel.Controls.Add(genericConfigurationControl, 3, currentRow);
                currentRow += 2;
            }

            if (function.HasGenericArguments)
            {
                if (_editFunctionControlHelper.ValidateGenericArgs())
                {
                    FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
                    function = _genericFunctionHelper.ConvertGenericTypes
                    (
                        _configurationService.FunctionList.Functions[function.Name], //start with _configurationService.FunctionList.Functions to make sure we are starting with a generic type definition.
                        functionData.GenericArguments, Application
                    );

                    LoadParameterControls();
                    _editFunctionControlHelper.UpdateParameterControls(tableLayoutPanel, editControlsSet);
                }

                //We still need the layout for the genericConfigurationControl and lblFunction
                //If ValidateGenericArgs(), then this must run after converting any generic parameters
                _tableLayoutPanelHelper.SetUp(tableLayoutPanel, radPanelTableParent, function.Parameters, function.HasGenericArguments);

            }
            else
            {
                LoadParameterControls();
                _editFunctionControlHelper.UpdateParameterControls(tableLayoutPanel, editControlsSet);
                _tableLayoutPanelHelper.SetUp(tableLayoutPanel, radPanelTableParent, function.Parameters, function.HasGenericArguments);
            }

            void LoadParameterControls()
            {
                _loadParameterControlsDictionary.Load(editControlsSet, function.Parameters);
                foreach (ParameterBase parameter in function.Parameters)
                {
                    ParameterControlSet parameterControlSet = editControlsSet[parameter.Name];
                    this.tableLayoutPanel.Controls.Add(parameterControlSet.ImageLabel, 1, currentRow);
                    this.tableLayoutPanel.Controls.Add(parameterControlSet.ChkInclude, 2, currentRow);
                    this.tableLayoutPanel.Controls.Add(parameterControlSet.Control, 3, currentRow);
                    ShowHideParameterControls(parameterControlSet.ChkInclude);
                    parameterControlSet.ChkInclude.CheckStateChanged += ChkInclude_CheckStateChanged;
                    currentRow += 2;
                }
            }

            //
            //radCheckBoxNot
            //
            radCheckBoxNot.Dock = DockStyle.Top;
            radCheckBoxNot.Name = "radCheckBoxNot";
            radCheckBoxNot.Text = Strings.notString;
            _radCheckBoxHelper.SetLabelMargin(radCheckBoxNot);
            radCheckBoxNot.Visible = DisplayNotCheckBox && _functionHelper.IsBoolean(Function);

            // 
            // lblFunctionName
            // 
            this.lblFunction.Dock = DockStyle.Fill;
            this.lblFunction.Location = new Point(0, 0);
            this.lblFunction.Name = "lblFunction";
            this.lblFunction.Size = new Size(39, 18);
            this.lblFunction.TabIndex = 0;
            this.lblFunction.Text = function.Name;
            lblFunction.TextAlignment = ContentAlignment.MiddleLeft;
            lblFunction.Font = new Font(lblFunction.Font, FontStyle.Bold);

            // 
            // EditFunctionControl
            // 
            this.AutoScaleDimensions = new SizeF(9F, 21F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxFunction);
            this.Name = "ConfigureFunctionControl";
            this.Size = new Size(855, 300);
            ((ISupportInitialize)(this.groupBoxFunction)).EndInit();
            this.groupBoxFunction.ResumeLayout(false);
            this.radPanelFunction.PanelContainer.ResumeLayout(false);
            ((ISupportInitialize)(this.radPanelFunction)).EndInit();
            this.radPanelFunction.ResumeLayout(false);
            ((ISupportInitialize)(this.radPanelTableParent)).EndInit();
            this.radPanelTableParent.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();

            ((ISupportInitialize)(this.lblFunction)).EndInit();
            if (function.HasGenericArguments)
            {
                ((ISupportInitialize)(this.lblGenericArguments!)).EndInit();
            }

            this.ResumeLayout(false);

            CollapsePanelBorder(radPanelTableParent);
            CollapsePanelBorder(radPanelFunction);
        }

        private void ShowHideParameterControls(RadCheckBox chkSender)
        {
            if (chkSender.Checked)
                editControlsSet[chkSender.Name].ValueControl.ShowControls();
            else
                editControlsSet[chkSender.Name].ValueControl.HideControls();
        }

        #region Event Handlers
        private void ChkInclude_CheckStateChanged(object? sender, EventArgs e)
        {
            if (sender is not RadCheckBox radChackBox)
                return;

            ShowHideParameterControls(radChackBox);
        }
        #endregion Event Handlers
    }
}
