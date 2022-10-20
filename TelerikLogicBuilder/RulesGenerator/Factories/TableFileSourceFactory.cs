using System;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal class TableFileSourceFactory : ITableFileSourceFactory
    {
        private readonly Func<string, int, int, TableFileSource> _getTableFileSource;

        public TableFileSourceFactory(Func<string, int, int, TableFileSource> getTableFileSource)
        {
            _getTableFileSource = getTableFileSource;
        }

        public TableFileSource GetTableFileSource(string sourceFileFullname, int row, int column)
            => _getTableFileSource(sourceFileFullname, row, column);
    }
}
