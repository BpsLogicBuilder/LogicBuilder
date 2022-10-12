using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System;
using System.Globalization;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class ReturnTypeManager : IReturnTypeManager
    {
        private readonly IReturnTypeInfoFactory _returnTypeInfoFactory;
        private readonly ITypeHelper _typeHelper;

        public ReturnTypeManager(IReturnTypeInfoFactory returnTypeInfoFactory, ITypeHelper typeHelper)
        {
            _returnTypeInfoFactory = returnTypeInfoFactory;
            _typeHelper = typeHelper;
        }

        public ReturnTypeInfoBase GetReturnTypeInfo(MethodInfo methodInfo)
        {
            Type memberType = methodInfo.ReturnType;
            if (methodInfo.IsGenericMethod)
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.genericMethodsNotSupportedFormat, methodInfo.Name));
            }
            else if (_typeHelper.IsValidLiteralReturnType(memberType))
                return _returnTypeInfoFactory.GetLiteralReturnTypeInfo(methodInfo);
            else if (memberType.IsGenericParameter)
                return _returnTypeInfoFactory.GetGenericReturnTypeInfo(methodInfo);
            else if (_typeHelper.IsValidList(memberType))
            {
                Type underlyingType = _typeHelper.GetUndelyingTypeForValidList(memberType);
                if (_typeHelper.IsValidLiteralReturnType(underlyingType))
                    return _returnTypeInfoFactory.GetListOfLiteralsReturnTypeInfo(methodInfo);
                else if (underlyingType.IsGenericParameter)
                    return _returnTypeInfoFactory.GetListOfGenericsReturnTypeInfo(methodInfo);
                else
                    return _returnTypeInfoFactory.GetListOfObjectsReturnTypeInfo(methodInfo);
            }
            else if (memberType.IsAbstract || memberType.IsInterface || memberType.IsEnum)
            {//keeping these separate form the regular concrete type below - may need further work
                return _returnTypeInfoFactory.GetObjectReturnTypeInfo(methodInfo);
            }
            else
            {
                return _returnTypeInfoFactory.GetObjectReturnTypeInfo(methodInfo);
            }
        }
    }
}
