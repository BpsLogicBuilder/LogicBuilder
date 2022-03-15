using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class TypeLoadHelper : ITypeLoadHelper
    {
        private readonly IAssemblyLoadContextManager _assemblyLoadContextService;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public TypeLoadHelper(IContextProvider contextProvider, IAssemblyLoadContextManager assemblyLoadContextService)
        {
            _assemblyLoadContextService = assemblyLoadContextService;
            _enumHelper = contextProvider.EnumHelper;
            _exceptionHelper = contextProvider.ExceptionHelper;
        }

        public bool TryGetSystemType(GenericConfigBase config, ApplicationTypeInfo application, out Type? type)
        {
            return (type = GetType()) != null;

            Type? GetType() 
                => config switch
                {
                    LiteralGenericConfig literalGenericConfig => GetLiteralType(literalGenericConfig),
                    ObjectGenericConfig objectGenericConfig => GetObjectType(objectGenericConfig),
                    LiteralListGenericConfig literalListGenericConfig => GetLiteralListType(literalListGenericConfig),
                    ObjectListGenericConfig objectListGenericConfig => GetObjectListType(objectListGenericConfig),
                    _ => throw _exceptionHelper.CriticalException("{D7C6E5E6-CA80-4A52-B6EA-A889D7A145B5}")
                };

            Type? GetLiteralType(LiteralGenericConfig literalGenericConfig) 
                => _enumHelper.GetSystemType(literalGenericConfig.LiteralType);
            Type? GetObjectType(ObjectGenericConfig objectGenericConfig)
            {
                TryGetSystemType(objectGenericConfig.ObjectType, application, out Type? objectType);
                return objectType;
            }
            Type? GetLiteralListType(LiteralListGenericConfig literalListGenericConfig)
                => _enumHelper.GetSystemType
                (
                    literalListGenericConfig.ListType,
                    _enumHelper.GetSystemType(literalListGenericConfig.LiteralType)
                );
            Type? GetObjectListType(ObjectListGenericConfig objectListGenericConfig)
            {
                TryGetSystemType(objectListGenericConfig.ObjectType, application, out Type? objectElementType);
                if (objectElementType == null)
                    return null;

                return _enumHelper.GetSystemType
                (
                    objectListGenericConfig.ListType,
                    objectElementType
                );
            }
        }

        public bool TryGetSystemType(ParameterBase parameter, ApplicationTypeInfo application, out Type? type)
        {
            return (type = GetType()) != null;

            Type? GetType() 
                => parameter switch
                {
                    LiteralParameter literalParameter => GetLiteralType(literalParameter),
                    ObjectParameter objectParameter => GetObjectType(objectParameter),
                    ListOfLiteralsParameter literalListParameter => GetLiteralListType(literalListParameter),
                    ListOfObjectsParameter objectListParameter => GetObjectListType(objectListParameter),
                    _ => throw _exceptionHelper.CriticalException("{077EB222-5EE8-4C72-A0DA-0398C3D1D9F0}")
                };

            Type? GetLiteralType(LiteralParameter literalParameter)
                => _enumHelper.GetSystemType(literalParameter.LiteralType);
            Type? GetObjectType(ObjectParameter objectParameter)
            {
                TryGetSystemType(objectParameter.ObjectType, application, out Type? objectType);
                return objectType;
            }
            Type? GetLiteralListType(ListOfLiteralsParameter literalListParameter)
                => _enumHelper.GetSystemType
                (
                    literalListParameter.ListType,
                    _enumHelper.GetSystemType(literalListParameter.LiteralType)
                );
            Type? GetObjectListType(ListOfObjectsParameter objectListParameter)
            {
                TryGetSystemType(objectListParameter.ObjectType, application, out Type? objectElementType);
                if (objectElementType == null)
                    return null;

                return _enumHelper.GetSystemType
                (
                    objectListParameter.ListType,
                    objectElementType
                );
            }
        }

        public bool TryGetSystemType(ReturnTypeBase returnType, IList<GenericConfigBase> genericArguments, ApplicationTypeInfo application, out Type? type)
        {
            IDictionary<string, GenericConfigBase> funcDataGenericArguments = genericArguments.ToDictionary(g => g.GenericArgumentName);

            return (type = GetType()) != null;

            Type? GetType() 
                => returnType switch
                {
                    LiteralReturnType literalReturnType => GetLiteralType(literalReturnType),
                    ObjectReturnType objectReturnType => GetObjectType(objectReturnType),
                    GenericReturnType genericReturnType => GetClosedGenericType(genericReturnType),
                    ListOfLiteralsReturnType listOfLiteralsReturnType => GetLiteralListType(listOfLiteralsReturnType),
                    ListOfObjectsReturnType listOfObjectsReturnType => GetObjectListType(listOfObjectsReturnType),
                    ListOfGenericsReturnType listOfGenericsReturnType => GetClosedGenericListType(listOfGenericsReturnType),
                    _ => throw _exceptionHelper.CriticalException("{7EC2305D-193F-4626-BEA0-9688BB527354}"),
                };

            Type? GetLiteralType(LiteralReturnType literalReturnType)
                => _enumHelper.GetSystemType(literalReturnType.ReturnType);
            Type? GetObjectType(ObjectReturnType objectReturnType)
            {
                TryGetSystemType(objectReturnType.ObjectType, application, out Type? objectType);
                return objectType;
            }
            Type? GetClosedGenericType(GenericReturnType objectReturnType)
            {
                TryGetSystemType(funcDataGenericArguments[objectReturnType.GenericArgumentName], application, out Type? genericArgumentType);
                return genericArgumentType;
            }
            Type? GetLiteralListType(ListOfLiteralsReturnType literalListReturnType)
                => _enumHelper.GetSystemType
                (
                    literalListReturnType.ListType,
                    _enumHelper.GetSystemType(literalListReturnType.UnderlyingLiteralType)
                );
            Type? GetObjectListType(ListOfObjectsReturnType objectListReturnType)
            {
                TryGetSystemType(objectListReturnType.ObjectType, application, out Type? objectElementType);
                if (objectElementType == null)
                    return null;

                return _enumHelper.GetSystemType
                (
                    objectListReturnType.ListType,
                    objectElementType
                );
            }
            Type? GetClosedGenericListType(ListOfGenericsReturnType objectListReturnType)
            {
                TryGetSystemType(funcDataGenericArguments[objectListReturnType.GenericArgumentName], application, out Type? objectElementType);
                if (objectElementType == null)
                    return null;

                return _enumHelper.GetSystemType
                (
                    objectListReturnType.ListType,
                    objectElementType
                );
            }
        }

        public bool TryGetSystemType(string typeName, ApplicationTypeInfo application, out Type? type)
        {
            if (application.AssemblyAvailable
                    && application.AllTypes.TryGetValue(typeName, out type))
                return true;

            try
            {
                if ((type = Type.GetType(typeName, ResolveAssembly, ResolveType)) != null)
                    return true;
            }
            catch (FileLoadException)
            {
                type = null;
                return false;
            }

            return false;

            Assembly? ResolveAssembly(AssemblyName assemblyName)
            {
                if (application.AllAssembliesDictionary.TryGetValue(assemblyName.FullName, out Assembly? assembly))
                    return assembly;

                if (typeof(string).Assembly.GetName().Name == assemblyName.Name)
                    return typeof(string).Assembly;

                return LoadAssembly(assemblyName);
            }

            static Type? ResolveType(Assembly? assembly, string typeName, bool ignoreCase)
            {
                if (assembly != null)
                    return assembly.GetType(typeName);

                return Type.GetType(typeName, false, ignoreCase);
            }

            Assembly? LoadAssembly(AssemblyName assemblyName)
            {
                try
                {
                    return _assemblyLoadContextService.GetAssemblyLoadContext().LoadFromAssemblyName(assemblyName);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public bool TryGetSystemType(VariableBase variable, ApplicationTypeInfo application, out Type? variableType)
        {
            return (variableType = GetType()) != null;

            Type? GetType()
            {
                if (variable.CastVariableAs != MiscellaneousConstants.TILDE && !string.IsNullOrEmpty(variable.CastVariableAs))
                {
                    if (TryGetSystemType(variable.CastVariableAs, application, out Type? variableType))
                        return variableType;
                    else
                        return null;
                }

                return variable switch
                {
                    LiteralVariable literalVariable => GetLiteralType(literalVariable),
                    ObjectVariable objectVariable => GetObjectType(objectVariable),
                    ListOfLiteralsVariable literalListVariable => GetLiteralListType(literalListVariable),
                    ListOfObjectsVariable objectListVariable => GetObjectListType(objectListVariable),
                    _ => throw _exceptionHelper.CriticalException("{9933A0F1-2D15-4867-AB58-FEEBC3CAB43E}")
                };
            }

            Type? GetLiteralType(LiteralVariable literalVariable)
                => _enumHelper.GetSystemType(literalVariable.LiteralType);
            Type? GetObjectType(ObjectVariable objectVariable)
            {
                TryGetSystemType(objectVariable.ObjectType, application, out Type? objectType);
                return objectType;
            }
            Type? GetLiteralListType(ListOfLiteralsVariable literalListVariable)
                => _enumHelper.GetSystemType
                (
                    literalListVariable.ListType,
                    _enumHelper.GetSystemType(literalListVariable.LiteralType)
                );
            Type? GetObjectListType(ListOfObjectsVariable objectListVariable)
            {
                TryGetSystemType(objectListVariable.ObjectType, application, out Type? objectElementType);
                if (objectElementType == null)
                    return null;

                return _enumHelper.GetSystemType
                (
                    objectListVariable.ListType,
                    objectElementType
                );
            }
        }
    }
}
