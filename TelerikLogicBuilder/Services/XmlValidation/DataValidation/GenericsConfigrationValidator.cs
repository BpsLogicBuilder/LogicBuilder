using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class GenericsConfigrationValidator : IGenericsConfigrationValidator
    {
        public bool GenericArgumentNameMismatch(List<string> configured, List<GenericConfigBase> data)
        {
            IEnumerable<string> missingFromConfig = data.Select(a => a.GenericArgumentName).Except(configured);
            IEnumerable<string> missingFromData = configured.Except(data.Select(a => a.GenericArgumentName));

            return missingFromConfig.Any() || missingFromData.Any();
        }

        public bool GenericArgumentCountMatchesType(Type objectType, List<GenericConfigBase> GenericArguments) 
            => objectType.IsGenericTypeDefinition
                && (objectType.GetGenericArguments().Length == GenericArguments.Count);
    }
}
