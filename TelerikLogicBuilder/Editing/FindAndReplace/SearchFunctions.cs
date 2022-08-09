using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class SearchFunctions : ISearchFunctions
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public SearchFunctions(IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public List<string> FindTextMatches(XmlDocument xmlDocument, string searchString, bool matchCase, bool matchWholeWord)
        {
            return xmlDocument.SelectNodes("//text()")!/*SelectNodes is never null when XmlNode is XmlDocument*/
                    .OfType<XmlText>()
                    .Where
                    (
                        n => matchWholeWord /*Value is not null for XmlText*/
                        ? ContainsWord(n.Value!, searchString, matchCase)
                        : n.Value!.Contains(searchString, matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)
                    )
                    .Select(node => node.Value)
                    .Concat
                    (
                        _xmlDocumentHelpers
                            .SelectElements(xmlDocument, $"//{XmlDataConstants.CONNECTORELEMENT}[@{XmlDataConstants.CONNECTORCATEGORYATTRIBUTE}={((int)ConnectorCategory.Dialog).ToString(CultureInfo.InvariantCulture)}]")
                            .Select(element => _xmlDocumentHelpers.GetSingleChildElement(element, e => e.Name == XmlDataConstants.METAOBJECTELEMENT))
                            .Select(element => element.GetAttribute(XmlDataConstants.OBJECTTYPEATTRIBUTE))
                            .Where
                            (
                                attributeValue => matchWholeWord
                                    ? ContainsWord(attributeValue, searchString, matchCase)
                                    : attributeValue.Contains(searchString, matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)
                            )
                    ).ToList()!;
        }

        public List<string> FindTextMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord)
            => FindTextMatches
            (
                _xmlDocumentHelpers.ToXmlDocument(xmlString), 
                searchString, 
                matchCase, 
                matchWholeWord
            );

        private static bool ContainsWord(string content, string searchString, bool matchCase) 
            => Regex.IsMatch(content, $"\\b{searchString}\\b", matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);
    }
}
