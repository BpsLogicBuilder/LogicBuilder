using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class ApplicationSpecificFlowShapeValidator : IApplicationSpecificFlowShapeValidator
    {
        private readonly IResultMessageHelper _resultMessageHelper;

        public ApplicationSpecificFlowShapeValidator(
            IResultMessageHelperFactory resultMessageHelperfactory,
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

            Shape = shape;
        }

        private Shape Shape { get; }

        public void Validate()
        {
            HashSet<string> outgoingConnectors = new();
            HashSet<string> incomingConnectors = new();
            foreach (Connect shapeFromConnect in this.Shape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visBegin)
                {
                    if (outgoingConnectors.Contains(shapeFromConnect.FromSheet.Master.NameU))
                        _resultMessageHelper.AddValidationMessage(Strings.duplicateOutgoingConnector);
                    else
                        outgoingConnectors.Add(shapeFromConnect.FromSheet.Master.NameU);
                }
            }

            foreach (Connect shapeFromConnect in this.Shape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visEnd)
                {
                    if (incomingConnectors.Contains(shapeFromConnect.FromSheet.Master.NameU))
                        _resultMessageHelper.AddValidationMessage(Strings.duplicateIncomingConnector);
                    else
                        incomingConnectors.Add(shapeFromConnect.FromSheet.Master.NameU);
                }
            }

            if (outgoingConnectors.Count != incomingConnectors.Count)
            {
                _resultMessageHelper.AddValidationMessage(Strings.applicationConnectorMismatch);
                return;
            }

            foreach (string master in outgoingConnectors)
            {
                if (!incomingConnectors.Contains(master))
                    _resultMessageHelper.AddValidationMessage(Strings.applicationConnectorMismatch);
            }
        }
    }
}
