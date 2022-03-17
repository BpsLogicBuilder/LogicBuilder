using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation
{
    internal interface IConstructorGenericsConfigrationValidator
    {
        bool Validate(Constructor constructor, List<GenericConfigBase> genericArguments, ApplicationTypeInfo application, List<string> validationErrors);
    }
}
