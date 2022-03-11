using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ObjectParameterData
    {
        public ObjectParameterData(string parameterName, XmlElement childElement, XmlElement objectParameterElement, IEnumHelper enumHelper)
        {
            ParameterName = parameterName;
            ChildElement = childElement;
            ChildElementCategory = enumHelper.GetObjectCategory(ChildElement.Name);
            ObjectParameterElement = objectParameterElement;
        }

        /// <summary>
        /// Name attribute of <objectParameter></objectParameter> element
        /// </summary>
        internal string ParameterName { get; }

        /// <summary>
        /// Child element of <objectParameter></objectParameter> (variable, function, constructor, literalList, objectList)
        /// (Usually constructor)
        /// </summary>
        internal XmlElement ChildElement { get; }

        /// <summary>
        /// ChildElementCategory depends on the child element (variable, function, constructor, literalList, or objectList)
        /// </summary>
        internal ObjectCategory ChildElementCategory { get; }

        /// <summary>
        /// <objectParameter></objectParameter> element
        /// </summary>
        internal XmlElement ObjectParameterElement { get; }
    }
}
