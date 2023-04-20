using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueToNullFunction
{
    internal partial class EditSetValueToNullFunctionControl : UserControl, IEditSetValueToNullFunctionControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEditSetValueFunctionCommandFactory _editSetValueFunctionCommandFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IRetractFunctionDataParser _retractFunctionDataParser;
        private readonly IRetractFunctionElementValidator _retractFunctionElementValidator;
        private readonly ISetValueFunctionTableLayoutPanelHelper _tableLayoutPanelHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingHost dataGraphEditingHost;
        private readonly Function function;
        private VariableBase? variable;
        private readonly XmlDocument xmlDocument;

        public EditSetValueToNullFunctionControl(
            IConfigurationService configurationService,
            IEditSetValueFunctionCommandFactory editSetValueFunctionCommandFactory,
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IRetractFunctionDataParser retractFunctionDataParser,
            IRetractFunctionElementValidator retractFunctionElementValidator,
            ISetValueFunctionTableLayoutPanelHelper tableLayoutPanelHelper,
            ITypeLoadHelper typeLoadHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost,
            Function function,
            XmlDocument formDocument,
            string treeNodeXPath)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _editSetValueFunctionCommandFactory = editSetValueFunctionCommandFactory;
            _exceptionHelper = exceptionHelper;
            _radDropDownListHelper = radDropDownListHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _retractFunctionDataParser = retractFunctionDataParser;
            _retractFunctionElementValidator = retractFunctionElementValidator;
            _tableLayoutPanelHelper = tableLayoutPanelHelper;
            _typeLoadHelper = typeLoadHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
            this.function = function;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );

            Initialize();
        }

        public Function Function => function;

        public XmlDocument XmlDocument => xmlDocument;

        public bool DenySpecialCharacters => dataGraphEditingHost.DenySpecialCharacters;

        public bool DisplayNotCheckBox => dataGraphEditingHost.DisplayNotCheckBox;

        public ApplicationTypeInfo Application => dataGraphEditingHost.Application;

        public bool IsValid
        {
            get
            {
                List<string> errors = new();
                errors.AddRange(ValidateControls());
                return errors.Count == 0;
            }
        }

        public XmlElement XmlResult
        {
            get
            {
                if (variable == null)
                    throw _exceptionHelper.CriticalException("{85C5D6DF-C9F4-45C0-85D9-36937FE0EFAF}");

                return _refreshVisibleTextHelper.RefreshSetValueToNullFunctionVisibleTexts
                (
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _xmlDataHelper.BuildRetractFunctionXml
                        (
                            function.Name,
                            function.Name,
                            variable.Name
                        )
                    )
                );
            }
        }

        public string VisibleText => XmlResult.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value;

        public void ClearMessage() => dataGraphEditingHost.ClearMessage();

        public void RequestDocumentUpdate() => dataGraphEditingHost.RequestDocumentUpdate(this);

        public void ResetControls()
        {
            Native.NativeMethods.LockWindowUpdate(this.Handle);
            variable = null;
            SetVariableName();
            RemoveValueControl();
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        public void SetErrorMessage(string message) => dataGraphEditingHost.SetErrorMessage(message);

        public void SetMessage(string message, string title = "") => dataGraphEditingHost.SetMessage(message, title);

        public void ValidateFields()
        {
            List<string> errors = new();
            errors.AddRange(ValidateControls());
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));
        }

        private static void AddClickCommand(HelperButtonDropDownList helperButtonDropDownList, IClickCommand command)
        {
            helperButtonDropDownList.ButtonClick += (sender, args) => command.Execute();
        }

        private void CmbSelectVariableChanged()
        {
            if (!_configurationService.VariableList.Variables.TryGetValue(cmbSelectVariable.Text, out variable))
            {
                RemoveValueControl();
                return;
            }

            Native.NativeMethods.LockWindowUpdate(this.Handle);
            RemoveValueControl();
            SetValueControls();
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private RadLabel GetVariableTypeLabel()
        {
            if (variable == null)
                throw _exceptionHelper.CriticalException("{2B94E19C-274E-4584-9617-2C859FFD5CF6}");

            RadToolTip toolTip = new();
            RadLabel label = new()
            {
                Dock = DockStyle.Top,
                Image = GetLabelImage(variable.VariableTypeCategory)
            };
            if (variable.Comments.Trim().Length > 0)
                toolTip.SetToolTip(label, variable.Comments);

            return label;

            Image GetLabelImage(VariableTypeCategory variableTypeCategory)
            {
                return variableTypeCategory switch
                {
                    VariableTypeCategory.Literal => Properties.Resources.LiteralParameter,
                    VariableTypeCategory.Object => Properties.Resources.ObjectParameter,
                    VariableTypeCategory.LiteralList => Properties.Resources.LiteralListParameter,
                    VariableTypeCategory.ObjectList => Properties.Resources.ObjectListParameter,
                    _ => throw _exceptionHelper.CriticalException("{8599601C-F878-4697-B918-6EE2F038A597}"),
                };
            }
        }

        private void Initialize()
        {
            RetractFunctionData retractFunctionData = _retractFunctionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(this.xmlDocument));
            _configurationService.VariableList.Variables.TryGetValue
            (
                retractFunctionData.VariableElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                out variable
            );

            SetupTableControls();
            LoadVariablesDropDown();
            SetVariableName();
            SetValueControls();

            AddClickCommand(cmbSelectVariable, _editSetValueFunctionCommandFactory.GetSelectVariableCommand(cmbSelectVariable));
            cmbSelectVariable.Changed -= CmbSelectVariable_Changed;
            cmbSelectVariable.Changed += CmbSelectVariable_Changed;

            this.lblFunction.Text = function.Name;
            lblFunction.TextAlignment = ContentAlignment.MiddleLeft;
            lblFunction.Font = new Font
            (
                ForeColorUtility.GetDefaultFont(ThemeResolutionService.ApplicationThemeName),
                FontStyle.Bold
            );

            CollapsePanelBorder(radPanelImageLabel);
            CollapsePanelBorder(radPanelTableParent);
            CollapsePanelBorder(radPanelFunction);
            CollapsePanelBorder(radPanelSelectVariable);
        }

        private void LoadVariablesDropDown()
        {
            IList<string> variables = _configurationService.VariableList.Variables.Keys.Order().ToArray();
            if (variables.Count > 0)
                _radDropDownListHelper.LoadTextItems(cmbSelectVariable.DropDownList, variables, RadDropDownStyle.DropDown);
        }

        private void RemoveValueControl()
        {
            RemoveAndDispose(radPanelImageLabel);

            static void RemoveAndDispose(RadPanel radPanel)
            {
                foreach (Control control in radPanel.Controls)
                {
                    control.Visible = false;
                    if (!control.IsDisposed)
                        control.Dispose();
                }
                radPanel.Controls.Clear();
            }
        }

        private void SetupTableControls()
        {
            groupBoxFunction.Padding = PerFontSizeConstants.GroupBoxPadding;
            radPanelImageLabel.Padding = PerFontSizeConstants.ImageLabelPadding;

            _tableLayoutPanelHelper.SetUp(tableLayoutPanel, radPanelTableParent, false);
        }

        private void SetValueControls()
        {
            if (variable != null)
            {
                radPanelImageLabel.Controls.Add(GetVariableTypeLabel());
            }
        }

        private void SetVariableName()
        {
            cmbSelectVariable.Changed -= CmbSelectVariable_Changed;
            cmbSelectVariable.Text = variable?.Name ?? string.Empty;
            cmbSelectVariable.Changed += CmbSelectVariable_Changed;
        }

        private IList<string> ValidateControls()
        {
            List<string> errors = new();
            if (!_configurationService.VariableList.Variables.TryGetValue(cmbSelectVariable.Text, out variable))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.decisionNotConfiguredFormat2, cmbSelectVariable.Text));
                cmbSelectVariable.SetErrorBackColor();
                return errors;
            }
            cmbSelectVariable.SetNormalBackColor();

            if (!_typeLoadHelper.TryGetSystemType(variable, Application, out Type? _))
            {
                errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForVariableFormat, variable.ObjectTypeString, variable.Name));
                return errors;
            }

            _retractFunctionElementValidator.Validate(XmlResult, Application, errors);
            if (errors.Count > 0)
            {
                cmbSelectVariable.SetErrorBackColor();
            }
            else
            {
                cmbSelectVariable.SetNormalBackColor();
            }

            return errors;
        }

        #region Event Handlers
        private void CmbSelectVariable_Changed(object? sender, EventArgs e) => CmbSelectVariableChanged();
        #endregion Event Handlers
    }
}
