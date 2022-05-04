using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class JumpShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public JumpShapeValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            IContextProvider contextProvider,
            IJumpDataParser jumpDataParser,
            IShapeXmlHelper shapeXmlHelper) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _jumpDataParser = jumpDataParser;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        internal override void Validate()
        {
            string jumpShapeXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (jumpShapeXml.Length == 0)
            {
                AddValidationMessage(Strings.jumpShapeDataRequired);
                return;
            }

            string jumpShapeText = _jumpDataParser.Parse(_xmlDocumentHelpers.ToXmlElement(jumpShapeXml));
            if (jumpShapeText.Length == 0)
            {
                AddValidationMessage(Strings.jumpShapeDataRequired);
            }

            if (Shape.FromConnects.Count == 0)
            {
                AddValidationMessage(Strings.noConnectorsOnJumpFormat);
                return;
            }

            if (ShapeHasIncomingAndOutGoung())
            {
                AddValidationMessage(Strings.jumpConnectorsBothDirections);
                return;
            }

            if (Shape.FromConnects.Count > 1 && ShapeIsToJumpShape())
                AddValidationMessage(Strings.jumpShape1OutGoing);

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
