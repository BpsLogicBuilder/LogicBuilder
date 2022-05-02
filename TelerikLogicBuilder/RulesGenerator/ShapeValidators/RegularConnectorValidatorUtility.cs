using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators
{
    internal class RegularConnectorValidatorUtility : ShapeValidatorUtility
    {
        private readonly IConnectorElementValidator _connectorElementValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public RegularConnectorValidatorUtility(
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors,
            ApplicationTypeInfo application,
            IContextProvider contextProvider,
            IConnectorElementValidator connectorElementValidator,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper) : base(sourceFile, page, shape, validationErrors, contextProvider)
        {
            _connectorElementValidator = connectorElementValidator;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            Application = application;
        }

        private ApplicationTypeInfo Application { get; }

        internal override void Validate()
        {
            if (this.Shape.Connects.Count != 2)
            {
                AddValidationMessage(Strings.connectorRequires2Shapes);
                return;
            }

            Shape fromShape = _shapeHelper.GetFromShape(this.Shape);
            Shape toShape = _shapeHelper.GetToShape(this.Shape);

            if ((fromShape.Master.NameU == UniversalMasterName.ACTION || fromShape.Master.NameU == UniversalMasterName.JUMPOBJECT) && fromShape == toShape)
                AddValidationMessage(Strings.shapeConnectedToBothEnds);

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
                AddValidationMessage(Strings.dialogFunctionsInvalidConnector);
        }
    }
}
