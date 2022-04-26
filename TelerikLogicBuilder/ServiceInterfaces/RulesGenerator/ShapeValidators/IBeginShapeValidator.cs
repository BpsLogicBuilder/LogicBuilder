using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators
{
    internal interface IBeginShapeValidator
    {
        void Validate(string sourceFile, Page page, Shape shape, List<ResultMessage> validationErrors);
    }
}
