﻿using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    abstract internal class ParameterNodeInfoBase
    {
        internal ParameterNodeInfoBase(ParameterInfo pInfo, IParameterAttributeReader parameterAttributeReader)
        {
            this.PInfo = pInfo;
            _parameterAttributeReader = parameterAttributeReader;
        }

        private readonly IParameterAttributeReader _parameterAttributeReader;

        #region Properties
        internal ParameterInfo PInfo { get; }

        internal string Name => this.PInfo.Name;

        /// <summary>
        /// Is the parameter optional
        /// </summary>
        internal bool IsOptional => PInfo.IsOptional;

        /// <summary>
        /// Comments
        /// </summary>
        internal string Comments => _parameterAttributeReader.GetComments(PInfo);

        abstract internal ParameterBase Parameter { get; }
        #endregion Properties

        #region Methods
        internal static ParameterNodeInfoBase Create(ParameterInfo pInfo, IContextProvider contextProvider)
        {
            if (contextProvider.TypeHelper.IsLiteralType(pInfo.ParameterType))
                return new LiteralParameterNodeInfo(pInfo, contextProvider);
            else if (pInfo.ParameterType.IsGenericParameter)
                return new GenericParameterNodeInfo(pInfo, contextProvider);
            else if (contextProvider.TypeHelper.IsValidList(pInfo.ParameterType))
            {
                Type underlyingType = contextProvider.TypeHelper.GetUndelyingTypeForValidList(pInfo.ParameterType);
                if (contextProvider.TypeHelper.IsLiteralType(underlyingType))
                    return new ListOfLiteralsParameterNodeInfo(pInfo, contextProvider);
                else if (underlyingType.IsGenericParameter)
                    return new ListOfGenericsParameterNodeInfo(pInfo, contextProvider);
                else
                    return new ListOfObjectsParameterNodeInfo(pInfo, contextProvider);
            }
            else if (pInfo.ParameterType.IsAbstract || pInfo.ParameterType.IsInterface || pInfo.ParameterType.IsEnum)
            {//keeping these separate form the regular concrete type below - may need further work
                return new ObjectParameterNodeInfo(pInfo, contextProvider);
            }
            else
            {
                return new ObjectParameterNodeInfo(pInfo, contextProvider);
            }
        }
        #endregion Methods
    }
}
