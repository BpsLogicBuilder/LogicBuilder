using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ObjectListParameterData
    {
        public ObjectListParameterData(string parameterName, XmlElement childElement, XmlElement objectListParameterElement, IEnumHelper enumHelper)
        {
            ParameterName = parameterName;
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            ObjectListParameterElement = objectListParameterElement;
        }

        /// <summary>
        /// Name attribute of <objectListParameter></objectListParameter> element
        /// </summary>
        internal string ParameterName { get; }

        /// <summary>
        /// Child element of <objectListParameter></objectListParameter> (variable, function, constructor, literalList, objectList)
        /// (Usually literalList)
        /// </summary>
        internal XmlElement ChildElement { get; }

        /// <summary>
        /// ChildElementCategory depends on the child element (variable, function, constructor, literalList, or objectList)
        /// </summary>
        internal ObjectCategory ChildElementCategory { get; }

        /// <summary>
        /// <objectListParameter></objectListParameter> element
        /// </summary>
        internal XmlElement ObjectListParameterElement { get; }
    }
}
