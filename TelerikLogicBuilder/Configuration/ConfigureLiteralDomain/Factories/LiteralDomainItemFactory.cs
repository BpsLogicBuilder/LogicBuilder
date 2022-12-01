using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralDomain.Factories
{
    internal class LiteralDomainItemFactory : ILiteralDomainItemFactory
    {
        private readonly Func<string, Type, LiteralDomainItem> _getLiteralDomainItem;

        public LiteralDomainItemFactory(Func<string, Type, LiteralDomainItem> getLiteralDomainItem)
        {
            _getLiteralDomainItem = getLiteralDomainItem;
        }

        public LiteralDomainItem GetLiteralDomainItem(string item, Type type)
            => _getLiteralDomainItem(item, type);
    }
}
