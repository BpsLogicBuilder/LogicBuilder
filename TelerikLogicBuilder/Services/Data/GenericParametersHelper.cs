using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Data
{
    internal class GenericParametersHelper : IGenericParametersHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IParameterFactory _parameterFactory;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public GenericParametersHelper(
            IExceptionHelper exceptionHelper,
            IParameterFactory parameterFactory,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper)
        {
            _exceptionHelper = exceptionHelper;
            _parameterFactory = parameterFactory;
            _typeHelper = typeHelper;
            _typeLoadHelper = typeLoadHelper;
        }

        public List<ParameterBase> GetConvertedParameters(IList<ParameterBase> parameters, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application) 
            => parameters
                .Select
                (
                    parameter => parameter switch
                    {
                        GenericParameter genericParameter => GetConvertedGenericParameter(genericParameter, genericParameters),
                        ListOfGenericsParameter listOfGenericsParameter => GetConvertedListOfGenericsParameter(listOfGenericsParameter, genericParameters, application),
                        _ => parameter,
                    }
                )
                .ToList();

        private ParameterBase GetConvertedGenericParameter(GenericParameter parameter, IList<GenericConfigBase> genericParameters)
        {
            var genericParametersDictionary = genericParameters.ToDictionary(gp => gp.GenericArgumentName);
            return genericParametersDictionary[parameter.GenericArgumentName] switch
            {
                LiteralGenericConfig literalGenericConfig => ConvertToLiteralParameter(literalGenericConfig, parameter),
                ObjectGenericConfig objectGenericConfig => ConvertToObjectParameter(objectGenericConfig, parameter),
                LiteralListGenericConfig literalListGenericConfig => ConvertToLiteralListParameter(literalListGenericConfig, parameter),
                ObjectListGenericConfig objectListGenericConfig => ConvertToObjectListParameter(objectListGenericConfig, parameter),
                _ => throw _exceptionHelper.CriticalException("{E0749B76-0618-4E98-8A25-2AEC4F24F6D3}"),
            };
        }

        private ParameterBase GetConvertedListOfGenericsParameter(ListOfGenericsParameter parameter, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application)
        {
            var genericParametersDictionary = genericParameters.ToDictionary(gp => gp.GenericArgumentName);
            return genericParametersDictionary[parameter.GenericArgumentName] switch
            {
                LiteralGenericConfig literalGenericConfig => ConvertToLiteralListParameter(literalGenericConfig, parameter),
                ObjectGenericConfig objectGenericConfig => ConvertToObjectListParameter(objectGenericConfig, parameter),
                LiteralListGenericConfig literalListGenericConfig => ConvertToObjectListParameter(literalListGenericConfig, parameter, application),
                ObjectListGenericConfig objectListGenericConfig => ConvertToObjectListParameter(objectListGenericConfig, parameter, application),
                _ => throw _exceptionHelper.CriticalException("{838EE7B8-F08B-49B8-A197-CCF8F9B609D3}"),
            };
        }

        #region Parameters from Generic Parameter
        private LiteralParameter ConvertToLiteralParameter(LiteralGenericConfig literalConfig, GenericParameter parameter) 
            => _parameterFactory.GetLiteralParameter
            (
                parameter.Name,
                parameter.IsOptional,
                parameter.Comments,
                literalConfig.LiteralType,
                literalConfig.Control,
                literalConfig.UseForEquality,
                literalConfig.UseForHashCode,
                literalConfig.UseForToString,
                literalConfig.PropertySource,
                literalConfig.PropertySourceParameter,
                literalConfig.DefaultValue,
                literalConfig.Domain
            );
        private ObjectParameter ConvertToObjectParameter(ObjectGenericConfig objectConfig, GenericParameter parameter) 
            => _parameterFactory.GetObjectParameter
            (
                parameter.Name,
                parameter.IsOptional,
                parameter.Comments,
                objectConfig.ObjectType,
                objectConfig.UseForEquality,
                objectConfig.UseForHashCode,
                objectConfig.UseForToString
            );
        private ListOfLiteralsParameter ConvertToLiteralListParameter(LiteralListGenericConfig literalListConfig, GenericParameter parameter) 
            => _parameterFactory.GetListOfLiteralsParameter
            (
                parameter.Name,
                parameter.IsOptional,
                parameter.Comments,
                literalListConfig.LiteralType,
                literalListConfig.ListType,
                literalListConfig.Control,
                literalListConfig.ElementControl,
                literalListConfig.PropertySource,
                literalListConfig.PropertySourceParameter,
                literalListConfig.DefaultValues,
                MiscellaneousConstants.DEFAULT_PARAMETER_DELIMITERS,
                literalListConfig.Domain
            );
        private ListOfObjectsParameter ConvertToObjectListParameter(ObjectListGenericConfig literalConfig, GenericParameter parameter) 
            => _parameterFactory.GetListOfObjectsParameter
            (
                parameter.Name,
                parameter.IsOptional,
                parameter.Comments,
                literalConfig.ObjectType,
                literalConfig.ListType,
                literalConfig.Control
            );
        #endregion Parameters from Generic Parameter

        #region Parameters from List of Generic Parameters
        private ListOfLiteralsParameter ConvertToLiteralListParameter(LiteralGenericConfig literalConfig, ListOfGenericsParameter parameter) 
            => _parameterFactory.GetListOfLiteralsParameter
            (
                parameter.Name,
                parameter.IsOptional,
                parameter.Comments,
                literalConfig.LiteralType,
                parameter.ListType,
                parameter.Control,
                literalConfig.Control,
                literalConfig.PropertySource,
                literalConfig.PropertySourceParameter,
                new List<string>() { literalConfig.DefaultValue },
                MiscellaneousConstants.DEFAULT_PARAMETER_DELIMITERS,
                literalConfig.Domain
            );
        private ListOfObjectsParameter ConvertToObjectListParameter(ObjectGenericConfig objectConfig, ListOfGenericsParameter parameter) 
            => _parameterFactory.GetListOfObjectsParameter
            (
                parameter.Name,
                parameter.IsOptional,
                parameter.Comments,
                objectConfig.ObjectType,
                parameter.ListType,
                parameter.Control
            );
        private ListOfObjectsParameter ConvertToObjectListParameter(LiteralListGenericConfig literalListConfig, ListOfGenericsParameter parameter, ApplicationTypeInfo application)
        {
            if (!_typeLoadHelper.TryGetSystemType(literalListConfig, application, out Type? literalListConfigType))
                throw _exceptionHelper.CriticalException("{55C3F831-F053-4DC8-8691-5501A96EDC33}");
                //generic types should be validated before conversion

            return _parameterFactory.GetListOfObjectsParameter
            (
                parameter.Name,
                parameter.IsOptional,
                parameter.Comments,
                _typeHelper.ToId(literalListConfigType),
                parameter.ListType,
                parameter.Control
            );
        }

        private ListOfObjectsParameter ConvertToObjectListParameter(ObjectListGenericConfig objectListConfig, ListOfGenericsParameter parameter, ApplicationTypeInfo application)
        {
            if (!_typeLoadHelper.TryGetSystemType(objectListConfig, application, out Type? objectListConfigType))
                throw _exceptionHelper.CriticalException("{9104C719-9142-4B57-9E80-BB8931EBCE13}");
                //generic types should be validated before conversion

            return _parameterFactory.GetListOfObjectsParameter
            (
                parameter.Name,
                parameter.IsOptional,
                parameter.Comments,
                _typeHelper.ToId(objectListConfigType),
                parameter.ListType,
                parameter.Control
            );
        }
        #endregion Parameters from List of  Generic Parameters
    }
}
