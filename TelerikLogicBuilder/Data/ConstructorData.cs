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
        internal string Name { get; private set; }
        internal string VisibleText { get; private set; }
        internal List<GenericConfigBase> GenericArguments { get; private set; }
        internal List<XmlElement> ParameterElementsList { get; private set; }
        internal XmlElement ConstructorElement { get; private set; }
        #endregion Properties
    }
}
