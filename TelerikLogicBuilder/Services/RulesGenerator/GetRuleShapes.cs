using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class GetRuleShapes : IGetRuleShapes
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public GetRuleShapes(
            IExceptionHelper exceptionHelper,
            IJumpDataParser jumpDataParser,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDictionary<string, Shape> jumpToShapes)
        {
            _exceptionHelper = exceptionHelper;
            _jumpDataParser = jumpDataParser;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            JumpToShapes = jumpToShapes;
        }

        private IDictionary<string, Shape> JumpToShapes { get; }

        public void GetShapes(Shape connector, IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors)
        {
            Shape lastShape = _shapeHelper.GetToShape(connector);

            switch (lastShape.Master.NameU)
            {
                case UniversalMasterName.ACTION:
                    ruleShapes.Add(GetShapeBag(lastShape, connector, ruleShapes[ruleShapes.Count - 1]));
                    break;
                case UniversalMasterName.JUMPOBJECT:
                    string jumpShapeText = _jumpDataParser.Parse
                    (
                        _xmlDocumentHelpers.ToXmlElement(_shapeXmlHelper.GetXmlString(lastShape))
                    );
                    Shape jumpToShape = JumpToShapes[jumpShapeText];
                    Shape jumpToShapeConnector = jumpToShape.FromConnects[1].FromSheet;
                    if (!ruleConnectors.Contains(jumpToShapeConnector))
                        ruleConnectors.Add(jumpToShapeConnector);

                    GetShapes(jumpToShapeConnector, ruleShapes, ruleConnectors);
                    return;//If lastShape is a jump object it can only have a visEnd connector attached
                           //so no reason to continue.
                default:
                    ruleShapes.Add(GetShapeBag(lastShape, connector, ruleShapes[ruleShapes.Count - 1]));
                    return;
            }

            if (lastShape.Master.NameU == UniversalMasterName.ACTION && ShapeCollections.ApplicationConnectors.ToHashSet().Contains(connector.Master.NameU))
            {//In this case the next connector selected is dependent on the previous connector
                Shape nextApplicationSpecificConnector = GetNextApplicationSpecificConnector(lastShape, connector.Master.NameU);

                if (!ruleConnectors.Contains(nextApplicationSpecificConnector))
                    ruleConnectors.Add(nextApplicationSpecificConnector);

                GetShapes(nextApplicationSpecificConnector, ruleShapes, ruleConnectors);
                return;
            }

            foreach (Connect lastShapeFromConnect in lastShape.FromConnects)
            {
                if (lastShapeFromConnect.FromPart == (short)VisFromParts.visEnd)
                    continue;

                if (!ruleConnectors.Contains(lastShapeFromConnect.FromSheet))
                    ruleConnectors.Add(lastShapeFromConnect.FromSheet);
                //last shape is always Action at this point
                //because Jump Shape can't have a visBegin connector
                //and also be a last shape

                GetShapes(lastShapeFromConnect.FromSheet, ruleShapes, ruleConnectors);
            }
        }

        private Shape GetNextApplicationSpecificConnector(Shape lastShape, string connectorMaterNameU)
        {
            foreach (Connect lastShapeFromConnect in lastShape.FromConnects)
            {
                if (lastShapeFromConnect.FromPart == (short)VisFromParts.visEnd)
                    continue;

                if (lastShapeFromConnect.FromSheet.Master.NameU == connectorMaterNameU)
                    return lastShapeFromConnect.FromSheet;
            }

            throw _exceptionHelper.CriticalException("{76A2717F-380C-4678-90D8-95F27B1FE4A5}");
        }

        private ShapeBag GetShapeBag(Shape toShape, Shape connector, ShapeBag fromShapeBag)
        {
            if (connector.Master.NameU == UniversalMasterName.CONNECTOBJECT)
                return new ShapeBag(toShape);

            if (connector.Master.NameU != UniversalMasterName.CONNECTOBJECT && toShape.Master.NameU == UniversalMasterName.MERGEOBJECT)
                return new ShapeBag(toShape);

            if (ShapeCollections.ApplicationConnectors.ToHashSet().Contains(connector.Master.NameU))
                return new ShapeBag(toShape, _shapeHelper.GetOtherApplicationsList(connector, fromShapeBag));

            throw _exceptionHelper.CriticalException("{1C90F5BD-57C0-4275-AE36-80EA162A9A88}");
        }
    }
}
