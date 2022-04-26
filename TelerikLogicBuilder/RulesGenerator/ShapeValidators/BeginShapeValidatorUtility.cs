using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class BeginShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IShapeHelper _shapeHelper;

        public BeginShapeValidatorUtility(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors, IContextProvider contextProvider, IShapeHelper shapeHelper) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _shapeHelper = shapeHelper;
        }

        internal override void Validate()
        {
            if (_shapeHelper.CountOutgoingBlankConnectors(this.Shape) != 1 
                    || _shapeHelper.CountOutgoingConnectors(this.Shape) != 1)
                AddValidationMessage(Strings.beginShapeOutgoingRequired);

            if (_shapeHelper.CountIncomingConnectors(this.Shape) > 0)
                AddValidationMessage(Strings.beginShapeIncoming);
        }
    }
}
