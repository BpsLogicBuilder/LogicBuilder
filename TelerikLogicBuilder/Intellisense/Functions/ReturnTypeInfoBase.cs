using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Globalization;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    abstract internal class ReturnTypeInfoBase
    {
        internal ReturnTypeInfoBase(MethodInfo mInfo)
        {
            this.MInfo = mInfo;
        }

        #region Properties
        internal MethodInfo MInfo { get; private set; }
        internal string Name => this.MInfo.Name;
        abstract internal ReturnTypeBase GetReturnType();
        #endregion Properties

        #region Methods
        internal static ReturnTypeInfoBase Create(MethodInfo mInfo, IContextProvider contextProvider)
        {
            Type memberType = mInfo.ReturnType;
            if (mInfo.IsGenericMethod)
            {
                throw new LogicBuilderException(string.Format(CultureInfo.CurrentCulture, Strings.genericMethodsNotSupportedFormat, mInfo.Name));
            }
            else if (contextProvider.TypeHelper.IsValidLiteralReturnType(memberType))
                return new LiteralReturnTypeInfo(mInfo, contextProvider);
            else if (memberType.IsGenericParameter)
                return new GenericReturnTypeInfo(mInfo, contextProvider);
            else if (contextProvider.TypeHelper.IsValidList(memberType))
            {
                Type underlyingType = contextProvider.TypeHelper.GetUndelyingTypeForValidList(memberType);
                if (contextProvider.TypeHelper.IsValidLiteralReturnType(underlyingType))
                    return new ListOfLiteralsReturnTypeInfo(mInfo, contextProvider);
                else if (underlyingType.IsGenericParameter)
                    return new ListOfGenericsReturnTypeInfo(mInfo, contextProvider);
                else
                    return new ListOfObjectsReturnTypeInfo(mInfo, contextProvider);
            }
            else if (memberType.IsAbstract || memberType.IsInterface || memberType.IsEnum)
            {//keeping these separate form the regular concrete type below - may need further work
                return new ObjectReturnTypeInfo(mInfo, contextProvider);
            }
            else
            {
                return new ObjectReturnTypeInfo(mInfo, contextProvider);
            }
        }
        #endregion Methods
    }
}
