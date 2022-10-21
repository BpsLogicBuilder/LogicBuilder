using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class DiagramErrorSourceDataParser : IDiagramErrorSourceDataParser
    {
        private readonly IContextProvider _contextProvider;
        private readonly IExceptionHelper _exceptionHelper;

        public DiagramErrorSourceDataParser(IContextProvider contextProvider)
        {
            _exceptionHelper = contextProvider.ExceptionHelper;
            _contextProvider = contextProvider;
        }

        public DiagramErrorSourceData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.DIAGRAMERRORSOURCE)
                throw _exceptionHelper.CriticalException("{A5804289-F2BE-4B8E-9A2E-288593E9289E}");

            return new DiagramErrorSourceData
            (
                xmlElement.GetAttribute(XmlDataConstants.FILEFULLNAMEATTRIBUTE),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.PAGEINDEXATTRIBUTE), CultureInfo.InvariantCulture),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.SHAPEINDEXATTRIBUTE), CultureInfo.InvariantCulture),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.PAGEIDATTRIBUTE), CultureInfo.InvariantCulture),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.SHAPEIDATTRIBUTE), CultureInfo.InvariantCulture),
                _contextProvider.XmlDocumentHelpers
            );
        }
    }
}
