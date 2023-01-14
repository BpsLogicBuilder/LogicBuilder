using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureLiteralListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal partial class LiteralListVariableControlsValidator : ILiteralListVariableControlsValidator
    {
        private readonly IVariableControlsValidator _variableControlsValidator;
        private readonly IConfigureLiteralListVariableControl configureLiteralListVariableControl;

        public LiteralListVariableControlsValidator(
            IVariableControlValidatorFactory variableControlHelperFactory,
            IConfigureLiteralListVariableControl configureLiteralListVariableControl)
        {
            _variableControlsValidator = variableControlHelperFactory.GetVariableControlsValidator(configureLiteralListVariableControl);
            this.configureLiteralListVariableControl = configureLiteralListVariableControl;
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

        public void ValidateForExistingVariableName(string currentVariableNameAttributeValue)
        {
            _variableControlsValidator.ValidateForExistingVariableName(currentVariableNameAttributeValue);
        }

        public void ValidateInputBoxes()
        {
            configureLiteralListVariableControl.ClearMessage();
            _variableControlsValidator.ValidateVariableName();
            _variableControlsValidator.ValidateMemberName();
            _variableControlsValidator.ValidateCastVariableAs();
            _variableControlsValidator.ValidateReferenceDefinition();
            _variableControlsValidator.ValidateReferenceName();
            _variableControlsValidator.ValidateTypeName();
            ValidatePropertySource();
        }

        public void ValidatePropertySource()
        {
            ValidatePropertySource
            (
                configureLiteralListVariableControl.CmbPropertySource.Text.Trim(),
                configureLiteralListVariableControl.LblPropertySource.Text,
                (LiteralVariableInputStyle)configureLiteralListVariableControl.CmbElementControl.SelectedValue,
                configureLiteralListVariableControl.LblElementControl.Text
            );

            static void ValidatePropertySource(string propertySource, string propertySourceLabel, LiteralVariableInputStyle inputStyle, string inputStyleLabel)
            {
                if (inputStyle == LiteralVariableInputStyle.PropertyInput && !FullyQulifiedClassNameMRegex().IsMatch(propertySource))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, propertySourceLabel));

                if (inputStyle != LiteralVariableInputStyle.PropertyInput && propertySource.Length > 0)
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.fieldSourceMustBeEmptyFormat, propertySourceLabel, inputStyleLabel, Strings.dropdownTextPropertyInput));
            }
        }

        [GeneratedRegex(RegularExpressions.FULLYQUALIFIEDCLASSNAME)]
        private static partial Regex FullyQulifiedClassNameMRegex();
    }
}
