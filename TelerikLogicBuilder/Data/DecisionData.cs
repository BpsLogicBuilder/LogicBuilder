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

        /// <summary>
        /// True if the decision element is wrapped in a "not" element. (<not><decision></decision></not>)
        /// </summary>
        internal bool IsNotDecision { get; }

        /// <summary>
        /// Name attribute of <decision></decision> element
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// VisibleText attribute of <decision></decision> element
        /// </summary>
        internal string VisibleText { get; }

        /// <summary>
        /// Element Name of first child of the <decision></decision> element (not, function, or, and)
        /// </summary>
        internal string FirstChildElementName { get; }

        /// <summary>
        /// List of function Elements (not, function). Count == 1 if the first child is "not" or "function"
        /// otherwise the count is more than 1.
        /// </summary>
        internal List<XmlElement> FunctionElements { get; }

        /// <summary>
        /// <decision></decision> element
        /// </summary>
        internal XmlElement DecisionElement { get; }
        #endregion Properties
    }
}
