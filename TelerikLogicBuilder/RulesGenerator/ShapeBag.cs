using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator
{
    internal class ShapeBag
    {
        public ShapeBag(Shape shape, IList<string> otherConnectorApplications)
        {
            Shape = shape;
            OtherConnectorApplications = otherConnectorApplications;
        }

        public ShapeBag(Shape shape)
        {
            Shape = shape;
            OtherConnectorApplications = null;
        }

        internal Shape Shape { get; }
        internal IList<string>? OtherConnectorApplications { get; }
    }
}
