using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace Contoso.Test.Flow.Test
{
    public class ConstructorXmlBuilder : ExpressionVisitor
    {
        private readonly XmlDocument xmlDocument = new();

        public static string ToContructorDefinitionXml(Expression expression)
        {
            ConstructorXmlBuilder visitor = new();
            visitor.Visit(expression);
            return visitor.xmlDocument.DocumentElement.OuterXml;
        }
    }
}
