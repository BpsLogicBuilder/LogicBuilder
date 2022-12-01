using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureLiteralListDefaultValue.Factories
{
    internal class LiteralListDefaultValueItemFactory : ILiteralListDefaultValueItemFactory
    {
        private readonly Func<string, Type, LiteralListDefaultValueItem> _getLiteralListDefaultValueItem;

        public LiteralListDefaultValueItemFactory(
            Func<string, Type, LiteralListDefaultValueItem> getLiteralListDefaultValueItem)
        {
            _getLiteralListDefaultValueItem = getLiteralListDefaultValueItem;
        }

        public LiteralListDefaultValueItem GetLiteralListDefaultValueItem(string item, Type type)
            => _getLiteralListDefaultValueItem(item, type);
    }
}
