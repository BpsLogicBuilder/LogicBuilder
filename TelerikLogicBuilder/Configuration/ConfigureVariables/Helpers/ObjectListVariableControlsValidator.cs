using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.ConfigureObjectListVariable;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal class ObjectListVariableControlsValidator : IObjectListVariableControlsValidator
    {
        private readonly IVariableControlsValidator _variableControlsValidator;
        private readonly IConfigureObjectListVariableControl configureObjectListVariableControl;

        public ObjectListVariableControlsValidator(
            IVariableControlValidatorFactory variableControlHelperFactory,
            IConfigureObjectListVariableControl configureObjectListVariableControl)
        {
            _variableControlsValidator = variableControlHelperFactory.GetVariableControlsValidator(configureObjectListVariableControl);
            this.configureObjectListVariableControl = configureObjectListVariableControl;
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
            configureObjectListVariableControl.ClearMessage();
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
                configureObjectListVariableControl.CmbObjectType.Text.Trim(),
                configureObjectListVariableControl.LblObjectType.Text
            );

            static void DoValidation(string objectType, string objectTypeLabel)
            {
                if (!Regex.IsMatch(objectType, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, objectTypeLabel));
            }
        }
    }
}
