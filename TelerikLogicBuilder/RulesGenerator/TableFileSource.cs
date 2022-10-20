using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator
{
    internal class TableFileSource
    {
        public TableFileSource(IPathHelper pathHelper, string sourceFileFullname, int row, int column)
        {
            SourceFileFullname = sourceFileFullname;
            FileName = pathHelper.GetFileName(sourceFileFullname);
            Row = row;
            Column = column;
        }

        internal string SourceFileFullname { get; }
        internal string FileName { get; }
        internal int Row { get; }
        internal int Column { get; }
    }
}
