using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class DiagramErrorSourceDataParser : IDiagramErrorSourceDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IStructuresFactory _structuresFactory;

        public DiagramErrorSourceDataParser(
            IExceptionHelper exceptionHelper,
            IStructuresFactory structuresFactory)
        {
            _exceptionHelper = exceptionHelper;
            _structuresFactory = structuresFactory;
        }

        public DiagramErrorSourceData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.DIAGRAMERRORSOURCE)
                throw _exceptionHelper.CriticalException("{A5804289-F2BE-4B8E-9A2E-288593E9289E}");

            return _structuresFactory.GetDiagramErrorSourceData
            (
                xmlElement.GetAttribute(XmlDataConstants.FILEFULLNAMEATTRIBUTE),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.PAGEINDEXATTRIBUTE), CultureInfo.InvariantCulture),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.SHAPEINDEXATTRIBUTE), CultureInfo.InvariantCulture),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.PAGEIDATTRIBUTE), CultureInfo.InvariantCulture),
                int.Parse(xmlElement.GetAttribute(XmlDataConstants.SHAPEIDATTRIBUTE), CultureInfo.InvariantCulture)
            );
        }
    }
}
