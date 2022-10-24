using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class DecisionsDataParser : IDecisionsDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DecisionsDataParser(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public DecisionsData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.DECISIONSELEMENT)
                throw _exceptionHelper.CriticalException("{19451309-C526-4A62-B15B-D5CAE35010BB}");

            return GetData(_xmlDocumentHelpers.GetSingleChildElement(xmlElement), new List<XmlElement>());
            DecisionsData GetData(XmlElement firstChild, List<XmlElement> list)
            {
                BuildDecisionsList(firstChild, list);
                return new DecisionsData
                (
                    firstChild.Name, //and, or, not, decision
                    list, 
                    xmlElement
                );
            }

            void BuildDecisionsList(XmlElement firstChild, List<XmlElement> list)
            {
                switch (firstChild.Name)
                {
                    case XmlDataConstants.NOTELEMENT:
                    case XmlDataConstants.DECISIONELEMENT:
                        list.Add(firstChild);
                        break;
                    case XmlDataConstants.ANDELEMENT:
                    case XmlDataConstants.ORELEMENT:
                            _xmlDocumentHelpers
                                .GetChildElements(firstChild)
                                .ForEach(node => BuildDecisionsList(node, list));
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{5EF82E45-06DF-4B1F-95E1-36E5C1C7EDCC}");
                }
            }
        }
    }
}
