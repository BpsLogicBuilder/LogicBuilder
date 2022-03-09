using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class DecisionData
    {
        internal DecisionData(string name, string visibleText, string firstChildElementName, List<XmlElement> functionElements, XmlElement decisionElement, bool isNotDecision)
        {
            this.Name = name;
            this.VisibleText = visibleText;
            this.FirstChildElementName = firstChildElementName;
            this.FunctionElements = functionElements;
            this.DecisionElement = decisionElement;
            this.IsNotDecision = isNotDecision;
        }

        #region Properties
        internal bool IsNotDecision { get; }

        internal string Name { get; }
        internal string VisibleText { get; }
        internal string FirstChildElementName { get; }
        internal List<XmlElement> FunctionElements { get; }
        internal XmlElement DecisionElement { get; }
        #endregion Properties
    }
}
