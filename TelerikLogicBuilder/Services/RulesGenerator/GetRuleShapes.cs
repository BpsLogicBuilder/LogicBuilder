using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class GetRuleShapes : IGetRuleShapes
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public GetRuleShapes(IExceptionHelper exceptionHelper, IJumpDataParser jumpDataParser, IShapeHelper shapeHelper, IShapeXmlHelper shapeXmlHelper, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _jumpDataParser = jumpDataParser;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void GetShapes(Shape connector, IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors, IDictionary<string, Shape> jumpToShapes) 
            => new GetRuleShapesUtility
            (
                _exceptionHelper,
                _jumpDataParser,
                _shapeHelper,
                _shapeXmlHelper,
                _xmlDocumentHelpers,
                jumpToShapes
            ).GetShapes(connector, ruleShapes, ruleConnectors);
    }
}
