using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ResultMessageBuilder : IResultMessageBuilder
    {
        private readonly IStructuresFactory _structuresFactory;

        public ResultMessageBuilder(IStructuresFactory structuresFactory)
        {
            _structuresFactory = structuresFactory;
        }

        public ResultMessage BuilderMessage(VisioFileSource source, string message) 
            => new
            (
                _structuresFactory.GetDiagramErrorSourceData
                (
                    source.SourceFileFullname,
                    source.PageIndex,
                    source.ShapeIndex,
                    source.PageId,
                    source.ShapeId,
                    source.ShapeMasterName
                ).ToXml,
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.diagramVisibleLinkFormat,
                    source.FileName,
                    source.PageIndex,
                    source.ShapeMasterName,
                    source.ShapeIndex
                ),
                message
            );

        public ResultMessage BuilderMessage(TableFileSource source, string message)
            => new
            (
                _structuresFactory.GetTableErrorSourceData
                (
                    source.SourceFileFullname,
                    source.Row,
                    source.Column
                ).ToXml,
                string.Format
                (
                    CultureInfo.CurrentCulture,
                    Strings.tableVisibleLinkFormat,
                    source.FileName,
                    source.Row,
                    source.Column
                ),
                message
            );

        public ResultMessage BuilderMessage(string message) => new(message);
    }
}
