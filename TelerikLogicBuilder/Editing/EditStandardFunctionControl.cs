using ABIS.LogicBuilder.FlowBuilder.Constants;
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
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal partial class EditStandardFunctionControl : UserControl, IEditStandardFunctionControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionElementValidator _functionElementValidator;
        private readonly IFunctionGenericsConfigrationValidator _functionGenericsConfigrationValidator;
        private readonly IFunctionParameterControlSetValidator _functionParameterControlSetValidator;
        private readonly IFieldControlFactory _fieldControlFactory;
        private readonly IGenericFunctionHelper _genericFunctionHelper;
        private readonly ILoadParameterControlsDictionary _loadParameterControlsDictionary;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITableLayoutPanelHelper _tableLayoutPanelHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IUpdateParameterControlValues _updateParameterControlValues;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IEditingForm editingForm;

        private readonly Type assignedTo;
        private readonly IDictionary<string, ParameterControlSet> editControlsSet = new Dictionary<string, ParameterControlSet>();
        private readonly XmlDocument xmlDocument;
        private readonly string? selectedParameter;

        private readonly RadGroupBox groupBoxFunction;
        private readonly RadScrollablePanel radPanelFunction;
        private readonly RadPanel radPanelTableParent;
        private readonly TableLayoutPanel tableLayoutPanel;
        private readonly RadLabel lblFunction;
        private readonly RadLabel? lblGenericArguments;

        public EditStandardFunctionControl(
            IConfigurationService configurationService,
            IFunctionDataParser functionDataParser,
            IFunctionElementValidator functionElementValidator,
            IFunctionGenericsConfigrationValidator functionGenericsConfigrationValidator,
            IFunctionParameterControlSetValidator functionParameterControlSetValidator,
            IEditingControlHelperFactory editingControlFactory,
            IFieldControlFactory fieldControlFactory,
            IGenericFunctionHelper genericFunctionHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            ITableLayoutPanelHelper tableLayoutPanelHelper,
            ITypeLoadHelper typeLoadHelper,
            IUpdateParameterControlValues updateParameterControlValues,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditingForm editingForm,
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
            _functionGenericsConfigrationValidator = functionGenericsConfigrationValidator;
            _functionParameterControlSetValidator = functionParameterControlSetValidator;
            _fieldControlFactory = fieldControlFactory;
            _genericFunctionHelper = genericFunctionHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _tableLayoutPanelHelper = tableLayoutPanelHelper;
            _typeLoadHelper = typeLoadHelper;
            _updateParameterControlValues = updateParameterControlValues;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.editingForm = editingForm;
            this.function = function;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );

            this.assignedTo = assignedTo;
            this.selectedParameter = selectedParameter;

            _loadParameterControlsDictionary = editingControlFactory.GetLoadParameterControlsDictionary(this, editingForm);

            this.groupBoxFunction = new RadGroupBox();
            this.radPanelFunction = new RadScrollablePanel();
            this.radPanelTableParent = new RadPanel();
            this.tableLayoutPanel = new TableLayoutPanel();
            this.lblFunction = new RadLabel();
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

        private static readonly string XmlParentXPath = $"/{XmlDataConstants.NOTELEMENT}|/{XmlDataConstants.FUNCTIONELEMENT}";
        private static readonly string ParametersXPath = $"{XmlParentXPath}/{XmlDataConstants.PARAMETERSELEMENT}";

        public Function Function => function;

        public XmlDocument XmlDocument => xmlDocument;

        public XmlElement XmlResult => GetXmlResult();

        public ApplicationTypeInfo Application => editingForm.Application;

        public bool IsValid => throw new NotImplementedException();

        public void ClearMessage() => editingForm.ClearMessage();

        public void RequestDocumentUpdate() => editingForm.RequestDocumentUpdate();

        public void ResetControls()
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            InitializeControls();
            this.PerformLayout();
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        public void SetErrorMessage(string message) => editingForm.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => editingForm.SetMessage(message, title);

        public void ValidateFields()
        {
            List<string> errors = new();
            errors.AddRange(ValidateControls());
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

        private XmlElement GetXmlResult()
        {
            FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
            string xmlString = _xmlDataHelper.BuildFunctionXml
            (
                function.Name,
                functionData.VisibleText,
                _xmlDataHelper.BuildGenericArgumentsXml(functionData.GenericArguments),
                GetParametersXml()
            );

            return _refreshVisibleTextHelper.RefreshFunctionVisibleTexts
            (
                _xmlDocumentHelpers.ToXmlElement(xmlString),
                Application
            );

            string GetParametersXml()
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.PARAMETERSELEMENT);
                    foreach (ParameterBase parameter in function.Parameters)
                    {
                        if (!editControlsSet[parameter.Name].ChkInclude.Checked)
                            continue;

                        xmlTextWriter.WriteRaw(editControlsSet[parameter.Name].ValueControl.XmlElement!.OuterXml);
                    }
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                }
                return stringBuilder.ToString();
            }
        }

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
                if (ValidateGenericArgs())
                {
                    FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
                    function = _genericFunctionHelper.ConvertGenericTypes
                    (
                        _configurationService.FunctionList.Functions[function.Name], //start with _configurationService.FunctionList.Functions to make sure we are starting with a generic type definition.
                        functionData.GenericArguments, Application
                    );

                    LoadParameterControls();
                    UpdateParameterControls();
                }

                //We still need the layout for the genericConfigurationControl and lblFunction
                //If ValidateGenericArgs(), then this must run after converting any generic parameters
                _tableLayoutPanelHelper.SetUp(tableLayoutPanel, radPanelTableParent, function.Parameters, function.HasGenericArguments);

            }
            else
            {
                LoadParameterControls();
                UpdateParameterControls();
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

        private void UpdateParameterControls()
        {
            ClearMessage();

            FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
            if (!_configurationService.FunctionList.Functions.ContainsKey(functionData.Name))
            {
                SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionData.Name));
                editControlsSet.Clear();
                tableLayoutPanel.Controls.Clear();
                return;
            }

            if (!ValidateParameters())
            {
                editControlsSet.Clear();
                tableLayoutPanel.Controls.Clear();
                return;
            }

            radPanelFunction.Enabled = true;

            bool newFunction = functionData.ParameterElementsList.Count == 0;
            _updateParameterControlValues.PrepopulateRequiredFields
            (
                editControlsSet,
                functionData.ParameterElementsList.ToDictionary(p => p.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)),
                function.Parameters.ToDictionary(p => p.Name),
                this.xmlDocument,
                ParametersXPath,
                Application
            );

            //after updating parameter fields refresh functionData - we'll need the updated ParameterElementsList.
            functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(xmlDocument));

            if (newFunction)
            {
                _updateParameterControlValues.SetDefaultsForLiterals
                (
                    editControlsSet,
                    function.Parameters.ToDictionary(p => p.Name)
                );//these are configured defaults for LiteralParameter and ListOfLiteralsParameter - different from prepopulating
            }
            else
            {
                _updateParameterControlValues.UpdateExistingFields
                (
                    functionData.ParameterElementsList,
                    editControlsSet,
                    function.Parameters.ToDictionary(p => p.Name),
                    selectedParameter
                );
            }
        }

        private IList<string> ValidateControls()
        {
            List<string> errors = new();
            _functionParameterControlSetValidator.Validate(editControlsSet, function, Application, errors);
            return errors;
        }

        private bool ValidateGenericArgs()
        {
            if (!function.HasGenericArguments)
            {
                ClearMessage();
                return true;
            }

            FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));

            if (functionData.GenericArguments.Count != function.GenericArguments.Count)
            {
                SetErrorMessage(Strings.genericArgumentsNotConfigured);
                return false;
            }

            List<string> errors = new();
            if (!_functionGenericsConfigrationValidator.Validate(function, functionData.GenericArguments, Application, errors))
            {
                SetErrorMessage(string.Join(Environment.NewLine, errors));
                return false;
            }

            ClearMessage();
            return true;
        }

        private bool ValidateParameters()
        {
            List<string> errors = new();
            foreach (ParameterBase parameter in function.Parameters)
            {
                if (!_typeLoadHelper.TryGetSystemType(parameter, Application, out Type? _))
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionCannotLoadTypeForParameterFormat, parameter.Description, parameter.Name, function.Name));
            }

            if (errors.Count > 0)
                SetErrorMessage(string.Join(Environment.NewLine, errors));

            return errors.Count == 0;
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
