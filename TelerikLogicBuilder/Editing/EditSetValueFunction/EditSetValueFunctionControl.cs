using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditSetValueFunction
{
    internal partial class EditSetValueFunctionControl : UserControl, IEditSetValueFunctionControl
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionGenericsConfigrationValidator _functionGenericsConfigrationValidator;
        private readonly IGenericFunctionHelper _genericFunctionHelper;
        private readonly ILoadParameterControlsDictionary _loadParameterControlsDictionary;
        private readonly IParameterFieldControlFactory _fieldControlFactory;
        private readonly ITableLayoutPanelHelper _tableLayoutPanelHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IUpdateParameterControlValues _updateParameterControlValues;
        private readonly IXmlDataHelper _xmlDataHelper;
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
        private readonly RadLabel lblFunction;
        private readonly RadLabel? lblGenericArguments;

        public EditSetValueFunctionControl(
            IConfigurationService configurationService,
            IFunctionDataParser functionDataParser,
            IFunctionGenericsConfigrationValidator functionGenericsConfigrationValidator,
            IEditingControlHelperFactory editingControlFactory,
            IGenericFunctionHelper genericFunctionHelper,
            IParameterFieldControlFactory fieldControlFactory,
            ITableLayoutPanelHelper tableLayoutPanelHelper,
            ITypeLoadHelper typeLoadHelper,
            IUpdateParameterControlValues updateParameterControlValues,
            IXmlDataHelper xmlDataHelper,
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
            _functionDataParser = functionDataParser;
            _functionGenericsConfigrationValidator = functionGenericsConfigrationValidator;
            _fieldControlFactory = fieldControlFactory;
            _genericFunctionHelper = genericFunctionHelper;
            _tableLayoutPanelHelper = tableLayoutPanelHelper;
            _typeLoadHelper = typeLoadHelper;
            _updateParameterControlValues = updateParameterControlValues;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingHost = dataGraphEditingHost;
            this.function = function;
            this.xmlDocument = _xmlDocumentHelpers.ToXmlDocument
            (
                _xmlDocumentHelpers.SelectSingleElement(formDocument, treeNodeXPath)
            );

            this.assignedTo = assignedTo;
            this.selectedParameter = selectedParameter;

            _loadParameterControlsDictionary = editingControlFactory.GetLoadParameterControlsDictionary(this, dataGraphEditingHost);

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

        public Function Function => function;

        public bool DenySpecialCharacters => dataGraphEditingHost.DenySpecialCharacters;

        public bool DisplayNotCheckBox => dataGraphEditingHost.DisplayNotCheckBox;

        public XmlDocument XmlDocument => throw new NotImplementedException();

        public XmlElement XmlResult
        {
            get
            {
                return this.xmlDocument.DocumentElement!;
                //VariableBase variableBase = (LiteralVariable)_configurationService.VariableList.Variables.First(kvp => kvp.Value.VariableTypeCategory == Enums.VariableTypeCategory.Literal && ((LiteralVariable)kvp.Value).LiteralType == Enums.LiteralVariableType.Integer).Value;
                //Function function = _configurationService.FunctionList.Functions.First(kvp => kvp.Value.FunctionCategory == Enums.FunctionCategories.Assert).Value;
                //return _xmlDocumentHelpers.ToXmlElement
                //(
                //    _xmlDataHelper.BuildAssertFunctionXml
                //    (
                //        function.Name,
                //        function.Name,
                //        variableBase.Name,
                //        "<literalVariable>2</literalVariable>"
                //    )
                //);
            }
        }

        public string VisibleText => XmlResult.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE);

        public ApplicationTypeInfo Application => throw new NotImplementedException();

        public bool IsValid => true;

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
