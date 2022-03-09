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
        internal string FirstChildElementName { get; }
        internal List<XmlElement> FunctionElements { get; }
        internal XmlElement ConditionsElement { get; }
        #endregion Properties
    }
}
