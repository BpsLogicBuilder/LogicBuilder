using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IGenericsConfigrationValidator
    {
        bool GenericArgumentNameMismatch(IList<string> configured, IList<GenericConfigBase> data);
        bool GenericArgumentCountMatchesType(Type objectType, IList<GenericConfigBase> GenericArguments);
    }
}
