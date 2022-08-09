using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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

        public IList<string> FindConstructorMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord) 
            => FindConfiguredItemMatches
            (
                _xmlDocumentHelpers.ToXmlDocument(xmlString),
                $"//{XmlDataConstants.CONSTRUCTORELEMENT}",
                searchString,
                matchCase,
                matchWholeWord
            );

        public IList<string> FindFunctionMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord)
            => FindConfiguredItemMatches
            (
                _xmlDocumentHelpers.ToXmlDocument(xmlString),
                $"//{XmlDataConstants.FUNCTIONELEMENT}|//{XmlDataConstants.ASSERTFUNCTIONELEMENT}|//{XmlDataConstants.RETRACTFUNCTIONELEMENT}",
                searchString,
                matchCase,
                matchWholeWord
            );

        public IList<string> FindTextMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord)
            => FindTextMatches
            (
                _xmlDocumentHelpers.ToXmlDocument(xmlString),
                searchString,
                matchCase,
                matchWholeWord
            );

        public IList<string> FindVariableMatches(string xmlString, string searchString, bool matchCase, bool matchWholeWord)
            => FindConfiguredItemMatches
            (
                _xmlDocumentHelpers.ToXmlDocument(xmlString),
                $"//{XmlDataConstants.VARIABLEELEMENT}",
                searchString,
                matchCase,
                matchWholeWord
            );

        public string ReplaceConstructorMatches(string xmlString, string searchString, string replacement, bool matchCase, bool matchWholeWord) 
            => ReplaceConfiguredItemMatches
            (
                _xmlDocumentHelpers.ToXmlDocument(xmlString),
                $"//{XmlDataConstants.CONSTRUCTORELEMENT}",
                searchString,
                replacement,
                matchCase,
                matchWholeWord
            );

        public string ReplaceFunctionMatches(string xmlString, string searchString, string replacement, bool matchCase, bool matchWholeWord)
            => ReplaceConfiguredItemMatches
            (
                _xmlDocumentHelpers.ToXmlDocument(xmlString),
                $"//{XmlDataConstants.FUNCTIONELEMENT}|//{XmlDataConstants.ASSERTFUNCTIONELEMENT}|//{XmlDataConstants.RETRACTFUNCTIONELEMENT}",
                searchString,
                replacement,
                matchCase,
                matchWholeWord
            );

        public string ReplaceTextMatches(string xmlString, string searchString, string replacement, bool matchCase, bool matchWholeWord) 
            => ReplaceTextMatches
            (
                _xmlDocumentHelpers.ToXmlDocument(xmlString),
                searchString,
                replacement,
                matchCase,
                matchWholeWord
            );

        public string ReplaceVariableMatches(string xmlString, string searchString, string replacement, bool matchCase, bool matchWholeWord)
            => ReplaceConfiguredItemMatches
            (
                _xmlDocumentHelpers.ToXmlDocument(xmlString),
                $"//{XmlDataConstants.VARIABLEELEMENT}",
                searchString,
                replacement,
                matchCase,
                matchWholeWord
            );

        private static bool ContainsWord(string content, string searchString, bool matchCase)
            => Regex.IsMatch(content, $"\\b{searchString}\\b", matchCase ? RegexOptions.None : RegexOptions.IgnoreCase);

        private IList<string> FindConfiguredItemMatches(XmlDocument xmlDocument, string xPath, string searchString, bool matchCase, bool matchWholeWord)
            => _xmlDocumentHelpers
                    .SelectElements(xmlDocument, xPath)
                    .Where(n => NameAttributeMatches(n, searchString, matchCase, matchWholeWord))
                    .Select(node => node.GetAttribute(XmlDataConstants.VISIBLETEXTATTRIBUTE))
                    .ToList();

        private IList<string> FindTextMatches(XmlDocument xmlDocument, string searchString, bool matchCase, bool matchWholeWord)
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

        private static bool NameAttributeMatches(XmlElement element, string searchString, bool matchCase, bool matchWholeWord)
            => matchWholeWord
                ? ContainsWord(element.GetAttribute(XmlDataConstants.NAMEATTRIBUTE), searchString, matchCase)
                : element.GetAttribute(XmlDataConstants.NAMEATTRIBUTE).Contains(searchString, matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);

        private string ReplaceConfiguredItemMatches(XmlDocument xmlDocument, string xPath, string searchString, string replacement, bool matchCase, bool matchWholeWord)
        {
            _xmlDocumentHelpers
                .SelectElements(xmlDocument, xPath)
                .Where(n => NameAttributeMatches(n, searchString, matchCase, matchWholeWord))
                .ToList()
                .ForEach
                (
                    node => node.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value = node.GetAttribute(XmlDataConstants.NAMEATTRIBUTE).Replace
                    (
                        searchString, 
                        replacement, 
                        matchCase 
                            ? StringComparison.Ordinal 
                            : StringComparison.OrdinalIgnoreCase
                    )
                );

            return xmlDocument.OuterXml;
        }

        private static string ReplaceTextMatches(XmlDocument xmlDocument, string searchString, string replacement, bool matchCase, bool matchWholeWord)
        {
            xmlDocument.SelectNodes("//text()")!/*SelectNodes is never null when XmlNode is XmlDocument*/
                      .OfType<XmlText>()
                      .Where(n => matchWholeWord/*Value is not null for XmlText*/
                                    ? ContainsWord(n.Value!, searchString, matchCase)
                                    : n.Value!.Contains(searchString, matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase))
                      .ToList()/*Value is not null for XmlText*/
                      .ForEach(node => node.Value = node.Value!.Replace(searchString, replacement, matchCase ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase));

            //This cannot be replaced as text - must be replaced from the UI because the cild element
            //(constructor, function, variable, literalList or objectList) must be updated and validated
            //at the same time.
            //xmlDocument
            //        .SelectNodes($"//{XmlDataConstants.CONNECTORELEMENT}[@{XmlDataConstants.CONNECTORCATEGORYATTRIBUTE}={((int)ConnectorCategory.Dialog).ToString(CultureInfo.InvariantCulture)}]")!/*SelectNodes is never null when XmlNode is XmlDocument*/
            //        .OfType<XmlElement>()
            //        .Select(element => _xmlDocumentHelpers.GetSingleChildElement(element, e => e.Name == XmlDataConstants.METAOBJECTELEMENT))
            //        .Select(element => element.Attributes[XmlDataConstants.OBJECTTYPEATTRIBUTE])
            //        .Where
            //        (
            //            n => matchWholeWord
            //                ? ContainsWord(n!.Value, searchString, matchCase)
            //                : n!.Value.Contains(searchString, matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)
            //        )
            //        .ToList()
            //        .ForEach(node => node!.Value = node!.Value.Replace(searchString, replacement, matchCase ? StringComparison.InvariantCulture : StringComparison.InvariantCultureIgnoreCase));

            return xmlDocument.OuterXml;
        }
    }
}
