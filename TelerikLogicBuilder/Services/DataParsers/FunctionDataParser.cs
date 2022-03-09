using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class FunctionDataParser : IFunctionDataParser
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IGenericConfigXmlParser _genericConfigXmlParser;

        public FunctionDataParser(IContextProvider contextProvider, IGenericConfigXmlParser genericConfigXmlParser)
        {
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _genericConfigXmlParser = genericConfigXmlParser;
        }

        public FunctionData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.FUNCTIONELEMENT
                && xmlElement.Name != XmlDataConstants.NOTELEMENT)
                throw _exceptionHelper.CriticalException("{7B301C9C-CF9D-4775-8771-92A9AB45C01F}");

            return GetData
            (
                xmlElement.Name == XmlDataConstants.NOTELEMENT 
                    ? _xmlDocumentHelpers.GetSingleChildElement(xmlElement)
                    : xmlElement
            );

            FunctionData GetData(XmlElement functionElement)
            {
                return new FunctionData
                (
                    functionElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                    functionElement.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE),
                    _xmlDocumentHelpers.GetChildElements
                    (
                        _xmlDocumentHelpers.GetSingleChildElement
                        (
                            functionElement, 
                            e => e.Name == XmlDataConstants.GENERICARGUMENTSELEMENT
                        )
                    )
                    .Select(e => _genericConfigXmlParser.Parse(e)).ToList(),
                    _xmlDocumentHelpers.GetChildElements
                    (
                        _xmlDocumentHelpers.GetSingleChildElement
                        (
                            functionElement, 
                            e => e.Name == XmlDataConstants.PARAMETERSELEMENT
                        )
                    ),
                    xmlElement,//not or function
                    xmlElement.Name == XmlDataConstants.NOTELEMENT
                );
            }
        }
    }
}
