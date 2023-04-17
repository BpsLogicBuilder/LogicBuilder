using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
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
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction
{
    internal partial class EditSetValueFunctionControl : UserControl, IEditSetValueFunctionControl
    {
        private readonly IAssertFunctionDataParser _assertFunctionDataParser;
        private readonly IAssertFunctionElementValidator _assertFunctionElementValidator;
        private readonly IConfigurationService _configurationService;
        private readonly IEditSetValueFunctionCommandFactory _editSetValueFunctionCommandFactory;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ISetValueFunctionTableLayoutPanelHelper _tableLayoutPanelHelper;
        private readonly IServiceFactory _serviceFactory;
        private readonly IVariableValueControlFactory _variableValueControlFactory;
        private readonly IVariableValueDataParser _variableValueDataParser;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingHost dataGraphEditingHost;
        private readonly Function function;
        private VariableBase? variable;
        private readonly XmlDocument xmlDocument;

        public EditSetValueFunctionControl(
            IAssertFunctionDataParser assertFunctionDataParser,
            IAssertFunctionElementValidator assertFunctionElementValidator,
            IConfigurationService configurationService,
            IEditSetValueFunctionCommandFactory editSetValueFunctionCommandFactory,
            IExceptionHelper exceptionHelper,
            IRadDropDownListHelper radDropDownListHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            ISetValueFunctionTableLayoutPanelHelper tableLayoutPanelHelper,
            IServiceFactory serviceFactory,
            IVariableValueControlFactory variableValueControlFactory,
            IVariableValueDataParser variableValueDataParser,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost,
            Function function,
            XmlDocument formDocument,
            string treeNodeXPath)
        {
            InitializeComponent();
            _assertFunctionDataParser = assertFunctionDataParser;
            _assertFunctionElementValidator = assertFunctionElementValidator;
            _configurationService = configurationService;
            _editSetValueFunctionCommandFactory = editSetValueFunctionCommandFactory;
            _exceptionHelper = exceptionHelper;
            _radDropDownListHelper = radDropDownListHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _serviceFactory = serviceFactory;
            _tableLayoutPanelHelper = tableLayoutPanelHelper;
            _variableValueControlFactory = variableValueControlFactory;
            _variableValueDataParser = variableValueDataParser;
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

        private IValueControl? _valueControl;

        private IValueControl ValueControl => _valueControl ?? throw _exceptionHelper.CriticalException("{AFA3D6FD-CAE9-497C-9317-13F29828FDBB}");

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
                    throw _exceptionHelper.CriticalException("{3917FCD8-32C8-4E91-8C54-44DC9231D30F}");

                return _refreshVisibleTextHelper.RefreshSetValueFunctionVisibleTexts
                (
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _xmlDataHelper.BuildAssertFunctionXml
                        (
                            function.Name,
                            function.Name,
                            variable.Name,
                            ValueControl.XmlElement?.OuterXml ?? throw _exceptionHelper.CriticalException("{AA84B882-B6C2-4E20-8134-EFDE01502921}")
                        )
                    ),
                    Application
                );
            }
        }

        public string VisibleText => XmlResult.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE);

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

            if (ValueControl.IsEmpty)
            {
                ValueControl.SetErrorBackColor();
                errors.Add(Strings.variableValueRequired);
                return errors;
            }
            ValueControl.SetNormalBackColor();

            _assertFunctionElementValidator.Validate(XmlResult, Application, errors);
            if (errors.Count > 0)
            {
                cmbSelectVariable.SetErrorBackColor();
                ValueControl.SetErrorBackColor();
            }
            else
            {
                cmbSelectVariable.SetNormalBackColor();
                ValueControl.SetNormalBackColor();
            }

            return errors;
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
            LayoutTableControls();
            SetValueControls();
            Native.NativeMethods.LockWindowUpdate(IntPtr.Zero);
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private IValueControl GetValueControl()
        {
            if (variable == null)
                throw _exceptionHelper.CriticalException("{E5C54A2C-9342-40AB-AD73-86A116B215F3}");

            if (variable is LiteralVariable literalVariable)
            {
                switch (literalVariable.Control)
                {
                    case LiteralVariableInputStyle.DomainAutoComplete:
                        _valueControl = _variableValueControlFactory.GetLiteralVariableDomainAutoCompleteControl(this, literalVariable);
                        break;
                    case LiteralVariableInputStyle.DropDown:
                        _valueControl = _variableValueControlFactory.GetLiteralVariableDropDownListControl(this, literalVariable);
                        break;
                    case LiteralVariableInputStyle.MultipleLineTextBox:
                        _valueControl = literalVariable.Domain.Any()
                                            ? _variableValueControlFactory.GetLiteralVariableDomainMultilineControl(this, literalVariable)
                                            : _variableValueControlFactory.GetLiteralVariableMultilineControl(this, literalVariable);
                        break;
                    case LiteralVariableInputStyle.TypeAutoComplete:
                        ILiteralVariableTypeAutoCompleteControl typeAutoCompleteControl = _variableValueControlFactory.GetLiteralVariableTypeAutoCompleteControl(this, literalVariable);
                        ITypeAutoCompleteManager typeAutoCompleteManager = _serviceFactory.GetTypeAutoCompleteManager(dataGraphEditingHost, typeAutoCompleteControl);
                        typeAutoCompleteManager.Setup();
                        _valueControl = typeAutoCompleteControl;
                        break;
                    case LiteralVariableInputStyle.PropertyInput:
                        _valueControl = _variableValueControlFactory.GetLiteralVariablePropertyInputRichInputBoxControl(this, literalVariable);
                        break;
                    case LiteralVariableInputStyle.SingleLineTextBox:
                        _valueControl = literalVariable.Domain.Any()
                                            ? _variableValueControlFactory.GetLiteralVariableDomainRichInputBoxControl(this, literalVariable)
                                            : _variableValueControlFactory.GetLiteralVariableRichInputBoxControl(this, literalVariable);
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{6B95FAF1-2264-4AF9-8B50-2DD207F3C7F0}");
                }
            }
            else if (variable is ObjectVariable objectVariable)
            {
                _valueControl = _variableValueControlFactory.GetObjectVariableRichTextBoxControl(this, objectVariable);
            }
            else if (variable is ListOfLiteralsVariable listOfLiteralsVariable)
            {
                _valueControl = _variableValueControlFactory.GetiteralListVariableRichTextBoxControl(this, listOfLiteralsVariable);
            }
            else if (variable is ListOfObjectsVariable listOfObjectsVariable)
            {
                _valueControl = _variableValueControlFactory.GetObjectListVariableRichTextBoxControl(this, listOfObjectsVariable);
            }

            if (_valueControl == null)
                throw _exceptionHelper.CriticalException("{863C1E05-BB0C-4773-AF4C-221849B4B819}");

            _valueControl.Location = new Point(0, 0);
            _valueControl.Dock = DockStyle.Fill;
            _valueControl.Margin = new Padding(0);
            return _valueControl;
        }

        private RadLabel GetVariableTypeLabel()
        {
            if (variable == null)
                throw _exceptionHelper.CriticalException("{54C5B13B-154F-48F3-B49F-7EE2D0F69494}");

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
                    _ => throw _exceptionHelper.CriticalException("{A3FF8507-6C3F-4755-B785-1F5DA72D993F}"),
                };
            }
        }


        private void Initialize()
        {
            AssertFunctionData assertFunctionData = _assertFunctionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(this.xmlDocument));
            _configurationService.VariableList.Variables.TryGetValue
            (
                assertFunctionData.VariableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                out variable
            );

            SetupTableControls();
            LoadVariablesDropDown();
            SetVariableName();
            SetValueControls();
            UpdateValueControl(assertFunctionData);

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

        private void LayoutTableControls()
        {
            radPanelFunction.PanelContainer.SuspendLayout();
            ((ISupportInitialize)radPanelTableParent).BeginInit();
            radPanelTableParent.SuspendLayout();
            tableLayoutPanel.SuspendLayout();

            SetupTableControls();

            ((ISupportInitialize)radPanelTableParent).EndInit();
            radPanelTableParent.ResumeLayout(false);
            tableLayoutPanel.ResumeLayout(false);
            tableLayoutPanel.PerformLayout();
            radPanelFunction.PanelContainer.ResumeLayout(true);
        }

        private void LoadVariablesDropDown()
        {
            IList<string> variables = _configurationService.VariableList.Variables.Keys.Order().ToArray();
            if (variables.Count > 0)
                _radDropDownListHelper.LoadTextItems(cmbSelectVariable.DropDownList, variables, RadDropDownStyle.DropDown);
        }

        private void RemoveValueControl()
        {
            RemoveAndDispose(radPanelValueControl);
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
            bool isMultiLine = variable != null
                                && variable is LiteralVariable literalVariable
                                && literalVariable.Control == LiteralVariableInputStyle.MultipleLineTextBox;

            groupBoxFunction.Padding = PerFontSizeConstants.GroupBoxPadding;
            radPanelImageLabel.Padding = PerFontSizeConstants.ImageLabelPadding;

            radPanelSelectVariable.Dock = isMultiLine ? DockStyle.Top : DockStyle.Fill;
            radPanelSelectVariable.Size = new Size(224, (int)PerFontSizeConstants.SingleLineHeight);
            radPanelSelectVariable.Padding = PerFontSizeConstants.SelectVariableCellPadding;

            _tableLayoutPanelHelper.SetUp(tableLayoutPanel, radPanelTableParent, isMultiLine);
        }

        private void SetValueControls()
        {
            if (variable != null)
            {
                radPanelImageLabel.Controls.Add(GetVariableTypeLabel());
                radPanelValueControl.Controls.Add((Control)GetValueControl());
            }
        }

        private void SetVariableName()
        {
            cmbSelectVariable.Changed -= CmbSelectVariable_Changed;
            cmbSelectVariable.Text = variable?.Name ?? string.Empty;
            cmbSelectVariable.Changed += CmbSelectVariable_Changed;
        }

        private void UpdateValueControl(AssertFunctionData assertFunctionData)
        {
            if (variable == null)
                return;

            VariableValueData variableValueData = _variableValueDataParser.Parse(assertFunctionData.VariableValueElement);

            if (variable.VariableTypeCategory != variableValueData.ChildElementCategory)
                return;

            ValueControl.Update(variableValueData.ChildElement);
        }

        #region Event Handlers
        private void CmbSelectVariable_Changed(object? sender, EventArgs e) => CmbSelectVariableChanged();
        #endregion Event Handlers
    }
}
