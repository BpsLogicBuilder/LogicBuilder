using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.ConfigureFunction;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions.Helpers
{
    internal partial class FunctionControlsValidator : IFunctionControlsValidator
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IFunctionValidationHelper _functionValidationHelper;
        private readonly IStringHelper _stringHelper;

        private readonly IConfigureFunctionControl configureFunctionControl;

        public FunctionControlsValidator(
            IEnumHelper enumHelper,
            IFunctionValidationHelper functionValidationHelper,
            IStringHelper stringHelper,
            IConfigureFunctionControl configureFunctionControl)
        {
            _enumHelper = enumHelper;
            _functionValidationHelper = functionValidationHelper;
            _stringHelper = stringHelper;
            this.configureFunctionControl = configureFunctionControl;
        }

        private RadDropDownList CmbReferenceCategory => configureFunctionControl.CmbReferenceCategory;
        private RadDropDownList CmbReferenceDefinition => configureFunctionControl.CmbReferenceDefinition;
        private RadLabel LblFunctionName => configureFunctionControl.LblFunctionName;
        private RadLabel LblMemberName => configureFunctionControl.LblMemberName;
        private RadLabel LblTypeName => configureFunctionControl.LblTypeName;
        private RadTextBox TxtCastReferenceAs => configureFunctionControl.TxtCastReferenceAs;
        private RadTextBox TxtFunctionName => configureFunctionControl.TxtFunctionName;
        private RadTextBox TxtMemberName => configureFunctionControl.TxtMemberName;
        private RadTextBox TxtReferenceName => configureFunctionControl.TxtReferenceName;
        private AutoCompleteRadDropDownList TxtTypeName => configureFunctionControl.TxtTypeName;

        public void CmbReferenceDefinitionValidating()
        {
            configureFunctionControl.ClearMessage();
            ValidateReferenceName();
            ValidateFunctionName();
            ValidateMemberName();
            ValidateTypeName();
        }

        public void TxtCastReferenceAsValidating()
        {
            configureFunctionControl.ClearMessage();
            ValidateReferenceName();
            ValidateFunctionName();
            ValidateMemberName();
            ValidateTypeName();
        }

        public void TxtFunctionNameValidating()
        {
            configureFunctionControl.ClearMessage();
            ValidateFunctionName();
            ValidateMemberName();
            ValidateReferenceName();
            ValidateTypeName();
        }

        public void TxtMemberNameValidating()
        {
            configureFunctionControl.ClearMessage();
            ValidateMemberName();
            ValidateFunctionName();
            ValidateReferenceName();
            ValidateTypeName();
        }

        public void TxtReferenceNameValidating()
        {
            configureFunctionControl.ClearMessage();
            ValidateReferenceName();
            ValidateFunctionName();
            ValidateMemberName();
            ValidateTypeName();
        }

        public void TxtTypeNameValidating()
        {
            configureFunctionControl.ClearMessage();
            ValidateTypeName();
            ValidateMemberName();
            ValidateFunctionName();
            ValidateReferenceName();
        }

        public void ValidateInputBoxes()
        {
            configureFunctionControl.ClearMessage();
            ValidateFunctionName();
            ValidateReferenceDefinition();
            ValidateReferenceName();
            ValidateMemberName();
            ValidateTypeName();
        }

        private void ValidateFunctionName()
        {
            if (!XmlNameAttributeRegex().IsMatch(TxtFunctionName.Text))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidTxtNameTextFormat, LblFunctionName.Text));
        }

        private void ValidateReferenceDefinition()
        {
            ValidateReferenceDefinition
            (
                (ReferenceCategories)CmbReferenceCategory.SelectedValue,
                CmbReferenceDefinition.Text.Trim()
            );

            void ValidateReferenceDefinition(ReferenceCategories referenceCategory, string referenceDefinition)
            {
                if ((referenceCategory == ReferenceCategories.This || referenceCategory == ReferenceCategories.Type)
                        && referenceDefinition.Length != 0)
                    throw new LogicBuilderException(Strings.functionDefinitionNotEmpty);

                if ((referenceCategory == ReferenceCategories.InstanceReference || referenceCategory == ReferenceCategories.StaticReference)
                        && referenceDefinition.Length == 0)
                    throw new LogicBuilderException(Strings.functionBlankDefinition);

                foreach (string definition in GetReferenceDefinitionParts())
                {
                    if (!Enum.IsDefined(typeof(ValidIndirectReference), definition))
                        throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.functionInvalidIndirectDefinitionFormat2, definition, Environment.NewLine, _enumHelper.GetValidIndirectReferencesList()));
                }

                IList<string> GetReferenceDefinitionParts()
                    => _enumHelper.ConvertVisibleDropDownValuesToEnumNames<ValidIndirectReference>
                    (
                        _stringHelper.SplitWithQuoteQualifier
                        (
                            CmbReferenceDefinition.Text.Trim(),//reference definitions are all valid at this point
                            MiscellaneousConstants.PERIODSTRING
                        )
                    );
            }
        }

        private void ValidateReferenceName()
        {
            ValidateReferenceDefinition();

            ValidateReferenceName
            (
                TxtFunctionName.Text.Trim(),
                _stringHelper.SplitWithQuoteQualifier(TxtReferenceName.Text.Trim(), MiscellaneousConstants.PERIODSTRING),
                _stringHelper.SplitWithQuoteQualifier(TxtCastReferenceAs.Text.Trim(), MiscellaneousConstants.PERIODSTRING),
                _enumHelper.ConvertToEnumList<ValidIndirectReference>
                (
                    _enumHelper.ConvertVisibleDropDownValuesToEnumNames<ValidIndirectReference>
                    (
                        _stringHelper.SplitWithQuoteQualifier
                        (
                            CmbReferenceDefinition.Text.Trim(),//reference definitions are all valid at this point
                            MiscellaneousConstants.PERIODSTRING
                        )
                    )
                )
            );

            void ValidateReferenceName(string functionName,
                string[] referenceNameArray,
                string[] castReferenceAsArray,
                IList<ValidIndirectReference> referenceDefinitionList)
            {
                if (referenceNameArray.Length != referenceDefinitionList.Count)
                    throw new LogicBuilderException(Strings.functionNameAndDefinitionFormat2);

                if (castReferenceAsArray.Length != 0 && referenceNameArray.Length != castReferenceAsArray.Length)
                    throw new LogicBuilderException(Strings.functionNameAndCastRefAsFormat2);

                List<string> errors = new();
                for (int i = 0; i < referenceDefinitionList.Count; i++)
                    _functionValidationHelper.ValidateFunctionIndirectReferenceName(referenceDefinitionList[i], referenceNameArray[i], functionName, errors);

                if (errors.Count > 0)
                    throw new LogicBuilderException(string.Join(Environment.NewLine, errors));
            }
        }

        private void ValidateMemberName()
        {
            if (!MemberNameRegex().IsMatch(TxtMemberName.Text))
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidVariableOrFunctionNameFormat, LblMemberName.Text));
        }

        private void ValidateTypeName()
        {
            ValidateTypeName
            (
                TxtTypeName.Text.Trim(),
                LblTypeName.Text,
                (ReferenceCategories)CmbReferenceCategory.SelectedValue
            );

            static void ValidateTypeName(string typeName, string typeDescription, ReferenceCategories referenceCategory)
            {
                if ((referenceCategory == ReferenceCategories.Type || referenceCategory == ReferenceCategories.StaticReference)
                    && !FullyQualifiedClassNameRegex().IsMatch(typeName))
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.invalidClassNameFormat, typeDescription));

                if ((referenceCategory != ReferenceCategories.Type && referenceCategory != ReferenceCategories.StaticReference)
                        && typeName.Length != 0)
                    throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.functionTypeNameMustBeEmpty, typeDescription));
            }
        }

        [GeneratedRegex(RegularExpressions.XMLNAMEATTRIBUTE)]
        private static partial Regex XmlNameAttributeRegex();
        [GeneratedRegex(RegularExpressions.VARIABLEORFUNCTIONNAME)]
        private static partial Regex MemberNameRegex();
        [GeneratedRegex(RegularExpressions.FULLYQUALIFIEDCLASSNAME)]
        private static partial Regex FullyQualifiedClassNameRegex();
    }
}
