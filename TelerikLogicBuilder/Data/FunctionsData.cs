using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class FunctionsData
    {
        public FunctionsData(List<XmlElement> functionElements, XmlElement functionsElement)
        {
            FunctionElements = functionElements;
            FunctionsElement = functionsElement;
        }

        internal List<XmlElement> FunctionElements { get; private set; }
        internal XmlElement FunctionsElement { get; }
    }
}
