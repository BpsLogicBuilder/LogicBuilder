using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class LiteralListParameterData
    {
        public LiteralListParameterData(string parameterName, XmlElement childElement, XmlElement literalListParameterElement, IEnumHelper enumHelper)
        {
            ParameterName = parameterName;
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            LiteralListParameterElement = literalListParameterElement;
        }

        /// <summary>
        /// Name attribute of <literalListParameter></literalListParameter> element
        /// </summary>
        internal string ParameterName { get; }

        /// <summary>
        /// Child element of <literalListParameter></literalListParameter> (variable, function, constructor, literalList, objectList)
        /// (Usually literalList)
        /// </summary>
        internal XmlElement ChildElement { get; }

        /// <summary>
        /// ChildElementCategory depends on the child element (variable, function, constructor, literalList, or objectList)
        /// </summary>
        internal ObjectCategory ChildElementCategory { get; }

        /// <summary>
        /// <literalListParameter></literalListParameter> element
        /// </summary>
        internal XmlElement LiteralListParameterElement { get; }
    }
}
