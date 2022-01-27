using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters
{
    internal class ObjectParameter : ParameterBase, IObjectParameter, IComparableParameter
    {
        private readonly IStringHelper _stringHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ObjectParameter(string name,
            bool isOptional,
            string comments,
            string objectType,
            bool useForEquality,
            bool useForHashCode,
            bool useForToString,
            IContextProvider contextProvider)
            : base(name, isOptional, comments)
        {
            this.ObjectType = objectType;
            this.UseForEquality = useForEquality;
            this.UseForHashCode = useForHashCode;
            this.UseForToString = useForToString;
            this._stringHelper = contextProvider.StringHelper;
            this._xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        internal string ObjectType { get; private set; }
        internal bool UseForEquality { get; private set; }
        internal bool UseForHashCode { get; private set; }
        internal bool UseForToString { get; private set; }

        internal override ParameterCategory ParameterCategory => ParameterCategory.Object;

        internal override string ToXml => this.BuildXml();

        public override string ToString() => string.Format
        (
            Strings.parameterTypeNameFormat2, 
            this._stringHelper.ToShortName(this.ObjectType), 
            this.Name
        );

        internal override string Description => ObjectType;
        #endregion Properties

        #region IConstructorParameter
        string IObjectParameter.Name => this.Name;

        bool IObjectParameter.IsOptional => this.IsOptional;

        string IObjectParameter.Comments => this.Comments;

        string IObjectParameter.ObjectType => this.ObjectType;
        #endregion IConstructorParameter

        #region IComparableParameter
        string IComparableParameter.Name => this.Name;

        bool IComparableParameter.UseForEquality => this.UseForEquality;

        bool IComparableParameter.UseForHashCode => this.UseForHashCode;

        bool IComparableParameter.UseForToString => this.UseForToString;

        ParameterCategory IComparableParameter.ParameterCategory => this.ParameterCategory;
        #endregion IComparableParameter

        #region Methods
        string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTPARAMETERELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                    xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, this.ObjectType);
                    xmlTextWriter.WriteElementString(XmlDataConstants.OPTIONALELEMENT, this.IsOptional.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEFOREQUALITYELEMENT, this.UseForEquality.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEFORHASHCODEELEMENT, this.UseForHashCode.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEFORTOSTRINGELEMENT, this.UseForToString.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    xmlTextWriter.WriteElementString(XmlDataConstants.COMMENTSELEMENT, this.Comments);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }
            return stringBuilder.ToString();
        }
        #endregion Methods
    }
}
