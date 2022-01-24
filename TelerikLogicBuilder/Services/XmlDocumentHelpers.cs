using System.Globalization;
using System.IO;
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
        #endregion Methods
    }
}
