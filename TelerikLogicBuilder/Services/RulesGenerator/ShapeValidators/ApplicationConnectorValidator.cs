using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class ApplicationConnectorValidator : IApplicationConnectorValidator
    {
        private readonly IContextProvider _contextProvider;
        private readonly IModuleDataParser _moduleDataParser;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;

        public ApplicationConnectorValidator(IContextProvider contextProvider, IModuleDataParser moduleDataParser, IShapeHelper shapeHelper, IShapeXmlHelper shapeXmlHelper)
        {
            _contextProvider = contextProvider;
            _moduleDataParser = moduleDataParser;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
        }

        public void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors)
        {
            new ApplicationConnectorValidatorUtility
            (
                sourceFile,
                page,
                shape,
                validationErrors,
                _contextProvider,
                _moduleDataParser,
                _shapeHelper,
                _shapeXmlHelper
            ).Validate();
        }
    }
}
