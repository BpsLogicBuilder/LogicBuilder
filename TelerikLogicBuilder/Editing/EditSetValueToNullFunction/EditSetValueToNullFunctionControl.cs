﻿using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueToNullFunction
{
    internal partial class EditSetValueToNullFunctionControl : UserControl, IEditSetValueToNullFunctionControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionGenericsConfigrationValidator _functionGenericsConfigrationValidator;
        private readonly IFieldControlFactory _fieldControlFactory;
        private readonly IGenericFunctionHelper _genericFunctionHelper;
        private readonly ILoadParameterControlsDictionary _loadParameterControlsDictionary;
        private readonly ITableLayoutPanelHelper _tableLayoutPanelHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IUpdateParameterControlValues _updateParameterControlValues;
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
        private readonly RadLabel lblFunction;
        private readonly RadLabel? lblGenericArguments;

        public EditSetValueToNullFunctionControl(
            IConfigurationService configurationService,
            IFunctionDataParser functionDataParser,
            IFunctionGenericsConfigrationValidator functionGenericsConfigrationValidator,
            IEditingControlHelperFactory editingControlFactory,
            IFieldControlFactory fieldControlFactory,
            IGenericFunctionHelper genericFunctionHelper,
            ITableLayoutPanelHelper tableLayoutPanelHelper,
            ITypeLoadHelper typeLoadHelper,
            IUpdateParameterControlValues updateParameterControlValues,
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
            _functionGenericsConfigrationValidator = functionGenericsConfigrationValidator;
            _fieldControlFactory = fieldControlFactory;
            _genericFunctionHelper = genericFunctionHelper;
            _tableLayoutPanelHelper = tableLayoutPanelHelper;
            _typeLoadHelper = typeLoadHelper;
            _updateParameterControlValues = updateParameterControlValues;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingForm = dataGraphEditingForm;
            this.function = function;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );

            this.assignedTo = assignedTo;
            this.selectedParameter = selectedParameter;

            _loadParameterControlsDictionary = editingControlFactory.GetLoadParameterControlsDictionary(this, dataGraphEditingForm);

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

        public bool DenySpecialCharacters => dataGraphEditingForm.DenySpecialCharacters;

        public bool DisplayNotCheckBox => dataGraphEditingForm.DisplayNotCheckBox;

        public Function Function => function;

        public XmlDocument XmlDocument => throw new NotImplementedException();

        public XmlElement XmlResult => throw new NotImplementedException();

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public bool IsValid => throw new NotImplementedException();

        public string? SelectedParameter => selectedParameter;

        public void ClearMessage()
        {
            throw new NotImplementedException();
        }

        public void RequestDocumentUpdate()
        {
            throw new NotImplementedException();
        }

        public void ResetControls()
        {
            throw new NotImplementedException();
        }

        public void SetErrorMessage(string message)
        {
            throw new NotImplementedException();
        }

        public void SetMessage(string message, string title = "")
        {
            throw new NotImplementedException();
        }

        public void ValidateFields()
        {
        }

        private void InitializeControls()
        {
            radTextBox1.Text = Function.Name;
        }
    }
}
