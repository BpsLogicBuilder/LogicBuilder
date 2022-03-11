using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class ConstructorData
    {
        internal ConstructorData(string name, string visibleText, List<GenericConfigBase> genericArguments, List<XmlElement> parameterXmlNodeList, XmlElement constructorElement)
        {
            this.Name = name;
            this.VisibleText = visibleText;
            this.GenericArguments = genericArguments;
            this.ParameterElementsList = parameterXmlNodeList;
            this.ConstructorElement = constructorElement;
        }

        #region Properties

        /// <summary>
        /// Name attribute of <constructor></constructor> element
        /// </summary>
        internal string Name { get;}

        /// <summary>
        /// VisibleText attribute of <constructor></constructor> element
        /// </summary>
        internal string VisibleText { get; }

        /// <summary>
        /// Configured Generic Arguments
        /// </summary>
        internal List<GenericConfigBase> GenericArguments { get; }

        /// <summary>
        /// Collection of parameter elements (literalParameter, objectParameter, literalListParameter, objectListParameter)
        /// (Child elements of the <parameters></parameters> element)
        /// </summary>
        internal List<XmlElement> ParameterElementsList { get; }

        /// <summary>
        /// <constructor></constructor> element
        /// </summary>
        internal XmlElement ConstructorElement { get; }
        #endregion Properties
    }
}
