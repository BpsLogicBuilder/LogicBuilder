﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor
{
    internal partial class EditConstructorControl : UserControl, IEditConstructorControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IConstructorElementValidator _constructorElementValidator;
        private readonly IConstructorGenericsConfigrationValidator _constructorGenericsConfigrationValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IParameterFieldControlFactory _fieldControlFactory;
        private readonly IGenericConstructorHelper _genericConstructorHelper;
        private readonly ILoadParameterControlsDictionary _loadParameterControlsDictionary;
        private readonly IParameterElementValidator _parameterElementValidator;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITableLayoutPanelHelper _tableLayoutPanelHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IUpdateParameterControlValues _updateParameterControlValues;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IDataGraphEditingHost dataGraphEditingHost;
        
        private readonly Type assignedTo;
        private readonly Dictionary<string, ParameterControlSet> editControlsSet = [];
        private readonly XmlDocument xmlDocument;
        private readonly string? selectedParameter;

        private readonly RadGroupBox groupBoxConstructor;
        private readonly RadScrollablePanel radPanelConstructor;
        private readonly RadPanel radPanelTableParent;
        private readonly TableLayoutPanel tableLayoutPanel;
        private readonly RadLabel lblConstructor;
        private readonly RadLabel? lblGenericArguments;

        public EditConstructorControl(
            IConfigurationService configurationService,
            IConstructorDataParser constructorDataParser,
            IConstructorElementValidator constructorElementValidator,
            IConstructorGenericsConfigrationValidator constructorGenericsConfigrationValidator,
            IEditingControlHelperFactory editingControlFactory,
            IExceptionHelper exceptionHelper,
            IGenericConstructorHelper genericConstructorHelper,
            IParameterElementValidator parameterElementValidator,
            IParameterFieldControlFactory fieldControlFactory,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            ITableLayoutPanelHelper tableLayoutPanelHelper,
            ITypeLoadHelper typeLoadHelper,
            IUpdateParameterControlValues updateParameterControlValues,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingHost dataGraphEditingHost,
            Constructor constructor,
            Type assignedTo,
            XmlDocument formDocument,
            string treeNodeXPath,
            string? selectedParameter = null)
        {
            InitializeComponent();
            _configurationService = configurationService;
            _constructorDataParser = constructorDataParser;
            _constructorElementValidator = constructorElementValidator;
            _constructorGenericsConfigrationValidator = constructorGenericsConfigrationValidator;
            _exceptionHelper = exceptionHelper;
            _fieldControlFactory = fieldControlFactory;
            _genericConstructorHelper = genericConstructorHelper;
            _parameterElementValidator = parameterElementValidator;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _tableLayoutPanelHelper = tableLayoutPanelHelper;
            _typeLoadHelper = typeLoadHelper;
            _updateParameterControlValues = updateParameterControlValues;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
            this.constructor = constructor;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );

            this.assignedTo = assignedTo;
            this.selectedParameter = selectedParameter;

            _loadParameterControlsDictionary = editingControlFactory.GetLoadParameterControlsDictionary(this, dataGraphEditingHost);

            this.groupBoxConstructor = new RadGroupBox();
            this.radPanelConstructor = new RadScrollablePanel();
            this.radPanelTableParent = new RadPanel();
            this.tableLayoutPanel = new TableLayoutPanel();
            this.lblConstructor = new RadLabel();
            if (constructor.HasGenericArguments)
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

        private Constructor constructor;
        private readonly RadToolTip toolTip = new();

        private static readonly string XmlParentXPath = $"/{XmlDataConstants.CONSTRUCTORELEMENT}";
        private static readonly string ParametersXPath = $"{XmlParentXPath}/{XmlDataConstants.PARAMETERSELEMENT}";

        public bool IsValid
        {
            get
            {
                List<string> errors = new();
                errors.AddRange(ValidateControls());
                if (errors.Count > 0)
                    return false;

                _constructorElementValidator.Validate(XmlResult, assignedTo, Application, errors);
                if (errors.Count > 0)
                    return false;

                return true;
            }
        }

        public ApplicationTypeInfo Application => dataGraphEditingHost.Application;

        public Constructor Constructor => constructor;

        public bool DenySpecialCharacters => dataGraphEditingHost.DenySpecialCharacters;

        public bool DisplayNotCheckBox => dataGraphEditingHost.DisplayNotCheckBox;

        public XmlDocument XmlDocument => xmlDocument;

        public XmlElement XmlResult => GetXmlResult();

        public string VisibleText => XmlResult.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value;

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
            errors.AddRange(ValidateControls());
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));

            _constructorElementValidator.Validate(XmlResult, assignedTo, Application, errors);
            if (errors.Count > 0)
                throw new LogicBuilderException(string.Join(Environment.NewLine, errors));
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private static void CollapsePanelBorder(RadScrollablePanel radPanel)
            => radPanel.PanelElement.Border.Visibility = ElementVisibility.Collapsed;

        private XmlElement GetXmlResult()
        {
            ConstructorData constructorData = _constructorDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
            string xmlString = _xmlDataHelper.BuildConstructorXml
            (
                constructor.Name,
                constructorData.VisibleText,
                _xmlDataHelper.BuildGenericArgumentsXml(constructorData.GenericArguments),
                GetParametersXml()
            );

            return _refreshVisibleTextHelper.RefreshConstructorVisibleTexts
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
                    foreach (ParameterBase parameter in constructor.Parameters)
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
            ((ISupportInitialize)(this.groupBoxConstructor)).BeginInit();
            this.groupBoxConstructor.SuspendLayout();
            ((ISupportInitialize)(this.radPanelConstructor)).BeginInit();
            this.radPanelConstructor.PanelContainer.SuspendLayout();
            this.radPanelConstructor.SuspendLayout();
            ((ISupportInitialize)(this.radPanelTableParent)).BeginInit();
            this.radPanelTableParent.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            //this.SuspendLayout();
            ((ISupportInitialize)(this.lblConstructor)).BeginInit();
            if (constructor.HasGenericArguments)
            {
                ((ISupportInitialize)(this.lblGenericArguments!)).BeginInit();
            }
            // 
            // groupBoxConstructor
            // 
            this.groupBoxConstructor.AccessibleRole = AccessibleRole.Grouping;
            this.groupBoxConstructor.Controls.Add(this.radPanelConstructor);
            this.groupBoxConstructor.Dock = DockStyle.Fill;
            this.groupBoxConstructor.HeaderText = Strings.editConstructorGroupBoxHeaderText;
            this.groupBoxConstructor.Location = new Point(0, 0);
            this.groupBoxConstructor.Name = "groupBoxConstructor";
            this.groupBoxConstructor.Size = new Size(855, 300);
            this.groupBoxConstructor.TabIndex = 0;
            this.groupBoxConstructor.Text = Strings.editConstructorGroupBoxHeaderText;
            this.groupBoxConstructor.Padding = PerFontSizeConstants.GroupBoxPadding;
            // 
            // radPanelConstructor
            // 
            this.radPanelConstructor.Dock = DockStyle.Fill;
            this.radPanelConstructor.Location = new Point(2, 18);
            this.radPanelConstructor.Name = "radPanelConstructor";
            // 
            // radPanelConstructor.PanelContainer
            // 
            this.radPanelConstructor.PanelContainer.Controls.Add(this.radPanelTableParent);
            this.radPanelConstructor.PanelContainer.Size = new Size(849, 278);
            this.radPanelConstructor.Size = new Size(851, 280);
            this.radPanelConstructor.TabIndex = 0;
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

            int currentRow = 1;//constructor name row
            this.tableLayoutPanel.Controls.Add(this.lblConstructor, 3, currentRow);
            currentRow += 2;

            if (constructor.HasGenericArguments)
            {
                Control genericConfigurationControl = (Control)_fieldControlFactory.GetConstructorGenericParametersControl(this);
                genericConfigurationControl.Location = new Point(0, 0);
                genericConfigurationControl.Dock = DockStyle.Fill;
                genericConfigurationControl.Margin = new Padding(0);

                if (this.lblGenericArguments == null)
                    throw _exceptionHelper.CriticalException("{5056E060-7C8C-4906-8CBE-91FE2940494D}");
                this.tableLayoutPanel.Controls.Add(this.lblGenericArguments, 2, currentRow);
                this.tableLayoutPanel.Controls.Add(genericConfigurationControl, 3, currentRow);
                currentRow += 2;
            }

            
            if (constructor.HasGenericArguments)
            {
                if (ValidateGenericArgs())
                {
                    ConstructorData constructorData = _constructorDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
                    constructor = _genericConstructorHelper.ConvertGenericTypes
                    (
                        _configurationService.ConstructorList.Constructors[constructor.Name], //start with _configurationService.ConstructorList.Constructors to make sure we are starting with a generic type definition.
                        constructorData.GenericArguments, Application
                    );

                    LoadParameterControls();
                    UpdateParameterControls();
                }

                //We still need the layout for the genericConfigurationControl and lblConstructor
                //If ValidateGenericArgs(), then this must run after converting any generic parameters
                _tableLayoutPanelHelper.SetUp(tableLayoutPanel, radPanelTableParent, constructor.Parameters, constructor.HasGenericArguments);
                
            }
            else
            {
                LoadParameterControls();
                UpdateParameterControls();
                _tableLayoutPanelHelper.SetUp(tableLayoutPanel, radPanelTableParent, constructor.Parameters, constructor.HasGenericArguments);
            }

            void LoadParameterControls()
            {
                _loadParameterControlsDictionary.Load(editControlsSet, constructor.Parameters);
                foreach (ParameterBase parameter in constructor.Parameters)
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
            // lblConstructorName
            // 
            this.lblConstructor.Dock = DockStyle.Fill;
            this.lblConstructor.Location = new Point(0, 0);
            this.lblConstructor.Name = "lblConstructor";
            this.lblConstructor.Size = new Size(39, 18);
            this.lblConstructor.TabIndex = 0;
            this.lblConstructor.Text = constructor.Name;
            lblConstructor.TextAlignment = ContentAlignment.MiddleLeft;
            lblConstructor.Font = new Font
            (
                ForeColorUtility.GetDefaultFont(ThemeResolutionService.ApplicationThemeName),
                FontStyle.Bold
            );

            // 
            // EditConstructorControl
            // 
            this.AutoScaleDimensions = new SizeF(9F, 21F);
            this.AutoScaleMode = AutoScaleMode.None;
            this.Controls.Add(this.groupBoxConstructor);
            this.Name = "ConfigureConstructorControl";
            this.Size = new Size(855, 300);
            ((ISupportInitialize)(this.groupBoxConstructor)).EndInit();
            
            this.radPanelConstructor.PanelContainer.ResumeLayout(false);
            ((ISupportInitialize)(this.radPanelConstructor)).EndInit();
            this.radPanelConstructor.ResumeLayout(false);
            ((ISupportInitialize)(this.radPanelTableParent)).EndInit();
            this.radPanelTableParent.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.groupBoxConstructor.ResumeLayout(true);
            if (editControlsSet.Any())
            {//editControlsSet could be empty here if HasGenericArguments %% ValidateGenericArgs() == false
                foreach (ParameterBase parameter in constructor.Parameters)
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

            ((ISupportInitialize)(this.lblConstructor)).EndInit();
            if (constructor.HasGenericArguments)
            {
                ((ISupportInitialize)(this.lblGenericArguments!)).EndInit();
            }

            //this.ResumeLayout(false);

            CollapsePanelBorder(radPanelTableParent);
            CollapsePanelBorder(radPanelConstructor);

            this.Disposed += EditConstructorControl_Disposed;
        }

        private void RemoveCheckStateChangedHandlers()
        {
            foreach(var kvp in editControlsSet)
            {
                kvp.Value.ChkInclude.CheckStateChanged -= ChkInclude_CheckStateChanged;
            }
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

            ConstructorData constructorData = _constructorDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));
            if (!_configurationService.ConstructorList.Constructors.ContainsKey(constructorData.Name))
            {
                SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredFormat, constructorData.Name));
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

            radPanelConstructor.Enabled = true;

            bool newConstructor = constructorData.ParameterElementsList.Count == 0;
            _updateParameterControlValues.PrepopulateRequiredFields
            (
                editControlsSet,
                constructorData.ParameterElementsList.ToDictionary(p => p.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value),
                constructor.Parameters.ToDictionary(p => p.Name),
                this.xmlDocument,
                ParametersXPath,
                Application
            );

            //after updating parameter fields refresh constructorData - we'll need the updated ParameterElementsList.
            constructorData = _constructorDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(xmlDocument));

            if (newConstructor)
            {
                _updateParameterControlValues.SetDefaultsForLiterals
                (
                    editControlsSet, 
                    constructor.Parameters.ToDictionary(p => p.Name)
                );//these are configured defaults for LiteralParameter and ListOfLiteralsParameter - different from prepopulating
            }
            else
            {
                _updateParameterControlValues.UpdateExistingFields
                (
                    constructorData.ParameterElementsList,
                    editControlsSet,
                    constructor.Parameters.ToDictionary(p => p.Name),
                    selectedParameter
                );
            }
        }

        private IList<string> ValidateControls()
        {
            List<string> errors = new();
            foreach (ParameterBase parameter in constructor.Parameters)
            {
                if (!editControlsSet[parameter.Name].ChkInclude.Checked)
                    continue;

                if (editControlsSet[parameter.Name].ValueControl.IsEmpty)
                {//Need to validate for IsEmpty parameters before using the XmlElement below.
                    errors.Add
                    (
                        parameter.IsOptional
                            ? string.Format(CultureInfo.CurrentCulture, Strings.checkedOptionalParameterIsEmptyFormat, parameter.Name)
                            : string.Format(CultureInfo.CurrentCulture, Strings.requiredParameterIsEmptyFormat, parameter.Name)
                    );

                    editControlsSet[parameter.Name].ValueControl.SetErrorBackColor();
                    continue;
                }

                if (editControlsSet[parameter.Name].ValueControl.XmlElement == null)
                    throw _exceptionHelper.CriticalException("{240EE618-D74B-4F8F-BA94-BEFE45B781EF}");

                List<string> parameterErrors = new();
                _parameterElementValidator.Validate
                (
                    editControlsSet[parameter.Name].ValueControl.XmlElement!, 
                    parameter, 
                    dataGraphEditingHost.Application,
                    parameterErrors
                );

                if (parameterErrors.Count > 0 )
                {
                    errors.AddRange(parameterErrors);
                    editControlsSet[parameter.Name].ValueControl.SetErrorBackColor();
                }
                else
                {
                    editControlsSet[parameter.Name].ValueControl.SetNormalBackColor();
                }
            }

            return errors;
        }

        private bool ValidateGenericArgs()
        {
            if (!constructor.HasGenericArguments)
            {
                ClearMessage();
                return true;
            }

            ConstructorData constructorData = _constructorDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(XmlDocument));

            if (constructorData.GenericArguments.Count != constructor.GenericArguments.Count)
            {
                SetErrorMessage(Strings.genericArgumentsNotConfigured);
                return false;
            }

            List<string> errors = new();
            if (!_constructorGenericsConfigrationValidator.Validate(constructor, constructorData.GenericArguments, Application, errors))
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
            foreach(ParameterBase parameter in constructor.Parameters)
            {
                if (!_typeLoadHelper.TryGetSystemType(parameter, Application, out Type? _))
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.constructorCannotLoadTypeForParameterFormat, parameter.Description, parameter.Name, constructor.Name));
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

        private void EditConstructorControl_Disposed(object? sender, EventArgs e)
        {
            toolTip.RemoveAll();
            toolTip.Dispose();
            RemoveCheckStateChangedHandlers();
        }
        #endregion Event Handlers
    }
}
