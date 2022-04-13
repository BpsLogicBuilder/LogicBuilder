using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class ShapeHelper : IShapeHelper
    {
        private readonly IConnectorDataParser connectorDataParser;
        private readonly IExceptionHelper _exceptionHelper;

        public ShapeHelper(IConnectorDataParser connectorDataParser, IExceptionHelper exceptionHelper)
        {
            this.connectorDataParser = connectorDataParser;
            _exceptionHelper = exceptionHelper;
        }

        public short CheckForDuplicateMultipleChoices(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public int CountDialogFunctions(Shape fromShape)
        {
            //string shapeXml;
            //switch (fromShape.Master.NameU)
            //{
            //    case UniversalMasterName.ACTION:
            //    case UniversalMasterName.DIALOG:
            //        shapeXml = ShapeEditor.GetShapeXml(fromShape, Schemas.FunctionsDataSchema);
            //        break;
            //    case UniversalMasterName.CONNECTOBJECT:
            //        shapeXml = ShapeEditor.GetShapeXml(fromShape, Schemas.ConnectorDataSchema);
            //        break;
            //    default:
            //        throw new CriticalLogicBuilderException(string.Format(CultureInfo.InvariantCulture, Strings.invalidArgumentTextFormat, "{F1B5A1B0-05E0-4292-8EF5-E03CD408CC4A}"));
            //}

            throw new NotImplementedException();
        }

        public int CountFunctions(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public short CountIncomingConnectors(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public short CountInvalidMultipleChoiceConnectors(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public short CountOutgoingBlankConnectors(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public short CountOutgoingConnectors(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public short CountOutgoingNonApplicationConnectors(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetApplicationList(Shape connector, ShapeBag fromShapeBag)
        {
            throw new NotImplementedException();
        }

        public Shape GetFromShape(Shape connector)
        {
            throw new NotImplementedException();
        }

        public IList<ConnectorData> GetMultipleChoiceConnectorData(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public Shape GetNextApplicationSpecificConnector(Shape lastShape, string connectorMasterNameU)
        {
            throw new NotImplementedException();
        }

        public IList<string> GetOtherApplicationsList(Shape connector, ShapeBag fromShapeBag)
        {
            throw new NotImplementedException();
        }

        public Shape GetOutgoingBlankConnector(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public Shape GetOutgoingNoConnector(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public Shape GetOutgoingYesConnector(Shape fromShape)
        {
            throw new NotImplementedException();
        }

        public Shape GetToShape(Shape connector)
        {
            throw new NotImplementedException();
        }

        public bool HasAllApplicationConnectors(Shape shape)
        {
            throw new NotImplementedException();
        }

        public bool HasAllNonApplicationConnectors(Shape shape)
        {
            throw new NotImplementedException();
        }
    }
}
