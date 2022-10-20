using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IGetRuleShapes
    {
        void GetShapes(Shape connector, IList<ShapeBag> ruleShapes, IList<Shape> ruleConnectors);
    }
}
