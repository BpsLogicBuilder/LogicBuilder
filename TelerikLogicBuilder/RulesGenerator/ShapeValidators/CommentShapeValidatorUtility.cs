using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class CommentShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IShapeHelper _shapeHelper;

        public CommentShapeValidatorUtility(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors, IContextProvider contextProvider, IShapeHelper shapeHelper) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _shapeHelper = shapeHelper;
        }

        internal override void Validate()
        {
            if (_shapeHelper.CountIncomingConnectors(this.Shape) > 0 
                || _shapeHelper.CountOutgoingConnectors(this.Shape) > 0)
                AddValidationMessage(Strings.commentShapeCannotHaveConnectors);
        }
    }
}
