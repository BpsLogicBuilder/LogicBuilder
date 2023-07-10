using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories
{
    internal class ConstructorFactory : IConstructorFactory
    {
        public Constructor GetConstructor(string name, string typeName, List<ParameterBase> parameters, List<string> genericArguments, string summary)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                name,
                typeName,
                parameters,
                genericArguments,
                summary
            );
    }
}
