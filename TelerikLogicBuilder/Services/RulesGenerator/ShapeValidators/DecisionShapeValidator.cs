using ABIS.LogicBuilder.FlowBuilder.StructuresFactories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class DecisionShapeValidator : IShapeValidator
    {
        private readonly IDecisionsElementValidator _decisionsElementValidator;
        private readonly IDiagramResultMessageHelper _resultMessageHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DecisionShapeValidator(
            IDecisionsElementValidator decisionsElementValidator,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IStructuresFactory structuresFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            ApplicationTypeInfo application)
        {
            _decisionsElementValidator = decisionsElementValidator;
            _resultMessageHelper = structuresFactory.GetDiagramResultMessageHelper
            (
                sourceFile,
                page,
                shape,
                validationErrors
            );
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            Application = application;
            Shape = shape;
        }

        private ApplicationTypeInfo Application { get; }
        private Shape Shape { get; }

        public void Validate()
        {
            if (_shapeHelper.CountOutgoingConnectors(this.Shape) != 2
                    || _shapeHelper.GetOutgoingYesConnector(this.Shape) == null
                    || _shapeHelper.GetOutgoingNoConnector(this.Shape) == null)
                _resultMessageHelper.AddValidationMessage(Strings.decisionBoxOutgoingRequired);

            if (_shapeHelper.CountIncomingConnectors(this.Shape) == 0)
                _resultMessageHelper.AddValidationMessage(Strings.decisionBoxIncomingRequired);

            string decisionsXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (decisionsXml.Length == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.decisionShapeDataRequired);
                return;
            }

            List<string> errors = new();
            _decisionsElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(decisionsXml),
                Application,
                errors
            );
            errors.ForEach(error => _resultMessageHelper.AddValidationMessage(error));
        }
    }
}
