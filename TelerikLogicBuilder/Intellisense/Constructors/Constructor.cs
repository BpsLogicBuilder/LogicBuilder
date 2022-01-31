using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors
{
    internal class Constructor
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public Constructor(string Name, string TypeName, List<ParameterBase> Parameters, List<string> genericArguments, string Summary, IContextProvider contextProvider)
        {
            this.Name = Name;
            this.TypeName = TypeName;
            this.Parameters = Parameters;
            this.GenericArguments = genericArguments;
            this.Summary = Summary;
            this._xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        /// <summary>
        /// Unique name for this constructor
        /// </summary>
        internal string Name { get; private set; }

        /// <summary>
        /// Fully Qualified class for for this constructor
        /// </summary>
        internal string TypeName { get; private set; }

        /// <summary>
        /// Parameters for this constructor
        /// </summary>
        internal List<ParameterBase> Parameters { get; private set; }

        /// <summary>
        /// Generic Arguments
        /// </summary>
        internal List<string> GenericArguments { get; private set; }

        /// <summary>
        /// Has Generic Arguments
        /// </summary>
        internal bool HasGenericArguments => GenericArguments.Count > 0;

        /// <summary>
        /// Comments about this constructor
        /// </summary>
        internal string Summary { get; private set; }

        internal string ToXml => this.BuildXml();
        #endregion Properties

        #region Methods
        public override string ToString()
        {
            return string.Format
            (
                CultureInfo.CurrentCulture, Strings.constructorToStringFormat,
                this.Name,
                this.TypeName,
                string.Join
                (
                    string.Concat(MiscellaneousConstants.COMMASTRING, " "),
                    this.Parameters.Select(p => p.ToString())
                )
            );
        }

        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.CONSTRUCTORELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                    xmlTextWriter.WriteElementString(XmlDataConstants.TYPENAMEELEMENT, this.TypeName);
                    xmlTextWriter.WriteStartElement(XmlDataConstants.PARAMETERSELEMENT);
                    foreach (ParameterBase parameter in this.Parameters)
                        xmlTextWriter.WriteRaw(parameter.ToXml);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteStartElement(XmlDataConstants.GENERICARGUMENTSELEMENT);
                    foreach (string item in this.GenericArguments)
                    {
                        xmlTextWriter.WriteStartElement(XmlDataConstants.ITEMELEMENT);
                        xmlTextWriter.WriteString(item);
                        xmlTextWriter.WriteEndElement();
                    }
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteElementString(XmlDataConstants.SUMMARYELEMENT, this.Summary);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}
