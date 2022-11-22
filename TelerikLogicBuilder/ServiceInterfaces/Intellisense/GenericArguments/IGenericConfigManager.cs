using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments
{
    internal interface IGenericConfigManager
    {
        GenericConfigBase CreateGenericConfig(string genericArgumentName, Type genericTypeArgument);
        IList<GenericConfigBase> CreateGenericConfigs(IList<string> configuredGenericArgumentNames, IList<Type> arguments);
        LiteralGenericConfig GetDefaultLiteralGenericConfig(string genericArgumentName);
        LiteralListGenericConfig GetDefaultLiteralListGenericConfig(string genericArgumentName);
        ObjectGenericConfig GetDefaultObjectGenericConfig(string genericArgumentName);
        ObjectListGenericConfig GetDefaultObjectListGenericConfig(string genericArgumentName);
        IList<GenericConfigBase> ReconcileGenericArguments(IList<string> configuredGenericArgumentNames, IList<GenericConfigBase> dataConfigGenericArguments);
    }
}
