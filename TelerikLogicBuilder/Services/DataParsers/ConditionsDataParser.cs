using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.DataParsers
{
    internal class ConditionsDataParser : IConditionsDataParser
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConditionsDataParser(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public ConditionsData Parse(XmlElement xmlElement)
        {
            if (xmlElement.Name != XmlDataConstants.CONDITIONSELEMENT)
                throw _exceptionHelper.CriticalException("{8F2BB41C-3F27-4D59-B0BE-13A9B33AA16B}");

            return GetData(new List<XmlElement>(), (XmlElement)xmlElement.FirstChild!);//FirstChild is never null per schema
            ConditionsData GetData(List<XmlElement> list, XmlElement firstChild)
            {
                BuildConditionsList(firstChild, list);
                return new ConditionsData(firstChild.Name, list, xmlElement);
            }

            void BuildConditionsList(XmlElement firstChild, List<XmlElement> list)
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
                            .ForEach(node => BuildConditionsList(node, list));
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{20141838-7A2E-4285-B27B-B4A89B163749}");
                }
            }
        }
    }
}
