using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator.ShapeValidators
{
    internal class DialogShapeValidator : IDialogShapeValidator
    {
        private readonly IContextProvider _contextProvider;
        private readonly IFunctionsElementValidator _functionsElementValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DialogShapeValidator(IContextProvider contextProvider, IFunctionsElementValidator functionsElementValidator, IShapeHelper shapeHelper, IShapeXmlHelper shapeXmlHelper, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _contextProvider = contextProvider;
            _functionsElementValidator = functionsElementValidator;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
        {
            new DialogShapeValidatorUtility
            (
                sourceFile,
                page,
                shape,
                validationErrors,
                application,
                _contextProvider,
                _functionsElementValidator,
                _shapeHelper,
                _shapeXmlHelper,
                _xmlDocumentHelpers
            ).Validate();
        }
    }
}
