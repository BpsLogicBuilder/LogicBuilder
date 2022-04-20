using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class TableErrorSourceDataParser : ITableErrorSourceDataParser
    {
        private readonly IContextProvider _contextProvider;
        private readonly IExceptionHelper _exceptionHelper;

        public TableErrorSourceDataParser(IContextProvider contextProvider)
        {
            _exceptionHelper = contextProvider.ExceptionHelper;
            _contextProvider = contextProvider;
        }

        public TableErrorSourceData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.TABLEERRORSOURCE)
                throw _exceptionHelper.CriticalException("{5D15167F-D61D-4B7F-9B8B-F2C542DCC156}");

            return new TableErrorSourceData
            (
                xmlElement.GetAttribute(XmlDataConstants.FILEFULLNAMEATTRIBUTE),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.ROWINDEXATTRIBUTE), CultureInfo.InvariantCulture),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.COLUMNINDEXATTRIBUTE), CultureInfo.InvariantCulture),
                _contextProvider
            );
        }
    }
}
