using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Data
{
    internal class FunctionData
    {
        internal FunctionData(string name, string visibleText, List<GenericConfigBase> genericArguments, List<XmlElement> parameterElementsList, XmlElement functionElement, bool isNotFunction)
        {
            this.Name = name;
            this.GenericArguments = genericArguments;
            this.ParameterElementsList = parameterElementsList;
            this.FunctionElement = functionElement;
            this.VisibleText = visibleText;
            this.IsNotFunction = isNotFunction;
        }

        #region Properties

        /// <summary>
        /// True if the function element is wrapped in a "not" element. (<not><function></function></not>)
        /// </summary>
        internal bool IsNotFunction { get; }

        /// <summary>
        /// Name attribute of <function></function> element
        /// </summary>
        internal string Name { get; }

        /// <summary>
        /// VisibleText attribute of <function></function> element
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
        /// <function></function> element
        /// </summary>
        internal XmlElement FunctionElement { get; }
        #endregion Properties
    }
}
