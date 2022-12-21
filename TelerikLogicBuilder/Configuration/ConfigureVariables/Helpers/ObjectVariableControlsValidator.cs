using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal class ObjectVariableControlsValidator : IObjectVariableControlsValidator
    {
        private readonly IVariableControlsValidator _variableControlsValidator;
        private readonly IConfigureObjectVariableControl configureObjectVariableControl;

        public ObjectVariableControlsValidator(
            IVariableControlValidatorFactory variableControlHelperFactory,
            IConfigureObjectVariableControl configureObjectVariableControl)
        {
            _variableControlsValidator = variableControlHelperFactory.GetVariableControlsValidator(configureObjectVariableControl);
            this.configureObjectVariableControl = configureObjectVariableControl;
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
            configureObjectVariableControl.ClearMessage();
            _variableControlsValidator.ValidateVariableName();
            _variableControlsValidator.ValidateMemberName();
            _variableControlsValidator.ValidateCastVariableAs();
            _variableControlsValidator.ValidateReferenceDefinition();
            _variableControlsValidator.ValidateReferenceName();
            _variableControlsValidator.ValidateTypeName();
            ValidateObjectType();
        }

        public void ValidateObjectType()
        {
            DoValidation
            (
                configureObjectVariableControl.CmbObjectType.Text.Trim(),
                configureObjectVariableControl.LblObjectType.Text
            );

            static void DoValidation(string objectType, string objectTypeLabel)
            {
                if (!Regex.IsMatch(objectType, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, objectTypeLabel));
            }
        }
    }
}
