using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal class ShapeValidatorFactory : IShapeValidatorFactory
    {
        private readonly Func<string, Page, Shape, List<ResultMessage>, IApplicationSpecificFlowShapeValidator> _getApplicationSpecificFlowShapeValidator;
        private readonly Func<string, Page, Shape, List<ResultMessage>, ApplicationTypeInfo, IShapeValidator> _getConnectorValidator;
        private readonly Func<string, Page, ShapeBag, List<ResultMessage>, ApplicationTypeInfo, IShapeValidator> _getShapeValidator;

        public ShapeValidatorFactory(
            Func<string, Page, Shape, List<ResultMessage>, IApplicationSpecificFlowShapeValidator> getApplicationSpecificFlowShapeValidator,
            Func<string, Page, Shape, List<ResultMessage>, ApplicationTypeInfo, IShapeValidator> getConnectorValidator,
            Func<string, Page, ShapeBag, List<ResultMessage>, ApplicationTypeInfo, IShapeValidator> getShapeValidator)
        {
            _getApplicationSpecificFlowShapeValidator = getApplicationSpecificFlowShapeValidator;
            _getConnectorValidator = getConnectorValidator;
            _getShapeValidator = getShapeValidator;
        }

        public IApplicationSpecificFlowShapeValidator GetApplicationSpecificFlowShapeValidator(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors)
            => _getApplicationSpecificFlowShapeValidator(sourceFile, page, shape, validationErrors);

        public IShapeValidator GetConnectorValidator(string sourceFile, Page page, Shape connector, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
             => _getConnectorValidator(sourceFile, page, connector, validationErrors, application);

        public IShapeValidator GetShapeValidator(string sourceFile, Page page, ShapeBag shapeBag, List<ResultMessage> validationErrors, ApplicationTypeInfo application)
            => _getShapeValidator(sourceFile, page, shapeBag, validationErrors, application);
    }
}
