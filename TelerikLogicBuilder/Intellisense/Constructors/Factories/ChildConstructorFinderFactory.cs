using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories
{
    internal class ChildConstructorFinderFactory : IChildConstructorFinderFactory
    {
        private readonly Func<Dictionary<string, Constructor>, IChildConstructorFinder> _getChildConstructorFinder;

        public ChildConstructorFinderFactory(Func<Dictionary<string, Constructor>, IChildConstructorFinder> getChildConstructorFinder)
        {
            _getChildConstructorFinder = getChildConstructorFinder;
        }

        public IChildConstructorFinder GetChildConstructorFinder(Dictionary<string, Constructor> existingConstructors)
            => _getChildConstructorFinder(existingConstructors);
    }
}
