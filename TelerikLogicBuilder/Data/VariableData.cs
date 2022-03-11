using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class VariableData
    {
        public VariableData(string name, string visibleText, XmlElement variableElement)
        {
            Name = name;
            VisibleText = visibleText;
            VariableElement = variableElement;
        }

        /// <summary>
        /// Name attribute of <variable></variable> element
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// VisibleText attribute of <variable></variable> element
        /// </summary>
        internal string VisibleText { get; }

        /// <summary>
        /// <variable></variable> element
        /// </summary>
        internal XmlElement VariableElement { get; private set; }
    }
}
