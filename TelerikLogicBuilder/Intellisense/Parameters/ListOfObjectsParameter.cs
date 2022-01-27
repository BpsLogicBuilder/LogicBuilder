using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ListOfObjectsParameter : ParameterBase, IObjectParameter
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IStringHelper _stringHelper;

        internal ListOfObjectsParameter(string name,
            bool isOptional,
            string comments,
            string objectType,
            ListType listType,
            ListParameterInputStyle control,
            IContextProvider contextProvider)
            : base(name, isOptional, comments)
        {
            this.ObjectType = objectType;
            this.ListType = listType;
            this.Control = control;
            this._enumHelper = contextProvider.EnumHelper;
            this._stringHelper = contextProvider.StringHelper;
            this._xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        internal string ObjectType { get; private set; }
        internal ListType ListType { get; private set; }
        internal ListParameterInputStyle Control { get; private set; }

        internal override ParameterCategory ParameterCategory => ParameterCategory.ObjectList;

        internal override string ToXml => this.BuildXml();

        public override string ToString() => string.Format
        (
            CultureInfo.CurrentCulture, 
            Strings.listParameterTypeNameFormat, 
            Enum.GetName(typeof(ListType), this.ListType), 
            this._stringHelper.ToShortName(this.ObjectType), 
            this.Name
        );

        internal override string Description => this._enumHelper.GetTypeDescription(ListType, ObjectType);
        #endregion Properties

        #region IConstructorParameter
        string IObjectParameter.Name => this.Name;

        bool IObjectParameter.IsOptional => this.IsOptional;

        string IObjectParameter.Comments => this.Comments;

        string IObjectParameter.ObjectType => this.ObjectType;
        #endregion IConstructorParameter

        #region Methods
        string BuildXml()
        {
            StringBuilder stringBuilder = new();
            XmlWriter xmlTextWriter = this._xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder);
            xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTLISTPARAMETERELEMENT);
                xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, this.ObjectType);
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
