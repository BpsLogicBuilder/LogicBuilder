using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class ConditionShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IConditionsElementValidator _conditionsElementValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConditionShapeValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            ApplicationTypeInfo application,
            IContextProvider contextProvider,
            IConditionsElementValidator conditionsElementValidator,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _conditionsElementValidator = conditionsElementValidator;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            Application = application;
        }

        private ApplicationTypeInfo Application { get; }

        internal override void Validate()
        {
            if (_shapeHelper.CountOutgoingConnectors(this.Shape) != 2
                || _shapeHelper.GetOutgoingYesConnector(this.Shape) == null
                || _shapeHelper.GetOutgoingNoConnector(this.Shape) == null)
                AddValidationMessage(Strings.conditionBoxOutgoingRequired);

            if (_shapeHelper.CountIncomingConnectors(this.Shape) == 0)
                AddValidationMessage(Strings.conditionBoxIncomingRequired);

            string conditionsXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (conditionsXml.Length == 0)
            {
                AddValidationMessage(Strings.conditionShapeDataRequired);
                return;
            }

            List<string> errors = new();
            _conditionsElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(conditionsXml),
                Application,
                errors
            );
            errors.ForEach(error => AddValidationMessage(error));
        }
    }
}
