using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Constructors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories
{
    internal class ChildConstructorFinderFactory : IChildConstructorFinderFactory
    {
        public IChildConstructorFinder GetChildConstructorFinder(IDictionary<string, Constructor> existingConstructors)
            => new ChildConstructorFinder
            (
                Program.ServiceProvider.GetRequiredService<IConstructorManager>(),
                Program.ServiceProvider.GetRequiredService<IParametersManager>(),
                Program.ServiceProvider.GetRequiredService<IReflectionHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<IStringHelper>(),
                Program.ServiceProvider.GetRequiredService<IMemberAttributeReader>(),
                existingConstructors
            );
    }
}
