using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.Factories;
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
using System.Linq;
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
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionElementValidator _functionElementValidator;
        private readonly IFunctionHelper _functionHelper;
        private readonly IFunctionParameterControlSetValidator _functionParameterControlSetValidator;
        private readonly IGenericFunctionHelper _genericFunctionHelper;
        private readonly ILoadParameterControlsDictionary _loadParameterControlsDictionary;
        private readonly IParameterFieldControlFactory _fieldControlFactory;
        private readonly IRadCheckBoxHelper _radCheckBoxHelper;
        private readonly ITableLayoutPanelHelper _tableLayoutPanelHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IDataGraphEditingHost dataGraphEditingHost;

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
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IFunctionElementValidator functionElementValidator,
            IFunctionHelper functionHelper,
            IFunctionParameterControlSetValidator functionParameterControlSetValidator,
            IEditingControlHelperFactory editingControlHelperFactory,
            IGenericFunctionHelper genericFunctionHelper,
            IParameterFieldControlFactory fieldControlFactory,
            IRadCheckBoxHelper radCheckBoxHelper,
            ITableLayoutPanelHelper tableLayoutPanelHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost,
            Function function,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionElementValidator = functionElementValidator;
            _functionHelper = functionHelper;
            _functionParameterControlSetValidator = functionParameterControlSetValidator;
            _fieldControlFactory = fieldControlFactory;
            _genericFunctionHelper = genericFunctionHelper;
            _radCheckBoxHelper = radCheckBoxHelper;
            _tableLayoutPanelHelper = tableLayoutPanelHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
            this.function = function;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );

            this.assignedTo = assignedTo;
            this.selectedParameter = selectedParameter;

            _editFunctionControlHelper = editingControlHelperFactory.GetEditFunctionControlHelper(this);
            _loadParameterControlsDictionary = editingControlHelperFactory.GetLoadParameterControlsDictionary(this, dataGraphEditingHost);

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
        private readonly RadToolTip toolTip = new();

        public bool DenySpecialCharacters => dataGraphEditingHost.DenySpecialCharacters;

        public bool DisplayNotCheckBox => dataGraphEditingHost.DisplayNotCheckBox;

        public Function Function => function;

        public XmlDocument XmlDocument => xmlDocument;

        public XmlElement XmlResult => _editFunctionControlHelper.GetXmlResult(editControlsSet, radCheckBoxNot.Checked);

        public string VisibleText => _functionDataParser.Parse(XmlResult).VisibleText;

        public ApplicationTypeInfo Application => dataGraphEditingHost.Application;

        public bool IsValid
        {
            get
            {
                List<string> errors = new();
                _functionParameterControlSetValidator.Validate(editControlsSet, function, Application, errors);
                if (errors.Count > 0)
                    return false;

                _functionElementValidator.Validate(XmlResult, assignedTo, Application, errors);
                if (errors.Count > 0)
                    return false;

                return true;
            }
        }

        public string? SelectedParameter => selectedParameter;

        public void ClearMessage() => dataGraphEditingHost.ClearMessage();

        public void RequestDocumentUpdate() => dataGraphEditingHost.RequestDocumentUpdate(this);

        public void ResetControls()
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            InitializeControls();
            this.PerformLayout();
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        public void SetErrorMessage(string message) => dataGraphEditingHost.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => dataGraphEditingHost.SetMessage(message, title);

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
            //this.SuspendLayout();
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
            this.groupBoxFunction.Padding = PerFontSizeConstants.GroupBoxPadding;
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
            foreach (ParameterControlSet controlSet in editControlsSet.Values)
            {
                Dispose(controlSet.ImageLabel);
                Dispose(controlSet.ChkInclude);
                Dispose(controlSet.Control);
                void Dispose(Control control)
                {
                    if (!control.IsDisposed)
                        control.Dispose();
                }
            }
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
                genericConfigurationControl.Margin = new Padding(0);

                if (this.lblGenericArguments == null)
                    throw _exceptionHelper.CriticalException("{3D9FEF45-F7BF-478F-BB80-8A1AE156F27D}");
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
                    SetCheckNotState(functionData.IsNotFunction);
                }

                //We still need the layout for the genericConfigurationControl and lblFunction
                //If ValidateGenericArgs(), then this must run after converting any generic parameters
                _tableLayoutPanelHelper.SetUp(tableLayoutPanel, radPanelTableParent, function.Parameters, function.HasGenericArguments);

            }
            else
            {
                LoadParameterControls();
                _editFunctionControlHelper.UpdateParameterControls(tableLayoutPanel, editControlsSet);
                FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
                SetCheckNotState(functionData.IsNotFunction);
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
                    //ShowHideParameterControls(parameterControlSet.ChkInclude);
                    //Incorrect layout fails for RadDropDownList if Visible set to false before
                    //the call to this.tableLayoutPanel.PerformLayout();
                    //Reproduced a similar layout in InvalidDropDownListLayoutWhenVisibleIsFalse
                    //by not calling tableLayoutPanel.PerformLayout() (could not be reproduced by setting visible to false before the PerformLayout() call as in this case)
                    parameterControlSet.ChkInclude.CheckStateChanged += ChkInclude_CheckStateChanged;
                    if (parameter.Comments.Trim().Length > 0)
                        toolTip.SetToolTip(parameterControlSet.ImageLabel, parameter.Comments);
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
            radCheckBoxNot.CheckStateChanged -= RadCheckBoxNot_CheckStateChanged;
            radCheckBoxNot.CheckStateChanged += RadCheckBoxNot_CheckStateChanged;

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
            lblFunction.Font = new Font
            (
                ForeColorUtility.GetDefaultFont(ThemeResolutionService.ApplicationThemeName),
                FontStyle.Bold
            );

            // 
            // EditFunctionControl
            // 
            this.AutoScaleDimensions = new SizeF(9F, 21F);
            this.AutoScaleMode = AutoScaleMode.None;
            this.Controls.Add(this.groupBoxFunction);
            this.Name = "ConfigureFunctionControl";
            this.Size = new Size(855, 300);
            ((ISupportInitialize)(this.groupBoxFunction)).EndInit();
            
            this.radPanelFunction.PanelContainer.ResumeLayout(false);
            ((ISupportInitialize)(this.radPanelFunction)).EndInit();
            this.radPanelFunction.ResumeLayout(false);
            ((ISupportInitialize)(this.radPanelTableParent)).EndInit();
            this.radPanelTableParent.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBoxFunction.ResumeLayout(true);

            if (editControlsSet.Any())
            {//editControlsSet could be empty here if HasGenericArguments %% ValidateGenericArgs() == false
                foreach (ParameterBase parameter in function.Parameters)
                {//Incorrect layout fails for RadDropDownList if Visible set to false before
                 //the call to this.tableLayoutPanel.PerformLayout();
                 //Reproduced a similar layout in InvalidDropDownListLayoutWhenVisibleIsFalse
                 //by not calling tableLayoutPanel.PerformLayout() (could not be reproduced by setting visible to false before the PerformLayout() call as in this case)
                 //This code should otherwise run where commented above "//ShowHideParameterControls(parameterControlSet.ChkInclude);"
                    ParameterControlSet parameterControlSet = editControlsSet[parameter.Name];
                    parameterControlSet.ChkInclude.Enabled = parameter.IsOptional;
                    ShowHideParameterControls(parameterControlSet.ChkInclude);
                }
            }

            ((ISupportInitialize)(this.lblFunction)).EndInit();
            if (function.HasGenericArguments)
            {
                ((ISupportInitialize)(this.lblGenericArguments!)).EndInit();
            }

            //this.ResumeLayout(false);

            CollapsePanelBorder(radPanelTableParent);
            CollapsePanelBorder(radPanelFunction);

            this.Disposed += EditStandardFunctionControl_Disposed;
        }

        private void RemoveCheckStateChangedHandlers()
        {
            radCheckBoxNot.CheckStateChanged -= RadCheckBoxNot_CheckStateChanged;
            foreach (var kvp in editControlsSet)
            {
                kvp.Value.ChkInclude.CheckStateChanged -= ChkInclude_CheckStateChanged;
            }
        }

        private void SetCheckNotState(bool isChecked)
        {
            radCheckBoxNot.CheckStateChanged -= RadCheckBoxNot_CheckStateChanged;
            radCheckBoxNot.Checked = isChecked;
            radCheckBoxNot.CheckStateChanged += RadCheckBoxNot_CheckStateChanged;
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

        private void EditStandardFunctionControl_Disposed(object? sender, EventArgs e)
        {
            toolTip.RemoveAll();
            toolTip.Dispose();
            RemoveCheckStateChangedHandlers();
        }

        private void RadCheckBoxNot_CheckStateChanged(object? sender, EventArgs e)
        {
            RequestDocumentUpdate();
        }
        #endregion Event Handlers
    }
}
