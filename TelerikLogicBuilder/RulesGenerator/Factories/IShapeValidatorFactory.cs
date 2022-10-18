using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal interface IShapeValidatorFactory
    {
        IApplicationSpecificFlowShapeValidator GetApplicationSpecificFlowShapeValidator(string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors);

        IShapeValidator GetConnectorValidator(string sourceFile, Page page, Shape connector, List<ResultMessage> validationErrors, ApplicationTypeInfo application);

        IShapeValidator GetShapeValidator(string sourceFile, Page page, ShapeBag shapeBag, List<ResultMessage> validationErrors, ApplicationTypeInfo application);
    }
}
