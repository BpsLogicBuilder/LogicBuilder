using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class DecisionsData
    {
        public DecisionsData(string firstChildElementName, List<XmlElement> decisionElements, XmlElement decisionsElement)
        {
            FirstChildElementName = firstChildElementName;
            DecisionElements = decisionElements;
            DecisionsElement = decisionsElement;
        }


        #region Properties

        /// <summary>
        /// Element Name of first child to <decisions></decisions> element (and, or, decision, not)
        /// </summary>
        internal string FirstChildElementName { get; }

        /// <summary>
        /// Decision Elements (decision, not)
        /// </summary>
        internal List<XmlElement> DecisionElements { get; }

        /// <summary>
        /// <decisions></decisions> element
        /// </summary>
        internal XmlElement DecisionsElement { get; }
        #endregion Properties
    }
}
