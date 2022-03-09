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
        internal string FirstChildElementName { get; }
        internal List<XmlElement> DecisionElements { get; }
        internal XmlElement DecisionsElement { get; }
        #endregion Properties
    }
}
