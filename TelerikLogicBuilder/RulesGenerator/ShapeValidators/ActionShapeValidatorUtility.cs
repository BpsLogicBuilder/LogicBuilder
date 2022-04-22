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
    internal class ActionShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IApplicationSpecificFlowShapeValidator _applicationSpecificFlowShapeValidator;
        private readonly IFunctionsElementValidator _functionsElementValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ActionShapeValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            ApplicationTypeInfo application,
            IContextProvider contextProvider,
            IApplicationSpecificFlowShapeValidator applicationSpecificFlowShapeValidator,
            IFunctionsElementValidator functionsElementValidator,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
            : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _applicationSpecificFlowShapeValidator = applicationSpecificFlowShapeValidator;
            _functionsElementValidator = functionsElementValidator;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            Application = application;
        }

        private ApplicationTypeInfo Application { get; }

        internal override void Validate()
        {
            int dialogFunctions = _shapeHelper.CountDialogFunctions(this.Shape);
            if (dialogFunctions > 0)
                AddValidationMessage(Strings.dialogFunctionsInvalid);

            if (_shapeHelper.CountIncomingConnectors(this.Shape) < 1)
                AddValidationMessage(Strings.actionIncomingConnectorCount);

            if (this.Shape.FromConnects.Count < 1)
                return;

            bool allApplication = _shapeHelper.HasAllApplicationConnectors(this.Shape);
            bool allNonApplication = _shapeHelper.HasAllNonApplicationConnectors(this.Shape);

            if (!(allApplication || allNonApplication))
            {
                AddValidationMessage(Strings.allActionConnectorsSameStencil);
                return;
            }

            if (allNonApplication)
            {
                ValidateActionNonSpecific();
            }

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

            ValidateFunctionsXmlData();
        }

        /// <summary>
        /// Ensures there is only one out going connector and that it is blank.
        /// </summary>
        private void ValidateActionNonSpecific()
        {
            if (_shapeHelper.CountOutgoingBlankConnectors(this.Shape) != 1 || _shapeHelper.CountOutgoingConnectors(this.Shape) != 1)
                AddValidationMessage(Strings.actionShapeOneBlankConnector);
        }

        /// <summary>
        /// Checks for Functions, Variables and Decisions not configured in an Action Shape
        /// </summary>
        private void ValidateFunctionsXmlData()
        {
            string functionsXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (functionsXml.Length == 0)
            {
                AddValidationMessage(Strings.actionShapeDataRequired);
                return;
            }

            List<string> errors = new();
            _functionsElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(functionsXml), 
                this.Application, 
                errors
            );
            errors.ForEach(error => AddValidationMessage(error));
        }
    }
}
