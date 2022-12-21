using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal class LiteralVariableControlsValidator : ILiteralVariableControlsValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly IVariableControlsValidator _variableControlsValidator;
        private readonly IConfigureLiteralVariableControl configureLiteralVariableControl;

        public LiteralVariableControlsValidator(
            IEnumHelper enumHelper,
            ITypeHelper typeHelper,
            IVariableControlValidatorFactory variableControlHelperFactory,
            IConfigureLiteralVariableControl configureLiteralVariableControl)
        {
            _enumHelper = enumHelper;
            _typeHelper = typeHelper;
            _variableControlsValidator = variableControlHelperFactory.GetVariableControlsValidator(configureLiteralVariableControl);
            this.configureLiteralVariableControl = configureLiteralVariableControl;
        }

        public void CmbReferenceDefinitionValidating()
        {
            _variableControlsValidator.CmbReferenceDefinitionValidating();
        }

        public void TxtCastReferenceAsValidating()
        {
            _variableControlsValidator.TxtCastReferenceAsValidating();
        }

        public void TxtCastVariableAsValidating()
        {
            _variableControlsValidator.TxtCastVariableAsValidating();
        }

        public void TxtMemberNameValidating()
        {
            _variableControlsValidator.TxtMemberNameValidating();
        }

        public void TxtNameChanged()
        {
            _variableControlsValidator.TxtNameChanged();
        }

        public void TxtNameValidating()
        {
            _variableControlsValidator.TxtNameValidating();
        }

        public void TxtReferenceNameValidating()
        {
            _variableControlsValidator.TxtReferenceNameValidating();
        }

        public void TxtTypeNameValidating()
        {
            _variableControlsValidator.TxtTypeNameValidating();
        }

        public void ValidateDefaultValue()
        {
            ValidateDefaultValue
            (
                configureLiteralVariableControl.TxtDefaultValue.Text,
                configureLiteralVariableControl.LblDefaultValue.Text,
                _enumHelper.GetSystemType
                (
                    (LiteralVariableType)configureLiteralVariableControl.CmbLiteralType.SelectedValue
                )
            );

            void ValidateDefaultValue(string defaultValue, string devaultValueLabel, Type type)
            {
                if (!string.IsNullOrEmpty(defaultValue) && !_typeHelper.TryParse(defaultValue, type, out object? _))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidDomainFormat, devaultValueLabel, defaultValue, type.Name));
            }
        }

        public void ValidateForExistingVariableName(string currentVariableNameAttributeValue)
        {
            _variableControlsValidator.ValidateForExistingVariableName(currentVariableNameAttributeValue);
        }

        public void ValidateInputBoxes()
        {
            configureLiteralVariableControl.ClearMessage();
            _variableControlsValidator.ValidateVariableName();
            _variableControlsValidator.ValidateMemberName();
            _variableControlsValidator.ValidateCastVariableAs();
            _variableControlsValidator.ValidateReferenceDefinition();
            _variableControlsValidator.ValidateReferenceName();
            _variableControlsValidator.ValidateTypeName();
            ValidatePropertySource();
            ValidateDefaultValue();
        }

        public void ValidatePropertySource()
        {
            ValidatePropertySource
            (
                configureLiteralVariableControl.CmbPropertySource.Text.Trim(),
                configureLiteralVariableControl.LblPropertySource.Text,
                (LiteralVariableInputStyle)configureLiteralVariableControl.CmbControl.SelectedValue,
                configureLiteralVariableControl.LblControl.Text
            );

            static void ValidatePropertySource(string propertySource, string propertySourceLabel, LiteralVariableInputStyle inputStyle, string inputStyleLabel)
            {
                if (inputStyle == LiteralVariableInputStyle.PropertyInput && !Regex.IsMatch(propertySource, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, propertySourceLabel));

                if (inputStyle != LiteralVariableInputStyle.PropertyInput && propertySource.Length > 0)
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, propertySourceLabel, inputStyleLabel, Strings.dropdownTextPropertyInput));
            }
        }
    }
}
