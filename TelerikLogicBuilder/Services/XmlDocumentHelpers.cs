using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class XmlDocumentHelpers : IXmlDocumentHelpers
    {
        #region Properties
        //private static XmlWriterSettings FormattedSettings
        //{
        //    get
        //    {
        //        return new XmlWriterSettings
        //        {
        //            Indent = true,
        //            IndentChars = "\t",
        //            OmitXmlDeclaration = true
        //        };
        //    }
        //}

        //private static XmlWriterSettings FormattedSettingsWithDeclaration
        //{
        //    get
        //    {
        //        return new XmlWriterSettings
        //        {
        //            Indent = true,
        //            IndentChars = "\t"
        //        };
        //    }
        //}

        private static XmlWriterSettings UnformattedSettings
        {
            get
            {
                return new XmlWriterSettings
                {
                    OmitXmlDeclaration = true
                };
            }
        }

        //private static XmlWriterSettings FragmentSettings
        //{
        //    get
        //    {
        //        return new XmlWriterSettings
        //        {
        //            OmitXmlDeclaration = true,
        //            ConformanceLevel = ConformanceLevel.Fragment
        //        };
        //    }
        //}
        #endregion Properties

        #region Methods
        public XmlWriter CreateUnformattedXmlWriter(StringBuilder stringBuilder) 
            => XmlWriter.Create(new StringWriter(stringBuilder, CultureInfo.InvariantCulture), UnformattedSettings);

        public List<XmlElement> GetChildElements(XmlNode xmlNode, 
            Func<XmlElement, bool> filter = null, 
            Func<IEnumerable<XmlElement>, IEnumerable<XmlElement>> enumerableFunc = null)
        {
            IEnumerable<XmlElement> getChildElements()
            {
                return xmlNode.ChildNodes.OfType<XmlElement>().Where(filter ?? (x => true));
            }

            return enumerableFunc != null
                ? enumerableFunc(getChildElements()).ToList()
                : getChildElements().ToList();
        }
        #endregion Methods
    }
}
