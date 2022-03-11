using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ConditionsData
    {
        public ConditionsData(string firstChildElementName, List<XmlElement> functionElements, XmlElement conditionsElement)
        {
            FirstChildElementName = firstChildElementName;
            FunctionElements = functionElements;
            ConditionsElement = conditionsElement;
        }

        #region Properties
        /// <summary>
        /// Element Name of the first child of the <conditions></conditions> element. (not, function, or, and)
        /// </summary>
        internal string FirstChildElementName { get; }

        /// <summary>
        /// List function elements (not, function)
        /// </summary>
        internal List<XmlElement> FunctionElements { get; }

        /// <summary>
        /// <conditions></conditions> element
        /// </summary>
        internal XmlElement ConditionsElement { get; }
        #endregion Properties
    }
}
