using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class WaitConditionShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IApplicationSpecificFlowShapeValidator _applicationSpecificFlowShapeValidator;
        private readonly IConditionsElementValidator _conditionsElementValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public WaitConditionShapeValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            ApplicationTypeInfo application,
            IContextProvider contextProvider,
            IApplicationSpecificFlowShapeValidator applicationSpecificFlowShapeValidator,
            IConditionsElementValidator conditionsElementValidator,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _applicationSpecificFlowShapeValidator = applicationSpecificFlowShapeValidator;
            _conditionsElementValidator = conditionsElementValidator;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            Application = application;
        }

        private ApplicationTypeInfo Application { get; }

        internal override void Validate()
        {
            if (_shapeHelper.CountIncomingConnectors(this.Shape) == 0)
                AddValidationMessage(Strings.waitConditionShapeIncomingRequired);

            if (this.Shape.FromConnects.Count < 1)
                return;

            bool allApplication = _shapeHelper.HasAllApplicationConnectors(this.Shape);
            bool allNonApplication = _shapeHelper.HasAllNonApplicationConnectors(this.Shape);

            if (!(allApplication || allNonApplication))
            {
                AddValidationMessage(Strings.allWaitConditionsConnectorsSameStencil);
                return;
            }

            if (allNonApplication)
                ValidateNonApplicationSpecific();

            if (allApplication)
            {
                _applicationSpecificFlowShapeValidator.Validate
                (
                    this.SourceFile,
                    this.Page,
                    this.Shape,
                    this.ValidationErrors
                );
            }

            string waitConditionXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (waitConditionXml.Length == 0)
            {
                AddValidationMessage(Strings.waitConditionShapeDataRequired);
                return;
            }

            List<string> errors = new();
            _conditionsElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(waitConditionXml),
                Application,
                errors
            );
            errors.ForEach(error => AddValidationMessage(error));
        }

        /// <summary>
        /// Ensures there is only one out going connector and that it is blank.
        /// </summary>
        private void ValidateNonApplicationSpecific()
        {
            if (_shapeHelper.CountOutgoingBlankConnectors(this.Shape) != 1 
                || _shapeHelper.CountOutgoingConnectors(this.Shape) != 1)
                AddValidationMessage(Strings.waitConditionShapeOutgoingRequired);
        }
    }
}
