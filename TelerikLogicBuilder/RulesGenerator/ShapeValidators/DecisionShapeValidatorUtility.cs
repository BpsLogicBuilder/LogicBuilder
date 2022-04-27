using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class DecisionShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IDecisionsElementValidator _decisionsElementValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DecisionShapeValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            ApplicationTypeInfo application,
            IContextProvider contextProvider,
            IDecisionsElementValidator decisionsElementValidator,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _decisionsElementValidator = decisionsElementValidator;
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
                AddValidationMessage(Strings.decisionBoxOutgoingRequired);

            if (_shapeHelper.CountIncomingConnectors(this.Shape) == 0)
                AddValidationMessage(Strings.decisionBoxIncomingRequired);

            string decisionsXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (decisionsXml.Length == 0)
            {
                AddValidationMessage(Strings.decisionShapeDataRequired);
                return;
            }

            List<string> errors = new();
            _decisionsElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(decisionsXml),
                Application,
                errors
            );
            errors.ForEach(error => AddValidationMessage(error));
        }
    }
}
