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
    internal class WaitConditionShapeValidator : IWaitConditionShapeValidator
    {
        private readonly IApplicationSpecificFlowShapeValidator _applicationSpecificFlowShapeValidator;
        private readonly IContextProvider _contextProvider;
        private readonly IConditionsElementValidator _conditionsElementValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;

        public WaitConditionShapeValidator(IApplicationSpecificFlowShapeValidator applicationSpecificFlowShapeValidator, IContextProvider contextProvider, IConditionsElementValidator conditionsElementValidator, IShapeHelper shapeHelper, IShapeXmlHelper shapeXmlHelper)
        {
            _applicationSpecificFlowShapeValidator = applicationSpecificFlowShapeValidator;
            _contextProvider = contextProvider;
            _conditionsElementValidator = conditionsElementValidator;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
        }

        public void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
        {
            new WaitConditionShapeValidatorUtility
            (
                sourceFile,
                page,
                shape,
                validationErrors,
                application,
                _contextProvider,
                _applicationSpecificFlowShapeValidator,
                _conditionsElementValidator,
                _shapeHelper,
                _shapeXmlHelper
            ).Validate();
        }
    }
}
