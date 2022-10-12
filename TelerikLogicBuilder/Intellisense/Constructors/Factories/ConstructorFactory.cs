using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories
{
    internal class ConstructorFactory : IConstructorFactory
    {
        private readonly Func<string, string, List<ParameterBase>, List<string>, string, Constructor> _getConstructor;

        public ConstructorFactory(
            Func<string, string, List<ParameterBase>, List<string>, string, Constructor> getConstructor)
        {
            _getConstructor = getConstructor;
        }

        public Constructor GetConstructor(string name, string typeName, List<ParameterBase> parameters, List<string> genericArguments, string summary)
            => _getConstructor
            (
                name,
                typeName,
                parameters,
                genericArguments,
                summary
            );
    }
}
