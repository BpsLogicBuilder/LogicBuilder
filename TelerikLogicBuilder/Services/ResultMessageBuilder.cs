using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class ResultMessageBuilder : IResultMessageBuilder
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ResultMessageBuilder(IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public ResultMessage BuilderMessage(VisioFileSource source, string message) 
            => new
            (
                new DiagramErrorSourceData
                (
                    source.SourceFileFullname,
                    source.PageIndex,
                    source.ShapeIndex,
                    source.PageId,
                    source.ShapeId,
                    _xmlDocumentHelpers
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
                new TableErrorSourceData
                (
                    source.SourceFileFullname,
                    source.Row,
                    source.Column,
                    _xmlDocumentHelpers
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
