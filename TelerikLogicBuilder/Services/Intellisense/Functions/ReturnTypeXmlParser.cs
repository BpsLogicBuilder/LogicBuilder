using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using System;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class ReturnTypeXmlParser : IReturnTypeXmlParser
    {
        private readonly IContextProvider _contextProvider;

        public ReturnTypeXmlParser(IContextProvider contextProvider)
        {
            _contextProvider = contextProvider;
        }

        public ReturnTypeBase Parse(XmlElement xmlElement) 
            => new ReturnTypeXmlParserUtility(xmlElement, _contextProvider).ReturnType;
    }
}
