using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class JumpShapeValidator : IShapeValidator
    {
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IDiagramResultMessageHelper _resultMessageHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public JumpShapeValidator(
            IJumpDataParser jumpDataParser,
            IShapeXmlHelper shapeXmlHelper,
            IStructuresFactory structuresFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors)
        {
            _jumpDataParser = jumpDataParser;
            _resultMessageHelper = structuresFactory.GetDiagramResultMessageHelper
            (
                sourceFile,
                page,
                shape,
                validationErrors
            );
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            Shape = shape;
        }

        private Shape Shape { get; }

        public void Validate()
        {
            string jumpShapeXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (jumpShapeXml.Length == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.jumpShapeDataRequired);
                return;
            }

            string jumpShapeText = _jumpDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(jumpShapeXml));
            if (jumpShapeText.Length == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.jumpShapeDataRequired);
            }

            if (Shape.FromConnects.Count == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.noConnectorsOnJumpFormat);
                return;
            }

            if (ShapeHasIncomingAndOutGoung())
            {
                _resultMessageHelper.AddValidationMessage(Strings.jumpConnectorsBothDirections);
                return;
            }

            if (Shape.FromConnects.Count > 1 && ShapeIsToJumpShape())
                _resultMessageHelper.AddValidationMessage(Strings.jumpShape1OutGoing);

            bool ShapeHasIncomingAndOutGoung()
            {
                foreach (Connect fromConnect in this.Shape.FromConnects)
                {
                    if (fromConnect.FromPart != this.Shape.FromConnects[1].FromPart)
                        return true;
                }

                return false;
            }

            bool ShapeIsToJumpShape()
                => this.Shape.FromConnects[1].FromPart == (short)VisFromParts.visBegin;
        }
    }
}
