using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;
using System.Globalization;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class DialogShapeValidatorUtility : ShapeValidatorUtility
    {
        private readonly IFunctionsElementValidator _functionsElementValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DialogShapeValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            ApplicationTypeInfo application,
            IContextProvider contextProvider,
            IFunctionsElementValidator functionsElementValidator,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _functionsElementValidator = functionsElementValidator;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            Application = application;
        }

        private ApplicationTypeInfo Application { get; }

        internal override void Validate()
        {
            if (_shapeHelper.CountFunctions(this.Shape) > 1)
                AddValidationMessage(Strings.dialogShapeOnlyOneFunction);

            if (_shapeHelper.CountDialogFunctions(this.Shape) != 1)
                AddValidationMessage(Strings.dialogShapesOneDialogFunction);

            if (_shapeHelper.CountIncomingConnectors(this.Shape) < 1)
                AddValidationMessage(Strings.dialogShapeRequiresIncomming);

            if (this.Shape.FromConnects.Count < 1)
                return;

            int outGoingConnectors = _shapeHelper.CountOutgoingConnectors(this.Shape);
            if (outGoingConnectors == 0)
            {
                AddValidationMessage(Strings.dialogOrQuestionsOutgoingCount);
                return;
            }

            int blankOutGoingConnectors = _shapeHelper.CountOutgoingBlankConnectors(this.Shape);
            if (outGoingConnectors > 1 && blankOutGoingConnectors > 0)
            {
                AddValidationMessage(Strings.dialogOrQuestionsOutgoingCount);
                return;
            }

            if (outGoingConnectors == 1 && blankOutGoingConnectors == 1)
            {
                Shape connector = _shapeHelper.GetOutgoingBlankConnector(this.Shape);
                Shape toShape = _shapeHelper.GetToShape(connector);

                if (toShape.Master.NameU != UniversalMasterName.ENDFLOW)
                    AddValidationMessage(Strings.blankConnectorExitingDialogMustEndFlow);

                return;
            }

            //now outgoing >= 1 && blank == 0
            short invalidConnectors = _shapeHelper.CountInvalidMultipleChoiceConnectors(this.Shape);
            if (invalidConnectors > 0)
                AddValidationMessage(string.Format(CultureInfo.CurrentCulture, Strings.dialogInvalidConnectorsFormat, invalidConnectors));

            short duplicateChoice = _shapeHelper.CheckForDuplicateMultipleChoices(this.Shape);
            if (duplicateChoice > 0)
                AddValidationMessage(string.Format(CultureInfo.CurrentCulture, Strings.dialogDuplicateChoiceFormat, duplicateChoice));

            string functionsXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (functionsXml.Length == 0)
            {
                AddValidationMessage(Strings.dialogShapeDataRequired);
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
