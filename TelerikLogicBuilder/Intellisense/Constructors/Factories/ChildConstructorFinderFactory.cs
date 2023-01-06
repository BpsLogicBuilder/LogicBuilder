using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Constructors;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors.Factories
{
    internal class ChildConstructorFinderFactory : IChildConstructorFinderFactory
    {
        private readonly Func<IDictionary<string, Constructor>, IChildConstructorFinder> _getChildConstructorFinder;

        public ChildConstructorFinderFactory(Func<IDictionary<string, Constructor>, IChildConstructorFinder> getChildConstructorFinder)
        {
            _getChildConstructorFinder = getChildConstructorFinder;
        }

        public IChildConstructorFinder GetChildConstructorFinder(IDictionary<string, Constructor> existingConstructors)
            => _getChildConstructorFinder(existingConstructors);
    }
}
