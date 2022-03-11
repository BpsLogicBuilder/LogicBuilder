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

        /// <summary>
        /// List of <function></function> elements
        /// </summary>
        internal List<XmlElement> FunctionElements { get; }

        /// <summary>
        /// <functions></functions> element
        /// </summary>
        internal XmlElement FunctionsElement { get; }
    }
}
