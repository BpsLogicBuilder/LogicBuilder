﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.Intellisense.Functions
{
    internal class FunctionXmlParser : IFunctionXmlParser
    {
        private readonly IContextProvider _contextProvider;
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IParametersXmlParser _parametersXmlParser;
        private readonly IReturnTypeXmlParser _returnTypeXmlParser;

        public FunctionXmlParser(IContextProvider contextProvider, IParametersXmlParser parametersXmlParser, IReturnTypeXmlParser returnTypeXmlParser)
        {
            _enumHelper = contextProvider.EnumHelper;
            _exceptionHelper = contextProvider.ExceptionHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
            _contextProvider = contextProvider;
            _parametersXmlParser = parametersXmlParser;
            _returnTypeXmlParser = returnTypeXmlParser;
        }

        public Function Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.FUNCTIONELEMENT)
                throw _exceptionHelper.CriticalException("{4EDEE8F0-D5DA-4AE6-A655-EAFBE6DD0708}");

            return GetFunction(_xmlDocumentHelpers.GetChildElements(xmlElement).ToDictionary(e => e.Name));

            Function GetFunction(IDictionary<string, XmlElement> elements)
            {
                return new Function
                (
                    xmlElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                    elements[XmlDataConstants.MEMBERNAMEELEMENT].InnerText,
                    _enumHelper.ParseEnumText<FunctionCategories>(elements[XmlDataConstants.FUNCTIONCATEGORYELEMENT].InnerText),
                    elements[XmlDataConstants.TYPENAMEELEMENT].InnerText,
                    elements[XmlDataConstants.REFERENCENAMEELEMENT].InnerText,
                    elements[XmlDataConstants.REFERENCEDEFINITIONELEMENT].InnerText,
                    elements[XmlDataConstants.CASTREFERENCEASELEMENT].InnerText,
                    _enumHelper.ParseEnumText<ReferenceCategories>(elements[XmlDataConstants.REFERENCECATEGORYELEMENT].InnerText),
                    _enumHelper.ParseEnumText<ParametersLayout>(elements[XmlDataConstants.PARAMETERSLAYOUTELEMENT].InnerText),
                    _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.PARAMETERSELEMENT]).Select(e => _parametersXmlParser.Parse(e)).ToList(),
                    _xmlDocumentHelpers.GetChildElements(elements[XmlDataConstants.GENERICARGUMENTSELEMENT]).Select(e => e.InnerText).ToList(),
                    _returnTypeXmlParser.Parse(_xmlDocumentHelpers.GetSingleChildElement(elements[XmlDataConstants.RETURNTYPEELEMENT])),
                    elements[XmlDataConstants.SUMMARYELEMENT].InnerText,
                    _contextProvider
                );
            }
        }
    }
}