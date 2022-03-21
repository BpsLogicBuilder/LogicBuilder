using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
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
    internal class ConstructorGenericsConfigrationValidator : IConstructorGenericsConfigrationValidator
    {
        private readonly IConfigurationService _configurationService;
        private readonly IGenericsConfigrationValidator _genericsConfigrationValidator;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public ConstructorGenericsConfigrationValidator(IConfigurationService configurationService, IGenericsConfigrationValidator genericsConfigrationValidator, ITypeLoadHelper typeLoadHelper)
        {
            _configurationService = configurationService;
            _genericsConfigrationValidator = genericsConfigrationValidator;
            _typeLoadHelper = typeLoadHelper;
        }

        public bool Validate(Constructor constructor, List<GenericConfigBase> genericArguments, ApplicationTypeInfo application, List<string> validationErrors)
        {
            //Make sure we are using the unconverted _constructor
            Constructor _constructor = _configurationService.ConstructorList.Constructors[constructor.Name];

            //constructorType may be generic type definition
            if (!_typeLoadHelper.TryGetSystemType(_constructor.TypeName, application, out Type? constructorType))
            {
                validationErrors.Add(string.Format(CultureInfo.CurrentCulture, Strings.cannotLoadTypeForConstructorFormat, _constructor.TypeName, _constructor.Name));
                return false;
            }

            //ensure constructor data is consistent with the constructor with regard to generics
            if (!ValidateGenericArgumentsMatch
                (
                    _constructor.GenericArguments,
                    genericArguments,
                    validationErrors
                ))
            {
                return false;
            }

            //also ensure that the constructor type is a generic type definition if the data has generic arguments
            if (genericArguments.Count > 0 && !_genericsConfigrationValidator.GenericArgumentCountMatchesType(constructorType, genericArguments))
            {
                validationErrors.Add
                (
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.constructorGenericArgsMisMatchFormat2,
                        _constructor.TypeName,
                        string.Join(Strings.itemsCommaSeparator, _constructor.GenericArguments)
                    )
                );
                return false;
            }

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
                                    CultureInfo.CurrentCulture, 
                                    Strings.cannotLoadTypeForGenericArgumentForConstructorFormat, 
                                    next.GenericArgumentName,
                                    _constructor.Name
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
                        Strings.constructorGenericArgsMisMatchFormat,
                        string.Join(Strings.itemsCommaSeparator, configured),
                        string.Join(Strings.itemsCommaSeparator, dataGenericArgumentNames)
                    )
                );
                 
                return false;
            }

            return true;
        }
    }
}
