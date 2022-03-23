using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class FunctionGenericsConfigrationValidator : IFunctionGenericsConfigrationValidator
    {
        private readonly IConfigurationService _configurationService;
        private readonly IEnumHelper _enumHelper;
        private readonly IGenericsConfigrationValidator _genericsConfigrationValidator;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public FunctionGenericsConfigrationValidator(IContextProvider contextProvider, IGenericsConfigrationValidator genericsConfigrationValidator, ITypeLoadHelper typeLoadHelper)
        {
            _configurationService = contextProvider.ConfigurationService;
            _enumHelper = contextProvider.EnumHelper;
            _genericsConfigrationValidator = genericsConfigrationValidator;
            _typeLoadHelper = typeLoadHelper;
        }

        public bool Validate(Function function, List<GenericConfigBase> genericArguments, ApplicationTypeInfo application, List<string> validationErrors)
        {
            //use the unconverted function
            Function _function = _configurationService.FunctionList.Functions[function.Name];

            //ensure function data is consistent with the configured function generic argument names and count
            if (!ValidateGenericArgumentsMatch
                (
                    _function.GenericArguments,
                    genericArguments,
                    validationErrors
                ))
            {
                return false;
            }

            //make sure the function is from a static, generic class if function has generic arguments
            //and the generic argment count matches loaded type.
            if (!ValidateGenericArgumentsCount(_function, genericArguments, application, validationErrors))
                return false;

            //make sure the configured generic argument types can be loaded
            if (!genericArguments.Aggregate
                (
                    true,
                    (isValid, next) =>
                    {
                        if (!_typeLoadHelper.TryGetSystemType(next, application, out Type? _))
                        {
                            validationErrors.Add
                            (
                                string.Format
                                (
                                    Strings.cannotLoadTypeForGenericArgumentForFunctionFormat, 
                                    next.GenericArgumentName, 
                                    function.Name
                                )
                            );
                            return false;
                        }

                        return isValid;
                    }
                )
               )
            {
                return false;
            }

            return true;
        }

        private static bool ValidateGenericArgumentsMatch(List<string> configured, List<GenericConfigBase> data, List<string> validationErrors)
        {
            List<string> dataGenericArgumentNames = data.Select(a => a.GenericArgumentName).ToList();

            IEnumerable<string> missingFromConfig = dataGenericArgumentNames.Except(configured);
            IEnumerable<string> missingFromData = configured.Except(dataGenericArgumentNames);

            if (missingFromConfig.Any() || missingFromData.Any())
            {
                validationErrors.Add
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.functionGenericArgsMisMatchFormat,
                        string.Join(Strings.itemsCommaSeparator, configured),
                        string.Join(Strings.itemsCommaSeparator, dataGenericArgumentNames)
                    )
                );

                return false;
            }

            return true;
        }

        private bool ValidateGenericArgumentsCount(Function function, List<GenericConfigBase> genericArguments, ApplicationTypeInfo application, List<string> validationErrors)
        {
            if (function.ReferenceCategory != ReferenceCategories.Type && genericArguments.Count == 0)
                return true;

            if (function.ReferenceCategory != ReferenceCategories.Type && genericArguments.Count > 0)
            {
                validationErrors.Add(GetValidationMessage());
                return false;
            }

            //Now function.ReferenceCategory == ReferenceCategories.Type

            if (!_typeLoadHelper.TryGetSystemType(function.TypeName, application, out Type? functionDeclaringType))
            {
                validationErrors.Add(GetValidationMessage());
                return false;
            }

            if (!_genericsConfigrationValidator.GenericArgumentCountMatchesType(functionDeclaringType, genericArguments))
            {
                validationErrors.Add(GetValidationMessage());
                return false;
            }

            return true;

            string GetValidationMessage()
                => string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.functionGenericArgsMisMatchFormat2,
                    _enumHelper.GetVisibleEnumText(ReferenceCategories.Type),
                    function.TypeName,
                    string.Join(Strings.itemsCommaSeparator, function.GenericArguments)
                );
        }
    }
}
