using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Data
{
    internal class GenericReturnTypeHelper : IGenericReturnTypeHelper
    {
        private readonly IContextProvider _contextProvider;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly ITypeHelper _typeHelper;
        private readonly ITypeLoadHelper _typeLoadHelper;

        public GenericReturnTypeHelper(IContextProvider contextProvider, ITypeLoadHelper typeLoadHelper)
        {
            _enumHelper = contextProvider.EnumHelper;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _typeHelper = contextProvider.TypeHelper;
            _contextProvider = contextProvider;
            _typeLoadHelper = typeLoadHelper;
        }

        public ReturnTypeBase GetConvertedReturnType(ReturnTypeBase returnType, IList<GenericConfigBase> genericParametersDictionary, ApplicationTypeInfo application)
        {
            return returnType switch
            {
                GenericReturnType genericReturnType => GetConvertedGenericReturnType(genericReturnType, genericParametersDictionary),
                ListOfGenericsReturnType listOfGenericsReturnType => GetConvertedListOfGenericsReturnType(listOfGenericsReturnType, genericParametersDictionary, application),
                _ => returnType,
            };
        }

        private ReturnTypeBase GetConvertedGenericReturnType(GenericReturnType returnType, IList<GenericConfigBase> genericParameters)
        {
            var genericParametersDictionary = genericParameters.ToDictionary(gp => gp.GenericArgumentName);
            return genericParametersDictionary[returnType.GenericArgumentName] switch
            {
                LiteralGenericConfig literalGenericConfig => ConvertToLiteralReturn(literalGenericConfig),
                ObjectGenericConfig objectGenericConfig => ConvertToObjectReturn(objectGenericConfig),
                LiteralListGenericConfig literalListGenericConfig => ConvertToLiteralListReturn(literalListGenericConfig),
                ObjectListGenericConfig objectListGenericConfig => ConvertToObjectListReturn(objectListGenericConfig),
                _ => throw _exceptionHelper.CriticalException("{86CC339F-0AC3-4CEB-825B-C7380202D10A}"),
            };
        }

        private ReturnTypeBase GetConvertedListOfGenericsReturnType(ListOfGenericsReturnType returnType, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application)
        {
            var genericParametersDictionary = genericParameters.ToDictionary(gp => gp.GenericArgumentName);
            return genericParametersDictionary[returnType.GenericArgumentName] switch
            {
                LiteralGenericConfig literalGenericConfig => ConvertToLiteralListReturn(literalGenericConfig, returnType),
                ObjectGenericConfig objectGenericConfig => ConvertToObjectListReturn(objectGenericConfig, returnType),
                LiteralListGenericConfig literalListGenericConfig => ConvertToObjectListReturn(literalListGenericConfig, returnType, application),
                ObjectListGenericConfig objectListGenericConfig => ConvertToObjectListReturn(objectListGenericConfig, returnType, application),
                _ => throw _exceptionHelper.CriticalException("{F845BAF5-1177-4DEF-B572-1E14C12CDA41}"),
            };
        }

        #region Return Types from Generic Parameter
        private LiteralReturnType ConvertToLiteralReturn(LiteralGenericConfig literalConfig) 
            => new
            (
                _enumHelper.GetLiteralFunctionReturnType
                (
                    _enumHelper.GetSystemType(literalConfig.LiteralType)
                ), 
                _contextProvider
            );

        private ObjectReturnType ConvertToObjectReturn(ObjectGenericConfig objectConfig) 
            => new
            (
                objectConfig.ObjectType,
                _contextProvider
            );

        private ListOfLiteralsReturnType ConvertToLiteralListReturn(LiteralListGenericConfig literalListConfig)
            => new
            (
                _enumHelper.GetLiteralFunctionReturnType(_enumHelper.GetSystemType(literalListConfig.LiteralType)),
                literalListConfig.ListType,
                _contextProvider
            );

        private ListOfObjectsReturnType ConvertToObjectListReturn(ObjectListGenericConfig objectListConfig) 
            => new
            (
                objectListConfig.ObjectType,
                objectListConfig.ListType,
                _contextProvider
            );
        #endregion Return Types from Generic Parameter

        #region  Return Types from List Of Generics Parameter
        private ListOfLiteralsReturnType ConvertToLiteralListReturn(LiteralGenericConfig literalConfig, ListOfGenericsReturnType returnType) 
            => new
            (
                _enumHelper.GetLiteralFunctionReturnType
                (
                    _enumHelper.GetSystemType(literalConfig.LiteralType)
                ),
                returnType.ListType,
                _contextProvider
            );

        private ListOfObjectsReturnType ConvertToObjectListReturn(ObjectGenericConfig literalConfig, ListOfGenericsReturnType returnType) 
            => new
            (
                literalConfig.ObjectType,
                returnType.ListType,
                _contextProvider
            );

        private ListOfObjectsReturnType ConvertToObjectListReturn(LiteralListGenericConfig literalListConfig, ListOfGenericsReturnType returnType, ApplicationTypeInfo application)
        {
            if (!_typeLoadHelper.TryGetSystemType(literalListConfig, application, out Type? literalListConfigType))
                throw _exceptionHelper.CriticalException("{4D84D9CB-3BAC-45CA-966F-C87354208E43}");
                //generic types should be validated before conversion

            return new
            (
                _typeHelper.ToId(literalListConfigType),
                returnType.ListType,
                _contextProvider
            );
        }

        private ListOfObjectsReturnType ConvertToObjectListReturn(ObjectListGenericConfig objectListConfig, ListOfGenericsReturnType returnType, ApplicationTypeInfo application)
        {
            if (!_typeLoadHelper.TryGetSystemType(objectListConfig, application, out Type? objectListConfigType))
                throw _exceptionHelper.CriticalException("{4AD84CE6-D34A-4158-8FD6-2F2806587A66}");
                //generic types should be validated before conversion

            return new
            (
                _typeHelper.ToId(objectListConfigType),
                returnType.ListType,
                _contextProvider
            );
        }
        #endregion Return Types from List Of Generics Parameter
    }
}
