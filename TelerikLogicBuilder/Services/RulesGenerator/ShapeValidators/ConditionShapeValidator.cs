using ABIS.LogicBuilder.FlowBuilder.Factories;
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
    internal class ConditionShapeValidator : IShapeValidator
    {
        private readonly IConditionsElementValidator _conditionsElementValidator;
        private readonly IResultMessageHelper _resultMessageHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConditionShapeValidator(
            IConditionsElementValidator conditionsElementValidator,
            IResultMessageHelperFactory resultMessageHelperfactory,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            ApplicationTypeInfo application)
        {
            _conditionsElementValidator = conditionsElementValidator;
            _resultMessageHelper = resultMessageHelperfactory.GetResultMessageHelper
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
                _resultMessageHelper.AddValidationMessage(Strings.conditionBoxOutgoingRequired);

            if (_shapeHelper.CountIncomingConnectors(this.Shape) == 0)
                _resultMessageHelper.AddValidationMessage(Strings.conditionBoxIncomingRequired);

            string conditionsXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (conditionsXml.Length == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.conditionShapeDataRequired);
                return;
            }

            List<string> errors = new();
            _conditionsElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(conditionsXml),
                Application,
                errors
            );
            errors.ForEach(error => _resultMessageHelper.AddValidationMessage(error));
        }
    }
}
