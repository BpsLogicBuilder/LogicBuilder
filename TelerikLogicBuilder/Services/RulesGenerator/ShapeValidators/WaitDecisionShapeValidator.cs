using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class WaitDecisionShapeValidator : IShapeValidator
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IDecisionsElementValidator _decisionsElementValidator;
        private readonly IResultMessageHelper _resultMessageHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IRulesGeneratorFactory _rulesGeneratorFactory;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public WaitDecisionShapeValidator(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IDecisionsElementValidator decisionsElementValidator,
            IResultMessageHelperFactory resultMessageHelperfactory,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IRulesGeneratorFactory rulesGeneratorFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string sourceFile,
            Page page,
            ShapeBag shapeBag,
            List<ResultMessage> validationErrors,
            ApplicationTypeInfo application)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _decisionsElementValidator = decisionsElementValidator;
            _resultMessageHelper = resultMessageHelperfactory.GetResultMessageHelper
            (
                sourceFile,
                page,
                shapeBag.Shape,
                validationErrors
            );
            _shapeHelper = shapeHelper;
            _rulesGeneratorFactory = rulesGeneratorFactory;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            Application = application;
            Page = page;
            Shape = shapeBag.Shape;
            ShapeBag = shapeBag;
            SourceFile = sourceFile;
            ValidationErrors = validationErrors;
        }

        private ApplicationTypeInfo Application { get; }
        private Page Page { get; }
        private Shape Shape { get; }
        private ShapeBag ShapeBag { get; }
        private string SourceFile { get; }
        private List<ResultMessage> ValidationErrors { get; }

        public void Validate()
        {
            if (_shapeHelper.CountIncomingConnectors(this.Shape) == 0)
                _resultMessageHelper.AddValidationMessage(Strings.waitDecisionShapeIncomingRequired);

            if (this.Shape.FromConnects.Count < 1)
                return;

            bool allApplication = _shapeHelper.HasAllApplicationConnectors(this.Shape);
            bool allNonApplication = _shapeHelper.HasAllNonApplicationConnectors(this.Shape);

            if (!(allApplication || allNonApplication))
            {
                _resultMessageHelper.AddValidationMessage(Strings.allWaitDecisionsConnectorsSameStencil);
                return;
            }

            if (allNonApplication)
                ValidateNonApplicationSpecific();

            if (allApplication)
            {
                _rulesGeneratorFactory.GetApplicationSpecificFlowShapeValidator
                (
                    this.SourceFile,
                    this.Page,
                    this.Shape,
                    this.ValidationErrors
                ).Validate();
            }

            string waitDecisionXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (waitDecisionXml.Length == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.waitDecisionShapeDataRequired);
                return;
            }

            List<string> errors = new();
            foreach (Shape connector in _shapeHelper.GetOutgoingBlankConnectors(this.Shape))
            {//need the appropriate ApplicationTypeInfo for application specific connectors
                _decisionsElementValidator.Validate
                (
                    _xmlDocumentHelpers.ToXmlElement(waitDecisionXml),
                    GetApplicationTypeInfo(connector),
                    errors
                );
            }
            errors.ForEach(error => _resultMessageHelper.AddValidationMessage(error));

            ApplicationTypeInfo GetApplicationTypeInfo(Shape connector)
            {
                if (connector.Master.NameU == UniversalMasterName.CONNECTOBJECT)
                    return this.Application;

                return _applicationTypeInfoManager.GetApplicationTypeInfo
                (
                    _shapeHelper.GetApplicationList(connector, ShapeBag).OrderBy(n => n).First()
                );
            }
        }

        /// <summary>
        /// Ensures there is only one out going connector and that it is blank.
        /// </summary>
        private void ValidateNonApplicationSpecific()
        {
            if (_shapeHelper.CountOutgoingBlankConnectors(this.Shape) != 1
                || _shapeHelper.CountOutgoingConnectors(this.Shape) != 1)
                _resultMessageHelper.AddValidationMessage(Strings.waitDecisionShapeOutgoingRequired);
        }
    }
}
