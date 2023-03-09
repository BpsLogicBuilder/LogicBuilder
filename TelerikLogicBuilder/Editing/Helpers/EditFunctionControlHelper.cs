using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class EditFunctionControlHelper : IEditFunctionControlHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionGenericsConfigrationValidator _functionGenericsConfigrationValidator;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;
        private readonly IUpdateParameterControlValues _updateParameterControlValues;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IEditFunctionControl editFunctionControl;

        public EditFunctionControlHelper(
            IConfigurationService configurationService,
            IFunctionDataParser functionDataParser,
            IFunctionGenericsConfigrationValidator functionGenericsConfigrationValidator,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            ITypeLoadHelper typeLoadHelper,
            IUpdateParameterControlValues updateParameterControlValues,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IEditFunctionControl editFunctionControl)
        {
            _configurationService = configurationService;
            _functionDataParser = functionDataParser;
            _functionGenericsConfigrationValidator = functionGenericsConfigrationValidator;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _updateParameterControlValues = updateParameterControlValues;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _typeLoadHelper = typeLoadHelper;
            this.editFunctionControl = editFunctionControl;
        }

        private static readonly string XmlParentXPath = $"/{XmlDataConstants.NOTELEMENT}|/{XmlDataConstants.FUNCTIONELEMENT}";
        private static readonly string ParametersXPath = $"{XmlParentXPath}/{XmlDataConstants.PARAMETERSELEMENT}";

        public XmlElement GetXmlResult(IDictionary<string, ParameterControlSet> editControlsSet)
        {
            FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(editFunctionControl.XmlDocument));
            string xmlString = _xmlDataHelper.BuildFunctionXml
            (
                editFunctionControl.Function.Name,
                functionData.VisibleText,
                _xmlDataHelper.BuildGenericArgumentsXml(functionData.GenericArguments),
                GetParametersXml()
            );

            return _refreshVisibleTextHelper.RefreshFunctionVisibleTexts
            (
                _xmlDocumentHelpers.ToXmlElement(xmlString),
                editFunctionControl.Application
            );

            string GetParametersXml()
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.PARAMETERSELEMENT);
                    foreach (ParameterBase parameter in editFunctionControl.Function.Parameters)
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

        public void UpdateParameterControls(TableLayoutPanel tableLayoutPanel, IDictionary<string, ParameterControlSet> editControlsSet)
        {
            editFunctionControl.ClearMessage();

            FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(editFunctionControl.XmlDocument));
            if (!_configurationService.FunctionList.Functions.ContainsKey(functionData.Name))
            {
                editFunctionControl.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionData.Name));
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

            bool isNewFunction = functionData.ParameterElementsList.Count == 0;
            _updateParameterControlValues.PrepopulateRequiredFields
            (
                editControlsSet,
                functionData.ParameterElementsList.ToDictionary(p => p.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)),
                editFunctionControl.Function.Parameters.ToDictionary(p => p.Name),
                editFunctionControl.XmlDocument,
                ParametersXPath,
                editFunctionControl.Application
            );

            if (isNewFunction)
            {
                _updateParameterControlValues.SetDefaultsForLiterals
                (
                    editControlsSet,
                    editFunctionControl.Function.Parameters.ToDictionary(p => p.Name)
                );//these are configured defaults for LiteralParameter and ListOfLiteralsParameter - different from prepopulating
            }
            else
            {
                _updateParameterControlValues.UpdateExistingFields
                (
                    functionData.ParameterElementsList,
                    editControlsSet,
                    editFunctionControl.Function.Parameters.ToDictionary(p => p.Name),
                    editFunctionControl.SelectedParameter
                );
            }
        }

        public bool ValidateGenericArgs()
        {
            if (!editFunctionControl.Function.HasGenericArguments)
            {
                editFunctionControl.ClearMessage();
                return true;
            }

            FunctionData functionData = _functionDataParser.Parse(_xmlDocumentHelpers.GetDocumentElement(editFunctionControl.XmlDocument));

            if (functionData.GenericArguments.Count != editFunctionControl.Function.GenericArguments.Count)
            {
                editFunctionControl.SetErrorMessage(Strings.genericArgumentsNotConfigured);
                return false;
            }

            List<string> errors = new();
            if (!_functionGenericsConfigrationValidator.Validate(editFunctionControl.Function, functionData.GenericArguments, editFunctionControl.Application, errors))
            {
                editFunctionControl.SetErrorMessage(string.Join(Environment.NewLine, errors));
                return false;
            }

            editFunctionControl.ClearMessage();
            return true;
        }

        public bool ValidateParameters()
        {
            List<string> errors = new();
            foreach (ParameterBase parameter in editFunctionControl.Function.Parameters)
            {
                if (!_typeLoadHelper.TryGetSystemType(parameter, editFunctionControl.Application, out Type? _))
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionCannotLoadTypeForParameterFormat, parameter.Description, parameter.Name, editFunctionControl.Function.Name));
            }

            if (errors.Count > 0)
                editFunctionControl.SetErrorMessage(string.Join(Environment.NewLine, errors));

            return errors.Count == 0;
        }
    }
}
