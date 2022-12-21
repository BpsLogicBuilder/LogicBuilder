using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables.Helpers
{
    internal class VariableControlsValidator : IVariableControlsValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IVariableValidationHelper _variableValidationHelper;
        private readonly IConfigureVariableControl configureVariableControl;

        public VariableControlsValidator(
            IEnumHelper enumHelper,
            IStringHelper stringHelper,
            IVariableValidationHelper variableValidationHelper,
            IConfigureVariableControl configureVariableControl)
        {
            _enumHelper = enumHelper;
            _stringHelper = stringHelper;
            _variableValidationHelper = variableValidationHelper;
            this.configureVariableControl = configureVariableControl;
        }

        public void CmbReferenceDefinitionValidating()
        {
            configureVariableControl.ClearMessage();
            ValidateReferenceName();
            ValidateVariableName();
            ValidateMemberName();
            ValidateTypeName();
        }

        public void TxtCastReferenceAsValidating()
        {
            configureVariableControl.ClearMessage();
            ValidateReferenceName();
            ValidateVariableName();
            ValidateMemberName();
            ValidateTypeName();
        }

        public void TxtCastVariableAsValidating()
        {
            configureVariableControl.ClearMessage();
            ValidateMemberName();
            ValidateVariableName();
            ValidateReferenceName();
            ValidateTypeName();
        }

        public void TxtMemberNameValidating()
        {
            configureVariableControl.ClearMessage();
            ValidateMemberName();
            ValidateVariableName();
            ValidateReferenceName();
            ValidateTypeName();
        }

        public void TxtNameChanged()
        {//prviously used for automatic variables
        }

        public void TxtNameValidating()
        {
            configureVariableControl.ClearMessage();
            ValidateVariableName();
            ValidateMemberName();
            ValidateReferenceName();
            ValidateTypeName();
        }

        public void TxtReferenceNameValidating()
        {
            configureVariableControl.ClearMessage();
            ValidateReferenceName();
            ValidateVariableName();
            ValidateMemberName();
            ValidateTypeName();
        }

        public void TxtTypeNameValidating()
        {
            configureVariableControl.ClearMessage();
            ValidateTypeName();
            ValidateReferenceName();
            ValidateVariableName();
            ValidateMemberName();
        }

        public void ValidateCastVariableAs()
        {
            configureVariableControl.ClearMessage();
            ValidateVariableName();
            ValidateMemberName();
            ValidateReferenceName();
            ValidateTypeName();
        }

        public void ValidateForExistingVariableName(string currentVariableNameAttributeValue)
        {
            string newValue = configureVariableControl.TxtName.Text.Trim();
            if (currentVariableNameAttributeValue != newValue && configureVariableControl.VariableNames.Contains(newValue))
            {
                throw new LogicBuilderException
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture, 
                        Strings.variableExistsFormat, 
                        newValue
                    )
                );
            }
        }

        public void ValidateMemberName()
        {
            ValidateMemberName
            (
                (VariableCategory)configureVariableControl.CmbVariableCategory.SelectedValue,
                configureVariableControl.TxtMemberName.Text.Trim(),
                configureVariableControl.TxtName.Text.Trim()
            );

            void ValidateMemberName(VariableCategory variableCategory, string memberName, string variableName)
            {
                List<string> errors = new();
                _variableValidationHelper.ValidateMemberName(variableCategory, memberName, variableName, errors, configureVariableControl.VariablesDictionary);
                if (errors.Count > 0)
                    throw new LogicBuilderException(string.Join(Environment.NewLine, errors));
            }
        }

        public void ValidateReferenceDefinition()
        {
            ValidateReferenceDefinition
            (
                (ReferenceCategories)configureVariableControl.CmbReferenceCategory.SelectedValue,
                configureVariableControl.CmbReferenceDefinition.Text.Trim()
            );

            void ValidateReferenceDefinition(ReferenceCategories referenceCategory, string referenceDefinition)
            {
                if ((referenceCategory == ReferenceCategories.This || referenceCategory == ReferenceCategories.Type)
                        && referenceDefinition.Length != 0)
                    throw new LogicBuilderException(Strings.variableDefinitionNotEmpty);

                if ((referenceCategory == ReferenceCategories.InstanceReference || referenceCategory == ReferenceCategories.StaticReference)
                        && referenceDefinition.Length == 0)
                    throw new LogicBuilderException(Strings.variableBlankDefinition);

                IList<string> dropDownEnumValues = _enumHelper.ConvertVisibleDropDownValuesToEnumNames<ValidIndirectReference>
                (
                    _stringHelper.SplitWithQuoteQualifier(referenceDefinition, MiscellaneousConstants.PERIODSTRING)
                );

                foreach (string definition in dropDownEnumValues)
                {
                    if (!Enum.IsDefined(typeof(ValidIndirectReference), definition))
                        throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.variableInvalidIndirectDefinitionFormat2, definition, Environment.NewLine, _enumHelper.GetValidIndirectReferencesList()));
                }
            }
        }

        public void ValidateReferenceName()
        {
            ValidateReferenceDefinition();

            ValidateReferenceName
            (
                configureVariableControl.TxtName.Text.Trim(),
                _stringHelper.SplitWithQuoteQualifier
                (
                    configureVariableControl.TxtReferenceName.Text.Trim(), 
                    MiscellaneousConstants.PERIODSTRING
                ),
                _stringHelper.SplitWithQuoteQualifier
                (
                    configureVariableControl.TxtCastReferenceAs.Text.Trim(), 
                    MiscellaneousConstants.PERIODSTRING
                ),
                _enumHelper.ConvertToEnumList<ValidIndirectReference>
                (
                    _enumHelper.ConvertVisibleDropDownValuesToEnumNames<ValidIndirectReference>
                    (
                        _stringHelper.SplitWithQuoteQualifier
                        (
                            configureVariableControl.CmbReferenceDefinition.Text.Trim(),//reference definitions are all valid at this point
                            MiscellaneousConstants.PERIODSTRING
                        )
                    )
                )
            );

            void ValidateReferenceName(string decisionName,
                string[] referenceNameArray,
                string[] castReferenceAsArray,
                IList<ValidIndirectReference> referenceDefinitionList)
            {
                List<string> errors = new();

                if (referenceNameArray.Length != referenceDefinitionList.Count)
                    throw new LogicBuilderException(Strings.variableNameAndDefinitionFormat2);

                if (castReferenceAsArray.Length != 0 && referenceNameArray.Length != castReferenceAsArray.Length)
                    throw new LogicBuilderException(Strings.variableCastRefAsAndDefinitionFormat2);

                for (int i = 0; i < referenceDefinitionList.Count; i++)
                    _variableValidationHelper.ValidateVariableIndirectReferenceName(referenceDefinitionList[i], referenceNameArray[i], decisionName, errors, configureVariableControl.VariablesDictionary);

                if (errors.Count > 0)
                    throw new LogicBuilderException(string.Join(Environment.NewLine, errors));
            }
        }

        public void ValidateTypeName()
        {
            ValidateTypeName
            (
                configureVariableControl.TxtTypeName.Text.Trim(),
                configureVariableControl.LblTypeName.Text,
                (ReferenceCategories)configureVariableControl.CmbReferenceCategory.SelectedValue
            );

            static void ValidateTypeName(string typeName, string typeDescription, ReferenceCategories referenceCategory)
            {
                if ((referenceCategory == ReferenceCategories.Type || referenceCategory == ReferenceCategories.StaticReference)
                    && !Regex.IsMatch(typeName, RegularExpressions.FULLYQUALIFIEDCLASSNAME))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, typeDescription));

                if ((referenceCategory != ReferenceCategories.Type && referenceCategory != ReferenceCategories.StaticReference)
                        && typeName.Length != 0)
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.variableTypeNameMustBeEmpty, typeDescription));
            }
        }

        public void ValidateVariableName()
        {
            if (!Regex.IsMatch(configureVariableControl.TxtName.Text, RegularExpressions.XMLNAMEATTRIBUTE))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidTxtNameTextFormat, configureVariableControl.LblName.Text));
        }
    }
}
