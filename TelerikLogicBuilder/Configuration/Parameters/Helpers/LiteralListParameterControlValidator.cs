using ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.ConfigureLiteralListParameter;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.Parameters.Helpers
{
    internal partial class LiteralListParameterControlValidator : ILiteralListParameterControlValidator
    {
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IConfigureLiteralListParameterControl configureLiteralListParameterControl;

        private AutoCompleteRadDropDownList CmbListLpPropertySource => configureLiteralListParameterControl.CmbListLpPropertySource;
        private RadDropDownList CmbListLpElementControl => configureLiteralListParameterControl.CmbListLpElementControl;
        private RadDropDownList CmbListLpPropertySourceParameter => configureLiteralListParameterControl.CmbListLpPropertySourceParameter;
        private RadLabel LblListLpElementControl => configureLiteralListParameterControl.LblListLpElementControl;
        private RadLabel LblListLpName => configureLiteralListParameterControl.LblListLpName;
        private RadLabel LblListLpPropertySource => configureLiteralListParameterControl.LblListLpPropertySourceParameter;
        private RadLabel LblListLpPropertySourceParameter => configureLiteralListParameterControl.LblListLpPropertySourceParameter;
        private RadTextBox TxtListLpName => configureLiteralListParameterControl.TxtListLpName;

        private RadTreeView TreeView => configureLiteralListParameterControl.TreeView;
        private XmlDocument XmlDocument => configureLiteralListParameterControl.XmlDocument;

        public LiteralListParameterControlValidator(
            IParametersXmlParser parametersXmlParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IConfigureLiteralListParameterControl configureLiteralListParameterControl)
        {
            _parametersXmlParser = parametersXmlParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.configureLiteralListParameterControl = configureLiteralListParameterControl;
        }

        public void ValidateInputBoxes()
        {
            configureLiteralListParameterControl.ClearMessage();
            ValidateListLpName();
            ValidateListLpPropertySource((LiteralParameterInputStyle)CmbListLpElementControl.SelectedValue);
            ValidateListLpPropertySourceParameter
            (
                (LiteralParameterInputStyle)CmbListLpElementControl.SelectedValue,
                _xmlDocumentHelpers.GetSiblingParameterElements
                (
                    _xmlDocumentHelpers.SelectSingleElement(XmlDocument, TreeView.SelectedNode.Name)
                )
                .Select(e => _parametersXmlParser.Parse(e).Name)
                .ToHashSet()
            );
        }

        private void ValidateListLpName()
        {
            if (!XmlAttributeRegex().IsMatch(TxtListLpName.Text))
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidAttributeFormat, LblListLpName.Text));
            }
        }

        private void ValidateListLpPropertySource(LiteralParameterInputStyle inputStyle)
        {
            if (inputStyle == LiteralParameterInputStyle.PropertyInput && !FullyQulifiedClassNameMRegex().IsMatch(CmbListLpPropertySource.Text))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, LblListLpPropertySource.Text));

            if (inputStyle != LiteralParameterInputStyle.PropertyInput && CmbListLpPropertySource.Text.Trim().Length > 0)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, LblListLpPropertySource.Text, LblListLpElementControl.Text, Strings.dropdownTextPropertyInput));
        }

        private void ValidateListLpPropertySourceParameter(LiteralParameterInputStyle inputStyle, HashSet<string> siblingNames)
        {
            if (inputStyle == LiteralParameterInputStyle.ParameterSourcedPropertyInput)
            {
                if (!siblingNames.Contains(CmbListLpPropertySourceParameter.Text.Trim()))
                    throw new LogicBuilderException
                    (
                        string.Format
                        (
                            CultureInfo.CurrentCulture, 
                            Strings.cannotLoadPropertySourceParameterFormat2,
                            LblListLpPropertySourceParameter.Text,
                            CmbListLpPropertySourceParameter.Text,
                            string.Join(Strings.itemsCommaSeparator, siblingNames),
                            LblListLpElementControl.Text,
                            Strings.dropdownTextParameterSourcedPropertyInput
                        )
                    );
            }

            if (inputStyle != LiteralParameterInputStyle.ParameterSourcedPropertyInput && CmbListLpPropertySourceParameter.Text.Trim().Length > 0)
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, LblListLpPropertySourceParameter.Text, LblListLpElementControl.Text, Strings.dropdownTextParameterSourcedPropertyInput));
        }

        [GeneratedRegex(RegularExpressions.FULLYQUALIFIEDCLASSNAME)]
        private static partial Regex FullyQulifiedClassNameMRegex();
        [GeneratedRegex(RegularExpressions.XMLATTRIBUTE)]
        private static partial Regex XmlAttributeRegex();
    }
}
