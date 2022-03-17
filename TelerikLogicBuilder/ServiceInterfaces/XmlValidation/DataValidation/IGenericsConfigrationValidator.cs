using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IGenericsConfigrationValidator
    {
        bool GenericArgumentNameMismatch(List<string> configured, List<GenericConfigBase> data);
        bool GenericArgumentCountMatchesType(Type objectType, List<GenericConfigBase> GenericArguments);
    }
}
