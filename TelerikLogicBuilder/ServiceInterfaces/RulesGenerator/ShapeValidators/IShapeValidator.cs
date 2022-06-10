using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators
{
    internal interface IShapeValidator
    {
        void ValidateShape(string sourceFile, Page page, ShapeBag shapeBag, List<ResultMessage> validationErrors, ApplicationTypeInfo application);
        void ValidateConnector(string sourceFile, Page page, Shape connector, List<ResultMessage> validationErrors, ApplicationTypeInfo application);
    }
}
