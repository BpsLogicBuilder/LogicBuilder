using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    /*
     *  For a non-connetor shape, Shape.FromConnects is a collection of Visio.Connects collection of Visio.Connect
	    For a non-connetor shape, a Visio.Connect has a FromSheet whose master is a Connector
	    For a non-connetor shape, a Visio.Connect has a FromPart which is either a VisFromParts.visBegin or VisFromParts.visEnd
		    A VisFromParts.visBegin FromPart connects the shape to a FromSheet connector pointing away from the shape and a VisFromParts.visEnd is the reverse (a FromSheet connector pointing towards the shape)

	    A connector is also a Visio.Shape.
	    For a connector shape, Shape.Connects is also a collection of Visio.Connects collection of Visio.Connect
	    For a connector shape, a Visio.Connect has a ToSheet whose master a non-connetor shape.
	    For a connector shape, a Visio.Connect has a FromPart which is either a VisFromParts.visBegin or VisFromParts.visEnd.
		A VisFromParts.visBegin FromPart connects the connector pointing away from a ToSheet non-connetor shape and a VisFromParts.visEnd is the reverse (connects the connector pointing towards a ToSheet non-connetor shape)
     */
    internal class ShapeHelper : IShapeHelper
    {
        private readonly IConfigurationService _configurationService;
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionsDataParser _functionsDataParser;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ShapeHelper(IConfigurationService configurationService, IConnectorDataParser connectorDataParser, IExceptionHelper exceptionHelper, IFunctionsDataParser functionsDataParser, IShapeXmlHelper shapeXmlHelper, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _configurationService = configurationService;
            _connectorDataParser = connectorDataParser;
            _exceptionHelper = exceptionHelper;
            _functionsDataParser = functionsDataParser;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public short CheckForDuplicateMultipleChoices(Shape fromShape)
        {
            if (fromShape.Master.NameU != UniversalMasterName.DIALOG)
                throw _exceptionHelper.CriticalException("{46979888-7878-47FA-A234-3FD8D0C62793}");

            Dictionary<string, short> connectorTextList = new();
            foreach (Connect shapeFromConnect in fromShape.FromConnects)
            {
                Shape connector = shapeFromConnect.FromSheet;
                string shapeXml = _shapeXmlHelper.GetXmlString(connector);
                if (shapeXml.Length == 0)
                    continue;

                ConnectorData connectorData = _connectorDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(shapeXml)
                );

                if (connectorData.ConnectorCategory != ConnectorCategory.Dialog)
                    continue;

                if (connectorTextList.TryGetValue(connectorData.TextXmlNode.OuterXml, out short existingIndex))
                {
                    return connectorData.Index > existingIndex 
                            ? connectorData.Index 
                            : existingIndex;
                }
                else
                {
                    connectorTextList.Add(connectorData.TextXmlNode.OuterXml, connectorData.Index);
                }
            }

            return 0;
        }

        public int CountDialogFunctions(Shape shape)
        {
            string shapeXml = shape.Master.NameU switch
            {//may need to revise the shapes allowed here to include, consitions, decisions and theit Wait_<master> counterparts i.e. anything that can have a function although currently, it is only being used in these threee contexts
                UniversalMasterName.ACTION or UniversalMasterName.DIALOG or UniversalMasterName.CONNECTOBJECT => _shapeXmlHelper.GetXmlString(shape),
                _ => throw _exceptionHelper.CriticalException("{7A51CB43-3EB9-40CD-B2B4-10014576D21C}"),
            };

            if (string.IsNullOrEmpty(shapeXml))
                return 0;

            return _xmlDocumentHelpers.SelectElements
            (
                _xmlDocumentHelpers.ToXmlDocument(shapeXml),
                $"//{XmlDataConstants.FUNCTIONELEMENT}"
            )
            .Select
            (
                element => element.GetAttribute(XmlDataConstants.NAMEATTRIBUTE)
            )
            .Aggregate
            (
                0,
                (count, functionName) =>
                {
                    if (_configurationService.FunctionList.Functions.TryGetValue(functionName, out Function? function) 
                            && function.FunctionCategory == FunctionCategories.DialogForm)
                        count++;

                    return count;
                }
            );
        }

        public int CountFunctions(Shape shape)
        {
            if (shape.Master.NameU != UniversalMasterName.DIALOG 
                && shape.Master.NameU != UniversalMasterName.ACTION)
                throw _exceptionHelper.CriticalException("{2B5BB080-A9EB-49DF-8204-5322E062E9BF}");

            string functionsXml = _shapeXmlHelper.GetXmlString(shape);
            if (functionsXml.Length == 0)
                return 0;

            return _functionsDataParser.Parse
            (
                _xmlDocumentHelpers.ToXmlElement(functionsXml)
            ).FunctionElements.Count;
        }

        public short CountIncomingConnectors(Shape toShape)
        {
            if (ShapeCollections.Connectors.ToHashSet().Contains(toShape.Master.NameU))
                throw _exceptionHelper.CriticalException("{AFCF9E84-6AB2-4A4D-BC5E-2B57550E3C11}");

            short incomingCount = 0;
            foreach (Connect shapeFromConnect in toShape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visEnd)
                    incomingCount++;
            }
            return incomingCount;
        }

        public short CountInvalidMultipleChoiceConnectors(Shape fromShape)
        {
            if (fromShape.Master.NameU != UniversalMasterName.DIALOG)
                throw _exceptionHelper.CriticalException("{51176A6A-419D-4762-9BDC-3A1F4A99171D}");

            if (CountDialogFunctions(fromShape) < 1)
                return 0;

            short outGoingCount = CountOutgoingConnectors(fromShape);
            ConnectorData[] connectorDataArray = new ConnectorData[outGoingCount];

            foreach (Connect shapeFromConnect in fromShape.FromConnects)
            {
                Shape connector = shapeFromConnect.FromSheet;
                string connectorXml = _shapeXmlHelper.GetXmlString(connector);
                if (connectorXml.Length == 0)
                    continue;

                ConnectorData connectorData = _connectorDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(connectorXml)
                );
                int dataArrayIndex = connectorData.Index - 1;
                if (connectorData.ConnectorCategory == ConnectorCategory.Dialog && dataArrayIndex < outGoingCount)
                    connectorDataArray[dataArrayIndex] = connectorData;
            }

            return (short)connectorDataArray.Count(data => data == null);
        }

        public short CountOutgoingBlankConnectors(Shape fromShape)
        {
            if (ShapeCollections.Connectors.ToHashSet().Contains(fromShape.Master.NameU))
                throw _exceptionHelper.CriticalException("{6D6FBE5B-59A2-4372-BADE-B95698BD0EBF}");

            short outGoingCount = 0;
            foreach (Connect shapeFromConnect in fromShape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visBegin 
                    && _shapeXmlHelper.GetXmlString(shapeFromConnect.FromSheet).Length == 0)
                    outGoingCount++;
            }
            return outGoingCount;
        }

        public short CountOutgoingConnectors(Shape fromShape)
        {
            if (ShapeCollections.Connectors.ToHashSet().Contains(fromShape.Master.NameU))
                throw _exceptionHelper.CriticalException("{79AE6046-FD96-41CA-8617-87C28FCC4144}");

            short outGoingCount = 0;
            foreach (Connect shapeFromConnect in fromShape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visBegin)
                    outGoingCount++;
            }
            return outGoingCount;
        }

        public short CountOutgoingNonApplicationConnectors(Shape fromShape)
        {
            if (ShapeCollections.Connectors.ToHashSet().Contains(fromShape.Master.NameU))
                throw _exceptionHelper.CriticalException("{81187778-E241-4737-BAE9-B9F4B43DE55E}");

            short outGoingCount = 0;
            foreach (Connect shapeFromConnect in fromShape.FromConnects)
            {
                if (shapeFromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT
                    && shapeFromConnect.FromPart == (short)VisFromParts.visBegin)
                    outGoingCount++;
            }
            return outGoingCount;
        }

        public IList<string> GetApplicationList(Shape connector, ShapeBag fromShapeBag)
        {
            if (!ShapeCollections.ApplicationConnectors.ToHashSet().Contains(connector.Master.NameU))
                throw _exceptionHelper.CriticalException("{FC8A71A1-81E0-43EF-811E-D143D0943F1D}");

            return connector.Master.NameU switch
            {
                UniversalMasterName.APP01CONNECTOBJECT 
                or UniversalMasterName.APP02CONNECTOBJECT 
                or UniversalMasterName.APP03CONNECTOBJECT 
                or UniversalMasterName.APP04CONNECTOBJECT 
                or UniversalMasterName.APP05CONNECTOBJECT 
                or UniversalMasterName.APP06CONNECTOBJECT 
                or UniversalMasterName.APP07CONNECTOBJECT 
                or UniversalMasterName.APP08CONNECTOBJECT 
                or UniversalMasterName.APP09CONNECTOBJECT 
                or UniversalMasterName.APP10CONNECTOBJECT
                    => new string[] { GetApplicationName(connector) },
                UniversalMasterName.OTHERSCONNECTOBJECT 
                    => GetOtherApplicationsList(connector, fromShapeBag),
                _ => throw _exceptionHelper.CriticalException("{CF08C37C-1FC0-43AA-8C44-8687DD8257C7}"),
            };
        }
        
        public string GetApplicationName(Shape connector)
            => GetApplicationName(connector.Master.NameU);

        public string GetApplicationName(string connectorMasterNameU)
        {
            if (!ShapeCollections.ApplicationSpecificConnectors.ToHashSet().Contains(connectorMasterNameU))
                throw _exceptionHelper.CriticalException("{2FADB011-5D91-415D-B698-F62A4C96A8BE}");

            Dictionary<string, int> dictionary = new()
            {
                [UniversalMasterName.APP01CONNECTOBJECT] = 1,
                [UniversalMasterName.APP02CONNECTOBJECT] = 2,
                [UniversalMasterName.APP03CONNECTOBJECT] = 3,
                [UniversalMasterName.APP04CONNECTOBJECT] = 4,
                [UniversalMasterName.APP05CONNECTOBJECT] = 5,
                [UniversalMasterName.APP06CONNECTOBJECT] = 6,
                [UniversalMasterName.APP07CONNECTOBJECT] = 7,
                [UniversalMasterName.APP08CONNECTOBJECT] = 8,
                [UniversalMasterName.APP09CONNECTOBJECT] = 9,
                [UniversalMasterName.APP10CONNECTOBJECT] = 10,
            };

            if (!dictionary.TryGetValue(connectorMasterNameU, out int appNumber))
                throw _exceptionHelper.CriticalException("{673341EF-B7B8-4AC8-A7B2-29307B4D93E7}");

            return string.Format
            (
                CultureInfo.CurrentCulture,
                Strings.applicationNameFormat,
                appNumber.ToString("00", CultureInfo.CurrentCulture)
            );
        }

        public Shape GetFromShape(Shape connector)
        {
            if (!ShapeCollections.Connectors.ToHashSet().Contains(connector.Master.NameU))
                throw _exceptionHelper.CriticalException("{1C081184-8203-433C-8985-5AD5DF2C312A}");

            foreach (Connect connectorConnect in connector.Connects)
            {
                if (connectorConnect.FromPart == (short)VisFromParts.visBegin)
                    return connectorConnect.ToSheet;
            }

            throw _exceptionHelper.CriticalException("{226DB763-8E68-4431-B778-9B11DFBA0B3E}");
        }

        public IList<ConnectorData> GetMultipleChoiceConnectorData(Shape fromShape)
        {
            if (fromShape.Master.NameU != UniversalMasterName.DIALOG)
                throw _exceptionHelper.CriticalException("{10D222F7-DB67-41B9-B377-7E5986345BEF}");

            if (CountDialogFunctions(fromShape) < 1)
                throw _exceptionHelper.CriticalException("{7A753DBD-E4E4-4ED2-8C8B-C2B5C0B9FDEB}");

            return fromShape.FromConnects.Cast<Connect>().Aggregate
            (
                new List<ConnectorData>(), 
                (list, shapeFromConnect) =>
                {
                    Shape connector = shapeFromConnect.FromSheet;
                    string shapeXml = _shapeXmlHelper.GetXmlString(connector);
                    if (shapeXml.Length == 0)
                        return list;

                    ConnectorData connectorData = _connectorDataParser.Parse
                    (
                        _xmlDocumentHelpers.ToXmlElement(shapeXml)
                    );

                    if (connectorData.ConnectorCategory == ConnectorCategory.Dialog)
                        list.Add(connectorData);

                    return list;
                }
            );
        }

        public IList<string> GetOtherApplications(Shape connector)
        {
            HashSet<string> usedApplications = GetUsedApplications();
            return GetAllApplications()
                .Where(appName => !usedApplications.Contains(appName))
                .ToArray();

            HashSet<string> GetUsedApplications()
            {
                Shape fromShape = GetFromShape(connector);
                if (fromShape.Master.NameU == UniversalMasterName.MERGEOBJECT)
                    return GetUsedOutgoingApplications(fromShape).ToHashSet();

                Shape toShape = GetToShape(connector);
                if (toShape.Master.NameU == UniversalMasterName.MERGEOBJECT)
                    return GetUsedIncomingApplications(toShape).ToHashSet();

                throw _exceptionHelper.CriticalException("{07850355-EFFF-4469-85A3-D923DF1B5F63}");
            }
        }

        public IList<string> GetOtherApplicationsList(Shape connector, ShapeBag fromShapeBag)
        {
            if (!ShapeCollections.ApplicationConnectors.ToHashSet().Contains(connector.Master.NameU))
                throw _exceptionHelper.CriticalException("{FFFA3C7A-1B02-4BD0-AD46-B7BED83D1477}");

            if (fromShapeBag.Shape.Master.NameU == UniversalMasterName.MERGEOBJECT)
                return GetOtherApplications(connector);
            else
                return fromShapeBag.OtherConnectorApplications ?? throw _exceptionHelper.CriticalException("{EC87C16A-4013-40F3-82A8-BFB1FBD9CD78}");
        }

        public Shape GetOutgoingBlankConnector(Shape fromShape)
        {
            if (ShapeCollections.Connectors.ToHashSet().Contains(fromShape.Master.NameU))
                throw _exceptionHelper.CriticalException("{05313042-7AE9-40D2-B7DD-EEC98D06E74C}");

            foreach (Connect shapeFromConnect in fromShape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visBegin 
                    && _shapeXmlHelper.GetXmlString(shapeFromConnect.FromSheet).Length == 0)
                    return shapeFromConnect.FromSheet;
            }

            throw _exceptionHelper.CriticalException("{B11075A7-465F-418E-96BA-E5A1F833E922}");
        }

        public IList<Shape> GetOutgoingBlankConnectors(Shape fromShape)
        {
            if (
                    !ShapeCollections.AllowedApplicationFlowShapes
                        .ToHashSet()
                        .Contains(fromShape.Master.NameU)
               )
            {
                throw _exceptionHelper.CriticalException("{D2E0F01D-5D0A-4107-A10F-E5BEBF64EDA9}");
            }

            return fromShape.FromConnects.Cast<Connect>()
                .Where
                (
                    fc => fc.FromPart == (short)VisFromParts.visBegin
                            && _shapeXmlHelper.GetXmlString(fc.FromSheet).Length == 0
                )
                .Select(fc => fc.FromSheet)
                .OrderBy(shape => shape.Master.NameU)
                .ToArray();
        }

        public Shape? GetOutgoingNoConnector(Shape fromShape)
        {
            if (!ShapeCollections.DecisionShapes.ToHashSet().Contains(fromShape.Master.NameU))
                throw _exceptionHelper.CriticalException("{7613E57D-D39E-46FF-BDDB-9B2A83ED391D}");

            const short noIndex = 2;
            foreach (Connect shapeFromConnect in fromShape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visBegin)
                {
                    string connectorXml = _shapeXmlHelper.GetXmlString(shapeFromConnect.FromSheet);
                    if (connectorXml.Length == 0)
                        continue;

                    ConnectorData connectorData = _connectorDataParser.Parse
                    (
                        _xmlDocumentHelpers.ToXmlElement(connectorXml)
                    );
                    if (connectorData.Index == noIndex && connectorData.ConnectorCategory == ConnectorCategory.Decision)
                        return shapeFromConnect.FromSheet;
                }
            }

            return null;
        }

        public Shape? GetOutgoingYesConnector(Shape fromShape)
        {
            if (!ShapeCollections.DecisionShapes.ToHashSet().Contains(fromShape.Master.NameU))
                throw _exceptionHelper.CriticalException("{5BAB7A7D-4EB9-40AA-84A4-1468E1D6F52C}");

            const short yesIndex = 1;
            foreach (Connect shapeFromConnect in fromShape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visBegin)
                {
                    string connectorXml = _shapeXmlHelper.GetXmlString(shapeFromConnect.FromSheet);
                    if (connectorXml.Length == 0)
                        continue;

                    ConnectorData connectorData = _connectorDataParser.Parse
                    (
                        _xmlDocumentHelpers.ToXmlElement(connectorXml)
                    );
                    if (connectorData.Index == yesIndex && connectorData.ConnectorCategory == ConnectorCategory.Decision)
                    {
                        return shapeFromConnect.FromSheet;
                    }
                }
            }

            return null;
        }

        public Shape GetToShape(Shape connector)
        {
            if (!ShapeCollections.Connectors.ToHashSet().Contains(connector.Master.NameU))
                throw _exceptionHelper.CriticalException("{3D052F01-DAD2-48F6-971F-4CAD43D8085D}");

            foreach (Connect connectorConnect in connector.Connects)
            {
                if (connectorConnect.FromPart == (short)VisFromParts.visEnd)
                    return connectorConnect.ToSheet;
            }

            throw _exceptionHelper.CriticalException("{F80964B3-B50E-471C-B7BF-846A5B4928D9}");
        }

        public IList<string> GetUnusedApplications(Shape shape, bool isSplitting)
        {
            if (shape.Master.NameU != UniversalMasterName.MERGEOBJECT)
                throw _exceptionHelper.CriticalException("{88AB85BA-5397-4745-92E2-68723F95A04B}");

            HashSet<string> usedMasters = isSplitting
                ? GetOutgoingUMasterNames(shape).ToHashSet()
                : GetIncomingUMasterNames(shape).ToHashSet();

            if (usedMasters.Contains(UniversalMasterName.OTHERSCONNECTOBJECT))
                return Array.Empty<string>();

            return GetAllApplicationSpecificMasters()
                .Aggregate
                (
                    new List<string>(), 
                    (list, masterName) =>
                    {
                        string applicationName = GetApplicationName(masterName);
                        if (_configurationService.GetApplication(applicationName) != null 
                                && !usedMasters.Contains(masterName))
                        {
                            list.Add(applicationName);
                        }

                        return list;
                    }
                );
        }

        public bool HasAllApplicationConnectors(Shape shape)
        {
            if (shape.FromConnects.Count < 1)
                return false;

            foreach (Connect fromConnect in shape.FromConnects)
            {
                if (fromConnect.FromSheet.Master.NameU == UniversalMasterName.CONNECTOBJECT)
                    return false;
            }

            return true;
        }

        public bool HasAllNonApplicationConnectors(Shape shape)
        {
            if (shape.FromConnects.Count < 1)
                return false;

            HashSet<string> applicationConnectors = ShapeCollections.ApplicationConnectors.ToHashSet();
            foreach (Connect fromConnect in shape.FromConnects)
            {
                if (applicationConnectors.Contains(fromConnect.FromSheet.Master.NameU))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Gets all incoming universal master names (includes the "other" connector's master)
        /// </summary>
        /// <param name="toShape"></param>
        /// <returns></returns>
        private IList<string> GetIncomingUMasterNames(Shape toShape)
        {
            if (toShape.Master.NameU != UniversalMasterName.MERGEOBJECT)
                throw _exceptionHelper.CriticalException("{46F6CA2C-505E-4D2F-87F3-DF35722EFA16}");

            HashSet<string> applicationConnectors = ShapeCollections.ApplicationConnectors.ToHashSet();
            return toShape.FromConnects.Cast<Connect>()
                .Where
                (
                    fm => fm.FromPart == (short)VisFromParts.visEnd
                    && applicationConnectors.Contains(fm.FromSheet.Master.NameU)
                )
                .Select(fm => fm.FromSheet.Master.NameU)
                .OrderBy(name => name)
                .ToArray();
        }

        /// <summary>
        /// Gets all outgoing universal master names (includes the "other" connector's master)
        /// </summary>
        /// <param name="fromShape"></param>
        /// <returns></returns>
        private IList<string> GetOutgoingUMasterNames(Shape fromShape)
        {
            if (fromShape.Master.NameU != UniversalMasterName.MERGEOBJECT)
                throw _exceptionHelper.CriticalException("{8344D4E8-AC77-47F5-A5B0-9A1CB69F6E85}");

            HashSet<string> applicationConnectors = ShapeCollections.ApplicationConnectors.ToHashSet();
            return fromShape.FromConnects.Cast<Connect>()
                .Where
                (
                    fm => fm.FromPart == (short)VisFromParts.visBegin
                    && applicationConnectors.Contains(fm.FromSheet.Master.NameU)
                )
                .Select(fm => fm.FromSheet.Master.NameU)
                .OrderBy(name => name)
                .ToArray();
        }

        /// <summary>
        /// Gets all incoming application names (excludes those covered by "other")
        /// </summary>
        /// <param name="toShape"></param>
        /// <returns></returns>
        private IList<string> GetUsedIncomingApplications(Shape toShape)
        {
            if (toShape.Master.NameU != UniversalMasterName.MERGEOBJECT)
                throw _exceptionHelper.CriticalException("{FF659BE6-3562-427B-AA29-0F885BC8A444}");

            HashSet<string> applicationConnectors = ShapeCollections.ApplicationSpecificConnectors.ToHashSet();
            return toShape.FromConnects.Cast<Connect>()
                .Where
                (
                    fm => fm.FromPart == (short)VisFromParts.visEnd
                    && applicationConnectors.Contains(fm.FromSheet.Master.NameU)
                )
                .Select(fm => GetApplicationName(fm.FromSheet))
                .OrderBy(name => name)
                .ToArray();
        }

        /// <summary>
        /// Gets all outgoing application names (excludes those covered by "other")
        /// </summary>
        /// <param name="fromShape"></param>
        /// <returns></returns>
        private IList<string> GetUsedOutgoingApplications(Shape fromShape)
        {
            if (fromShape.Master.NameU != UniversalMasterName.MERGEOBJECT)
                throw _exceptionHelper.CriticalException("{E44E48C5-2DD3-442B-A36A-3B4950BACD06}");

            HashSet<string> applicationConnectors = ShapeCollections.ApplicationSpecificConnectors.ToHashSet();
            return fromShape.FromConnects.Cast<Connect>()
                .Where
                (
                    fm => fm.FromPart == (short)VisFromParts.visBegin
                    && applicationConnectors.Contains(fm.FromSheet.Master.NameU)
                )
                .Select(fm => GetApplicationName(fm.FromSheet))
                .OrderBy(name => name)
                .ToArray();
        }

        private IList<string> GetAllApplications()
            => ShapeCollections.ApplicationSpecificConnectors
                .Select(application => GetApplicationName(application))
                .OrderBy(application => application)
                .ToArray();

        private static IList<string> GetAllApplicationSpecificMasters()
            => ShapeCollections.ApplicationSpecificConnectors
                .OrderBy(master => master)
                .ToArray();
    }
}
