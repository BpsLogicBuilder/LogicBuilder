using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class ApplicationSpecificFlowShapeValidatorUtility : ShapeValidatorUtility
    {
        public ApplicationSpecificFlowShapeValidatorUtility(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors, IContextProvider contextProvider) 
            : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
        }

        internal override void Validate()
        {
            HashSet<string> outgoingConnectors = new();
            HashSet<string> incomingConnectors = new();
            foreach (Connect shapeFromConnect in this.Shape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visBegin)
                {
                    if (outgoingConnectors.Contains(shapeFromConnect.FromSheet.Master.NameU))
                        AddValidationMessage(Strings.duplicateOutgoingConnector);
                    else
                        outgoingConnectors.Add(shapeFromConnect.FromSheet.Master.NameU);
                }
            }

            foreach (Connect shapeFromConnect in this.Shape.FromConnects)
            {
                if (shapeFromConnect.FromPart == (short)VisFromParts.visEnd)
                {
                    if (incomingConnectors.Contains(shapeFromConnect.FromSheet.Master.NameU))
                        AddValidationMessage(Strings.duplicateIncomingConnector);
                    else
                        incomingConnectors.Add(shapeFromConnect.FromSheet.Master.NameU);
                }
            }

            if (outgoingConnectors.Count != incomingConnectors.Count)
            {
                AddValidationMessage(Strings.applicationConnectorMismatch);
                return;
            }

            foreach (string master in outgoingConnectors)
            {
                if (!incomingConnectors.Contains(master))
                    AddValidationMessage(Strings.applicationConnectorMismatch);
            }
        }
    }
}
