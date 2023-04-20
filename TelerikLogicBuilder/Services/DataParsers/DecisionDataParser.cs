using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class DecisionDataParser : IDecisionDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DecisionDataParser(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public DecisionData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.DECISIONELEMENT
                && xmlElement.Name != XmlDataConstants.NOTELEMENT)
                throw _exceptionHelper.CriticalException("{82D2AEBC-2621-4225-BEE4-CB2532822F5C}");

            return GetDecisionData
            (
                xmlElement.Name == XmlDataConstants.NOTELEMENT
                    ? _xmlDocumentHelpers.GetSingleChildElement(xmlElement)
                    : xmlElement
            );

            DecisionData GetDecisionData(XmlElement decisionElement)
                => GetData(decisionElement, _xmlDocumentHelpers.GetSingleChildElement(decisionElement), new List<XmlElement>());

            DecisionData GetData(XmlElement decisionElement, XmlElement firstChild, List<XmlElement> list)
            {
                BuildPredicatesList(firstChild, list);
                return new DecisionData
                (
                    decisionElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                    decisionElement.Attributes[XmlDataConstants.VISIBLETEXTATTRIBUTE]!.Value,
                    firstChild.Name,//or, and, not, function
                    list,
                    xmlElement,
                    xmlElement.Name == XmlDataConstants.NOTELEMENT//IsNotDecision
                );
            }

            void BuildPredicatesList(XmlElement firstChild, List<XmlElement> list)
            {
                switch (firstChild.Name)
                {
                    case XmlDataConstants.NOTELEMENT:
                    case XmlDataConstants.FUNCTIONELEMENT:
                        list.Add(firstChild);
                        break;
                    case XmlDataConstants.ANDELEMENT:
                    case XmlDataConstants.ORELEMENT:
                            _xmlDocumentHelpers
                                .GetChildElements(firstChild)
                                .ForEach(node => BuildPredicatesList(node, list));
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{4C0AE838-C3E2-4012-94EB-BEAC1E28A9F6}");
                }
            }
        }
    }
}
