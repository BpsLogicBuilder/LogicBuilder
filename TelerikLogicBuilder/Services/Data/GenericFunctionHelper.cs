using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Data
{
    internal class GenericFunctionHelper : IGenericFunctionHelper
    {
        private readonly IContextProvider _contextProvider;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericParametersHelper _genericParametersHelper;
        private readonly IGenericReturnTypeHelper _genericReturnTypeHelper;
        private readonly IGenericsConfigrationValidator _genericsConfigrationValidator;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public GenericFunctionHelper(IContextProvider contextProvider, IGenericParametersHelper genericParametersHelper, IGenericReturnTypeHelper genericReturnTypeHelper, IGenericsConfigrationValidator genericsConfigrationValidator, ITypeLoadHelper typeLoadHelper)
        {
            _exceptionHelper = contextProvider.ExceptionHelper;
            _typeHelper = contextProvider.TypeHelper;
            _contextProvider = contextProvider;
            _genericParametersHelper = genericParametersHelper;
            _genericReturnTypeHelper = genericReturnTypeHelper;
            _genericsConfigrationValidator = genericsConfigrationValidator;
            _typeLoadHelper = typeLoadHelper;
        }

        public Function ConvertGenericTypes(Function function, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application)
        {
            if (!function.HasGenericArguments)
                return function;

            if (function.GenericArguments.Count != genericParameters.Count)
                throw _exceptionHelper.CriticalException("{B9480848-F58F-4D09-B6C8-4989FBE19D00}");

            return new Function
            (
                function.Name,
                function.MemberName,
                function.FunctionCategory,
                _typeHelper.ToId(MakeGenericType(function, genericParameters, application)),
                function.ReferenceNameString,
                function.ReferenceDefinitionString,
                function.CastReferenceAs,
                function.ReferenceCategory,
                function.ParametersLayout,
                _genericParametersHelper.GetConvertedParameters(function.Parameters, genericParameters, application),
                function.GenericArguments,
                _genericReturnTypeHelper.GetConvertedReturnType(function.ReturnType, genericParameters, application),
                function.Summary,
                _contextProvider
            );
        }

        public Type MakeGenericType(Function function, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application)
        {
            if (!_typeLoadHelper.TryGetSystemType(function.TypeName, application, out Type? functionType))
                throw _exceptionHelper.CriticalException("{EC4695BB-D8E0-40FE-9F74-BCCDC28C1B75}");

            if (!ValidateGenericArgumentsCount(function.ReferenceCategory, functionType, genericParameters))
                throw _exceptionHelper.CriticalException("{3B5DB5EC-DD17-41F8-8DFD-FD6ABD039599}");

            IEnumerable<string> missingFromConfig = genericParameters.Select(a => a.GenericArgumentName).Except(function.GenericArguments);
            IEnumerable<string> missingFromData = function.GenericArguments.Except(genericParameters.Select(a => a.GenericArgumentName));

            if (missingFromConfig.Any())
                throw _exceptionHelper.CriticalException("{93C22ACF-F7B2-4BEE-B29A-8816370DB47E}");

            if (missingFromData.Any())
                throw _exceptionHelper.CriticalException("{6939BBCC-7C55-490F-8CC3-F01E85D14DE0}");

            Dictionary<string, GenericConfigBase> genericArgumentsTable = genericParameters.ToDictionary(ga => ga.GenericArgumentName);

            return functionType.MakeGenericType
            (
                function
                .GenericArguments
                .Select
                (
                    ga =>
                    {
                        if (!_typeLoadHelper.TryGetSystemType(genericArgumentsTable[ga], application, out Type? argumentType))
                            throw _exceptionHelper.CriticalException("{E76331F1-B513-4F3E-90CC-836F2440340D}");
                        //Generic configurations should be validated before converting a constructor

                        return argumentType;
                    }
                )
                .ToArray()
            );
        }

        private bool ValidateGenericArgumentsCount(ReferenceCategories referenceCategory, Type functionDeclaringType, IList<GenericConfigBase> genericArguments)
        {
            if (referenceCategory != ReferenceCategories.Type && genericArguments.Count == 0)
                return true;

            if (referenceCategory != ReferenceCategories.Type && genericArguments.Count > 0)
                return false;

            //Now function.ReferenceCategory == ReferenceCategories.Type

            if (!_genericsConfigrationValidator.GenericArgumentCountMatchesType(functionDeclaringType, genericArguments))
                return false;

            return true;
        }
    }
}
