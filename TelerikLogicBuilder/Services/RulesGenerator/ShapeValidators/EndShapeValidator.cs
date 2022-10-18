using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class EndShapeValidator : IShapeValidator
    {
        private readonly IShapeHelper _shapeHelper;
        private readonly IResultMessageHelper _resultMessageHelper;

        public EndShapeValidator(
            IResultMessageHelperFactory resultMessageHelperfactory,
            IShapeHelper shapeHelper,
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors)
        {
            _resultMessageHelper = resultMessageHelperfactory.GetResultMessageHelper
            (
                sourceFile,
                page,
                shape,
                validationErrors
            );
            _shapeHelper = shapeHelper;

            Shape = shape;
        }

        private Shape Shape { get; }

        public void Validate()
        {
            if (_shapeHelper.CountOutgoingConnectors(this.Shape) > 0)
                _resultMessageHelper.AddValidationMessage(Strings.endShapeOutgoing);

            if (_shapeHelper.CountIncomingConnectors(this.Shape) < 1)
                _resultMessageHelper.AddValidationMessage(Strings.endShapeIncoming);
        }
    }
}
