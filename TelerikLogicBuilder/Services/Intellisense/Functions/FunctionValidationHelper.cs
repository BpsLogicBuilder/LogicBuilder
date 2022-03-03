using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Variables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class FunctionValidationHelper : IFunctionValidationHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IVariableHelper _variableHelper;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public FunctionValidationHelper(IConfigurationService configurationService, IContextProvider contextProvider)
        {
            _configurationService = configurationService;
            _variableHelper = contextProvider.VariableHelper;
            _enumHelper = contextProvider.EnumHelper;
            _exceptionHelper = contextProvider.ExceptionHelper;
        }

        public void ValidateFunctionIndirectReferenceName(ValidIndirectReference referenceDefinition, string referenceName, string functionName, ICollection<string> errors)
        {
            string referenceDefinitionVisibleText = _enumHelper.GetVisibleEnumText(referenceDefinition);
            switch (referenceDefinition)
            {
                case ValidIndirectReference.Property:
                case ValidIndirectReference.Field:
                    if (!Regex.IsMatch(referenceName, RegularExpressions.VARIABLEORFUNCTIONNAME))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.fieldPropertyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.StringKeyIndexer:
                    break;
                case ValidIndirectReference.IntegerKeyIndexer:
                    if (!int.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.integerKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.ArrayIndexer:
                    string[] referenceNames = referenceName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string index in referenceNames)
                    {
                        if (!int.TryParse(index, out int arrayIndex) || arrayIndex < 0)
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.arrayKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    }
                    break;
                case ValidIndirectReference.BooleanKeyIndexer:
                    if (!bool.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.booleanKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.ByteKeyIndexer:
                    if (!byte.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.byteKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.CharKeyIndexer:
                    if (!char.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.charKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.DateTimeKeyIndexer:
                    if (!DateTime.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateTimeKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.DateTimeOffsetKeyIndexer:
                    if (!DateTime.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateTimeOffsetKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.DateKeyIndexer:
                    if (!DateTime.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.dateKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.TimeSpanKeyIndexer:
                    if (!TimeSpan.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.timeSpanKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.TimeOfDayKeyIndexer:
                    if (!TimeSpan.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.timeOfDayKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.GuidKeyIndexer:
                    if (!Guid.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.guidKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.DecimalKeyIndexer:
                    if (!decimal.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.doubleKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.DoubleKeyIndexer:
                    if (!double.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.integerKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.FloatKeyIndexer:
                    if (!float.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.floatKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.LongKeyIndexer:
                    if (!long.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.longKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.SByteKeyIndexer:
                    if (!sbyte.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.sbyteKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.ShortKeyIndexer:
                    if (!short.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.shortKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.UIntegerKeyIndexer:
                    if (!uint.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.uIntegerKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.ULongKeyIndexer:
                    if (!ulong.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.uLongKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.UShortKeyIndexer:
                    if (!ushort.TryParse(referenceName, out _))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.uShortKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.VariableKeyIndexer:
                    if (!_configurationService.VariableList.Variables.ContainsKey(referenceName))
                        errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    break;
                case ValidIndirectReference.VariableArrayIndexer:
                    string[] indexes = referenceName.Trim().Split(new char[] { MiscellaneousConstants.COMMA }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string index in indexes)
                    {
                        //Since arrays can be multidimensional, allow the variableArrayIndex to combine variables and integer literals as indexes
                        if (_configurationService.VariableList.Variables.TryGetValue(index, out VariableBase? variable) && _variableHelper.CanBeInteger(variable))
                        {
                        }
                        else if (int.TryParse(index, out int arrayIndex) && arrayIndex > -1)
                        {
                        }
                        else
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.variableArrayKeyReferenceIsInvalidFormat3, referenceDefinitionVisibleText, referenceName, functionName));
                    }
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{B93C11EA-4A8D-4434-80CD-411359AFE6FF}");
            }
        }
    }
}
