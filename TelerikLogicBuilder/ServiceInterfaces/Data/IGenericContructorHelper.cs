using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data
{
    internal interface IGenericContructorHelper
    {
        Constructor ConvertGenericTypes(Constructor constructor, IList<GenericConfigBase> genericParameters, ApplicationTypeInfo application);
        Type MakeGenericType(Constructor constructor, IList<GenericConfigBase> consDataGenericArguments, ApplicationTypeInfo application);
    }
}
