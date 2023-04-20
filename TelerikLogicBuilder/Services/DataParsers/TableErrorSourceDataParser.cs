using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class TableErrorSourceDataParser : ITableErrorSourceDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IStructuresFactory _structuresFactory;

        public TableErrorSourceDataParser(
            IExceptionHelper exceptionHelper,
            IStructuresFactory structuresFactory)
        {
            _exceptionHelper = exceptionHelper;
            _structuresFactory = structuresFactory;
        }

        public TableErrorSourceData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.TABLEERRORSOURCE)
                throw _exceptionHelper.CriticalException("{5D15167F-D61D-4B7F-9B8B-F2C542DCC156}");

            return _structuresFactory.GetTableErrorSourceData
            (
                xmlElement.Attributes[XmlDataConstants.FILEFULLNAMEATTRIBUTE]!.Value,
                int.Parse(xmlElement.Attributes[XmlDataConstants.ROWINDEXATTRIBUTE]!.Value, CultureInfo.InvariantCulture),
                int.Parse(xmlElement.Attributes[XmlDataConstants.COLUMNINDEXATTRIBUTE]!.Value, CultureInfo.InvariantCulture)
            );
        }
    }
}
