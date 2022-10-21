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

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class RegularConnectorValidator : IConnectorValidator
    {
        private readonly IConnectorElementValidator _connectorElementValidator;
        private readonly IResultMessageHelper _resultMessageHelper;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public RegularConnectorValidator(
            IConnectorElementValidator connectorElementValidator,
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
            _connectorElementValidator = connectorElementValidator;
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

            Shape = shape;
            Application = application;
        }

        private Shape Shape { get; }
        private ApplicationTypeInfo Application { get; }

        public void Validate()
        {
            if (this.Shape.Connects.Count != 2)
            {
                _resultMessageHelper.AddValidationMessage(Strings.connectorRequires2Shapes);
                return;
            }

            Shape fromShape = _shapeHelper.GetFromShape(this.Shape);
            Shape toShape = _shapeHelper.GetToShape(this.Shape);

            if ((fromShape.Master.NameU == UniversalMasterName.ACTION || fromShape.Master.NameU == UniversalMasterName.JUMPOBJECT) && fromShape == toShape)
                _resultMessageHelper.AddValidationMessage(Strings.shapeConnectedToBothEnds);

            string connectorXml = _shapeXmlHelper.GetXmlString(this.Shape);
            if (connectorXml.Length == 0)
                return;

            List<string> errors = new();
            _connectorElementValidator.Validate
            (
                _xmlDocumentHelpers.ToXmlElement(connectorXml),
                Application,
                errors
            );
            if (errors.Count > 0)
                return;

            int dialogFunctions = _shapeHelper.CountDialogFunctions(this.Shape);
            if (dialogFunctions > 0)
                _resultMessageHelper.AddValidationMessage(Strings.dialogFunctionsInvalidConnector);
        }
    }
}
