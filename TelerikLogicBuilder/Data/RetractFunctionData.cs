using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class RetractFunctionData
    {
        public RetractFunctionData(string name, string visibleText, XmlElement variableElement, XmlElement retractFunctionElement)
        {
            Name = name;
            VisibleText = visibleText;
            VariableElement = variableElement;
            RetractFunctionElement = retractFunctionElement;
        }

        /// <summary>
        /// Name attribute of <retractFunction></retractFunction> element
        /// </summary>
        internal string Name { get; private set; }

        /// <summary>
        /// VisibleText attribute of <retractFunction></retractFunction> element
        /// </summary>
        internal string VisibleText { get; private set; }

        /// <summary>
        /// <variable></variable> element
        /// </summary>
        internal XmlElement VariableElement { get; private set; }

        /// <summary>
        /// <retractFunction></retractFunction> element
        /// </summary>
        internal XmlElement RetractFunctionElement { get; private set; }
    }
}
