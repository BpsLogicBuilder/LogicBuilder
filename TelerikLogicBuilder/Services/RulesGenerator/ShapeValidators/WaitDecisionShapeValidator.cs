﻿using ABIS.LogicBuilder.FlowBuilder.Reflection;
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
    internal class WaitDecisionShapeValidator : IWaitDecisionShapeValidator
    {
        private readonly IApplicationSpecificFlowShapeValidator _applicationSpecificFlowShapeValidator;
        private readonly IContextProvider _contextProvider;
        private readonly IDecisionsElementValidator _decisionsElementValidator;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;

        public WaitDecisionShapeValidator(IApplicationSpecificFlowShapeValidator applicationSpecificFlowShapeValidator, IContextProvider contextProvider, IDecisionsElementValidator decisionsElementValidator, IShapeHelper shapeHelper, IShapeXmlHelper shapeXmlHelper)
        {
            _applicationSpecificFlowShapeValidator = applicationSpecificFlowShapeValidator;
            _contextProvider = contextProvider;
            _decisionsElementValidator = decisionsElementValidator;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
        }

        public void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
        {
            new WaitDecisionShapeValidatorUtility
            (
                sourceFile,
                page,
                shape,
                validationErrors,
                application,
                _contextProvider,
                _applicationSpecificFlowShapeValidator,
                _decisionsElementValidator,
                _shapeHelper,
                _shapeXmlHelper
            ).Validate();
        }
    }
}