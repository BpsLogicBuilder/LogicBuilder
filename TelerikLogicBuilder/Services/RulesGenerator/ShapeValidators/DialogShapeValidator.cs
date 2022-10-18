using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class DialogShapeValidator : IShapeValidator
    {
        private readonly IFunctionsElementValidator _functionsElementValidator;
        private readonly IResultMessageHelper _resultMessageHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DialogShapeValidator(
            IFunctionsElementValidator functionsElementValidator,
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
            _functionsElementValidator = functionsElementValidator;
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
            if (_shapeHelper.CountFunctions(this.Shape) > 1)
                _resultMessageHelper.AddValidationMessage(Strings.dialogShapeOnlyOneFunction);

            if (_shapeHelper.CountDialogFunctions(this.Shape) != 1)
                _resultMessageHelper.AddValidationMessage(Strings.dialogShapesOneDialogFunction);

            if (_shapeHelper.CountIncomingConnectors(this.Shape) < 1)
                _resultMessageHelper.AddValidationMessage(Strings.dialogShapeRequiresIncoming);

            if (this.Shape.FromConnects.Count < 1)
                return;

            int outGoingConnectors = _shapeHelper.CountOutgoingConnectors(this.Shape);
            if (outGoingConnectors == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.dialogOrQuestionsOutgoingCount);
                return;
            }

            int blankOutGoingConnectors = _shapeHelper.CountOutgoingBlankConnectors(this.Shape);
            if (outGoingConnectors > 1 && blankOutGoingConnectors > 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.dialogOrQuestionsOutgoingCount);
                return;
            }

            if (outGoingConnectors == 1 && blankOutGoingConnectors == 1)
            {
                Shape connector = _shapeHelper.GetOutgoingBlankConnector(this.Shape);
                Shape toShape = _shapeHelper.GetToShape(connector);

                if (toShape.Master.NameU != UniversalMasterName.ENDFLOW)
                    _resultMessageHelper.AddValidationMessage(Strings.blankConnectorExitingDialogMustEndFlow);

                return;
            }

            //now outgoing >= 1 && blank == 0
            short invalidConnectors = _shapeHelper.CountInvalidMultipleChoiceConnectors(this.Shape);
            if (invalidConnectors > 0)
                _resultMessageHelper.AddValidationMessage(string.Format(CultureInfo.CurrentCulture, Strings.dialogInvalidConnectorsFormat, invalidConnectors));

            short duplicateChoice = _shapeHelper.CheckForDuplicateMultipleChoices(this.Shape);
            if (duplicateChoice > 0)
                _resultMessageHelper.AddValidationMessage(string.Format(CultureInfo.CurrentCulture, Strings.dialogDuplicateChoiceFormat, duplicateChoice));

            string functionsXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (functionsXml.Length == 0)
            {
                _resultMessageHelper.AddValidationMessage(Strings.dialogShapeDataRequired);
                return;
            }

            List<string> errors = new();
            _functionsElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(functionsXml),
                this.Application,
                errors
            );
            errors.ForEach(error => _resultMessageHelper.AddValidationMessage(error));
        }
    }
}
