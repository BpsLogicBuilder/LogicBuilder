using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralParameter;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers
{
    internal partial class LiteralParameterControlValidator : ILiteralParameterControlValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly ITypeHelper _typeHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureLiteralParameterControl configureLiteralParameterControl;

        private RadDropDownList CmbLpControl => configureLiteralParameterControl.CmbLpControl;
        private RadDropDownList CmbLpLiteralType => configureLiteralParameterControl.CmbLpLiteralType;
        private AutoCompleteRadDropDownList CmbLpPropertySource => configureLiteralParameterControl.CmbLpPropertySource;
        private RadDropDownList CmbLpPropertySourceParameter => configureLiteralParameterControl.CmbLpPropertySourceParameter;
        private RadLabel LblLpControl => configureLiteralParameterControl.LblLpControl;
        private RadLabel LblLpDefaultValue => configureLiteralParameterControl.LblLpDefaultValue;
        private RadLabel LblLpName => configureLiteralParameterControl.LblLpName;
        private RadLabel LblLpPropertySource => configureLiteralParameterControl.LblLpPropertySource;
        private RadLabel LblLpPropertySourceParameter => configureLiteralParameterControl.LblLpPropertySourceParameter;
        private RadTextBox TxtLpDefaultValue => configureLiteralParameterControl.TxtLpDefaultValue;
        private RadTextBox TxtLpName => configureLiteralParameterControl.TxtLpName;

        private RadTreeView TreeView => configureLiteralParameterControl.TreeView;
        private XmlDocument XmlDocument => configureLiteralParameterControl.XmlDocument;

        public LiteralParameterControlValidator(
            IEnumHelper enumHelper,
            IParametersXmlParser parametersXmlParser,
            ITypeHelper typeHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureLiteralParameterControl configureLiteralParameterControl)
        {
            _enumHelper = enumHelper;
            _parametersXmlParser = parametersXmlParser;
            _typeHelper = typeHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureLiteralParameterControl = configureLiteralParameterControl;
        }

        public void ValidateInputBoxes()
        {
            configureLiteralParameterControl.ClearMessage();
            ValidateLpName();
            ValidateLpPropertySource((LiteralParameterInputStyle)CmbLpControl.SelectedValue);
            ValidateLpPropertySourceParameter
            (
                (LiteralParameterInputStyle)CmbLpControl.SelectedValue,
                _xmlDocumentHelpers.GetSiblingParameterElements
                (
                    _xmlDocumentHelpers.SelectSingleElement(XmlDocument, TreeView.SelectedNode.Name)
                )
                .Select(e => _parametersXmlParser.Parse(e).Name)
                .ToHashSet()
            );

            ValidateLpDefaultValue
            (
                _enumHelper.GetSystemType((LiteralParameterType)CmbLpLiteralType.SelectedValue)
            );
        }

        private void ValidateLpDefaultValue(Type type)
        {
            if (!string.IsNullOrEmpty(TxtLpDefaultValue.Text) && !_typeHelper.TryParse(TxtLpDefaultValue.Text, type, out object? _))
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidDomainFormat, LblLpDefaultValue.Text, TxtLpDefaultValue.Text, type.Name));
            }
        }

        private void ValidateLpName()
        {
            if (!XmlAttributeRegex().IsMatch(TxtLpName.Text))
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidAttributeFormat, LblLpName.Text));
            }
        }

        private void ValidateLpPropertySource(LiteralParameterInputStyle inputStyle)
        {
            if (inputStyle == LiteralParameterInputStyle.PropertyInput && !FullyQulifiedClassNameMRegex().IsMatch(CmbLpPropertySource.Text))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, LblLpPropertySource.Text));

            if (inputStyle != LiteralParameterInputStyle.PropertyInput && CmbLpPropertySource.Text.Trim().Length > 0)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, LblLpPropertySource.Text, LblLpControl.Text, Strings.dropdownTextPropertyInput));
        }

        private void ValidateLpPropertySourceParameter(LiteralParameterInputStyle inputStyle, HashSet<string> siblingNames)
        {
            if (inputStyle == LiteralParameterInputStyle.ParameterSourcedPropertyInput)
            {
                if (!siblingNames.Contains(CmbLpPropertySourceParameter.Text.Trim()))
                    throw new LogicBuilderException
                    (
                        string.Format
                        (
                            CultureInfo.CurrentCulture, Strings.cannotLoadPropertySourceParameterFormat2,
                            LblLpPropertySourceParameter.Text,
                            CmbLpPropertySourceParameter.Text,
                            string.Join(Strings.itemsCommaSeparator, siblingNames),
                            LblLpControl.Text,
                            Strings.dropdownTextParameterSourcedPropertyInput
                        )
                    );
            }

            if (inputStyle != LiteralParameterInputStyle.ParameterSourcedPropertyInput && CmbLpPropertySourceParameter.Text.Trim().Length > 0)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, LblLpPropertySourceParameter.Text, LblLpControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
        }

        [GeneratedRegex(RegularExpressions.FULLYQUALIFIEDCLASSNAME)]
        private static partial Regex FullyQulifiedClassNameMRegex();
        [GeneratedRegex(RegularExpressions.XMLATTRIBUTE)]
        private static partial Regex XmlAttributeRegex();
    }
}
