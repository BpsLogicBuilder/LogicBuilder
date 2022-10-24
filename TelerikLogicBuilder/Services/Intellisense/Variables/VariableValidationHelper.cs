using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Variables
{
    internal class VariableValidationHelper : IVariableValidationHelper
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IVariableHelper _variableHelper;

        public VariableValidationHelper(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IVariableHelper variableHelper)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _variableHelper = variableHelper;
        }

        public void ValidateMemberName(VariableCategory variableCategory, string memberName, string variableName, ICollection<string> errors, IDictionary<string, VariableBase> variables)
        {
            switch (variableCategory)
            {
                case VariableCategory.Property:
                case VariableCategory.Field:
                    if (!Regex.IsMatch(memberName, RegularExpressions.VARIABLEORFUNCTIONNAME))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableNameIsInvalidFormat, variableName));
                    break;
                case VariableCategory.StringKeyIndexer:
                    break;
                case VariableCategory.IntegerKeyIndexer:
                    if (!int.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.integerKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.ArrayIndexer:
                    string[] variableNames = memberName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string index in variableNames)
                    {
                        if (!int.TryParse(index, out int arrayIndex) || arrayIndex < 0)
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.arrayKeyIndexIsInvalidFormat, variableName));
                    }
                    break;
                case VariableCategory.BooleanKeyIndexer:
                    if (!bool.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.booleanKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.ByteKeyIndexer:
                    if (!byte.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.byteKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.CharKeyIndexer:
                    if (!char.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.charKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.DateTimeKeyIndexer:
                    if (!DateTime.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateTimeKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.DateTimeOffsetKeyIndexer:
                    if (!DateTimeOffset.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateTimeOffsetKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.DateOnlyKeyIndexer:
                    if (!DateOnly.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateOnlyKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.DateKeyIndexer:
                    if (!Date.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.TimeSpanKeyIndexer:
                    if (!TimeSpan.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.timeSpanKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.TimeOnlyKeyIndexer:
                    if (!TimeOnly.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.timeOnlyKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.TimeOfDayKeyIndexer:
                    if (!TimeOfDay.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.timeOfDayKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.GuidKeyIndexer:
                    if (!Guid.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.guidKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.DecimalKeyIndexer:
                    if (!decimal.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.decimalKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.DoubleKeyIndexer:
                    if (!double.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.doubleKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.FloatKeyIndexer:
                    if (!float.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.floatKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.LongKeyIndexer:
                    if (!long.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.longKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.SByteKeyIndexer:
                    if (!sbyte.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.sbyteKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.ShortKeyIndexer:
                    if (!short.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.shortKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.UIntegerKeyIndexer:
                    if (!uint.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.uIntegerKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.ULongKeyIndexer:
                    if (!ulong.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.uLongKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.UShortKeyIndexer:
                    if (!ushort.TryParse(memberName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.uShortKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.VariableKeyIndexer:
                    if (memberName == variableName)
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableIndexCannotBeSelfFormat, variableName));
                    else if (!variables.ContainsKey(memberName))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableKeyIndexIsInvalidFormat, variableName));
                    break;
                case VariableCategory.VariableArrayIndexer:
                    string[] indexes = memberName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string index in indexes)
                    {
                        if (index == variableName)
                        {
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableIndexCannotBeSelfFormat, variableName));
                            continue;
                        }
                        //Since arrays can be multidimensional, allow the variableArrayIndex to combine variables and integer literals as indexes
                        if (variables.TryGetValue(index, out VariableBase? variable) && _variableHelper.CanBeInteger(variable))
                        {
                        }
                        else if (int.TryParse(index, out int arrayIndex) && arrayIndex > -1)
                        {
                        }
                        else
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableArrayIndexIsInvalidFormat, variableName));
                    }
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{8747BF4D-64F9-47B9-8136-2DA40C9FE80C}");
            }
        }

        public void ValidateVariableIndirectReferenceName(ValidIndirectReference referenceDefinition, string referenceName, string variableName, ICollection<string> errors, IDictionary<string, VariableBase> variables)
        {
            string referenceDefinitionVisibleText = _enumHelper.GetVisibleEnumText(referenceDefinition);

            switch (referenceDefinition)
            {
                case ValidIndirectReference.Property:
                case ValidIndirectReference.Field:
                    if (!Regex.IsMatch(referenceName, RegularExpressions.VARIABLEORFUNCTIONNAME))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.fieldPropertyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.StringKeyIndexer:
                    break;
                case ValidIndirectReference.IntegerKeyIndexer:
                    if (!int.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.integerKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.ArrayIndexer:
                    string[] referenceNames = referenceName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string index in referenceNames)
                    {
                        if (!int.TryParse(index, out int arrayIndex) || arrayIndex < 0)
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.arrayKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    }
                    break;
                case ValidIndirectReference.BooleanKeyIndexer:
                    if (!bool.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.booleanKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.ByteKeyIndexer:
                    if (!byte.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.byteKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.CharKeyIndexer:
                    if (!char.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.charKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.DateTimeKeyIndexer:
                    if (!DateTime.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateTimeKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.DateTimeOffsetKeyIndexer:
                    if (!DateTimeOffset.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateTimeOffsetKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.DateOnlyKeyIndexer:
                    if (!DateOnly.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateOnlyKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.DateKeyIndexer:
                    if (!Date.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.TimeSpanKeyIndexer:
                    if (!TimeSpan.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.timeSpanKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.TimeOnlyKeyIndexer:
                    if (!TimeOnly.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.timeOnlyKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.TimeOfDayKeyIndexer:
                    if (!TimeOfDay.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.timeOfDayKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.GuidKeyIndexer:
                    if (!Guid.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.guidKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.DecimalKeyIndexer:
                    if (!decimal.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.doubleKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.DoubleKeyIndexer:
                    if (!double.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.integerKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.FloatKeyIndexer:
                    if (!float.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.floatKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.LongKeyIndexer:
                    if (!long.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.longKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.SByteKeyIndexer:
                    if (!sbyte.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.sbyteKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.ShortKeyIndexer:
                    if (!short.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.shortKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.UIntegerKeyIndexer:
                    if (!uint.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.uIntegerKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.ULongKeyIndexer:
                    if (!ulong.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.uLongKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.UShortKeyIndexer:
                    if (!ushort.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.uShortKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.VariableKeyIndexer:
                    if (referenceName == variableName)
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.referenceCannotEqualVariable, referenceName, variableName));
                    else if (!variables.ContainsKey(referenceName))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    break;
                case ValidIndirectReference.VariableArrayIndexer:
                    string[] indexes = referenceName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string index in indexes)
                    {
                        if (index == variableName)
                        {
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.referenceCannotEqualVariable, referenceName, variableName));
                            continue;
                        }
                        //Since arrays can be multidimensional, allow the variableArrayIndex to combine variables and integer literals as indexes
                        if (variables.TryGetValue(index, out VariableBase? variable) && _variableHelper.CanBeInteger(variable))
                        {
                        }
                        else if (int.TryParse(index, out int arrayIndex) && arrayIndex > -1)//-1 must be a check to make sure the parsed integer is positive
                        {
                        }
                        else
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableArrayKeyReferenceIsInvalidFormat2, referenceDefinitionVisibleText, referenceName, variableName));
                    }
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{EC44EA7E-D037-4A88-9B79-502B99C6090B}");
            }
        }
    }
}
