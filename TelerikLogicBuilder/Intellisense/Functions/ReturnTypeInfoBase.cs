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
    }
}
