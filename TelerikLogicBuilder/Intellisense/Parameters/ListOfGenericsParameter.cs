using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ListOfGenericsParameter : ParameterBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ListOfGenericsParameter(
            IEnumHelper enumHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string name,
            bool isOptional,
            string comments,
            string genericArgumentName,
            ListType listType,
            ListParameterInputStyle control)
            : base(name, isOptional, comments)
        {
            this.GenericArgumentName = genericArgumentName;
            this.ListType = listType;
            this.Control = control;
            this._enumHelper = enumHelper;
            this._xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        internal string GenericArgumentName { get; private set; }
        internal ListType ListType { get; private set; }
        internal ListParameterInputStyle Control { get; private set; }

        internal override ParameterCategory ParameterCategory => ParameterCategory.GenericList;

        internal override string ToXml => this.BuildXml();

        public override string ToString() => string.Format(CultureInfo.CurrentCulture, Strings.listParameterTypeNameFormat, Enum.GetName(typeof(ListType), this.ListType), this.GenericArgumentName, this.Name);

        internal override string Description => this._enumHelper.GetTypeDescription(ListType, GenericArgumentName);
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder);
            xmlTextWriter.WriteStartElement(XmlDataConstants.GENERICLISTPARAMETERELEMENT);
                xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                xmlTextWriter.WriteElementString(XmlDataConstants.GENERICARGUMENTNAMEELEMENT, this.GenericArgumentName);
                xmlTextWriter.WriteElementString(XmlDataConstants.LISTTYPEELEMENT, Enum.GetName(typeof(ListType), this.ListType));
                xmlTextWriter.WriteElementString(XmlDataConstants.CONTROLELEMENT, Enum.GetName(typeof(ListParameterInputStyle), this.Control));
                xmlTextWriter.WriteElementString(XmlDataConstants.OPTIONALELEMENT, this.IsOptional.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                xmlTextWriter.WriteElementString(XmlDataConstants.COMMENTSELEMENT, this.Comments);
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}
