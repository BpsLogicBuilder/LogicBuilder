using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Globalization;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments
{
    internal class ObjectGenericConfig : GenericConfigBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ObjectGenericConfig(
            IXmlDocumentHelpers xmlDocumentHelpers,
            string genericArgumentName,
            string objectType,
            bool useForEquality,
            bool useForHashCode,
            bool useForToString)
            : base(genericArgumentName)
        {
            this.ObjectType = objectType;
            this.UseForEquality = useForEquality;
            this.UseForHashCode = useForHashCode;
            this.UseForToString = useForToString;
            this._xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        internal string ObjectType { get; private set; }
        internal bool UseForEquality { get; private set; }
        internal bool UseForHashCode { get; private set; }
        internal bool UseForToString { get; private set; }

        internal override GenericConfigCategory GenericConfigCategory => GenericConfigCategory.Object;
        internal override string ToXml => this.BuildXml();
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTPARAMETERELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE, this.GenericArgumentName);
                    xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, this.ObjectType);
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEFOREQUALITYELEMENT, this.UseForEquality.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEFORHASHCODEELEMENT, this.UseForHashCode.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                    xmlTextWriter.WriteElementString(XmlDataConstants.USEFORTOSTRINGELEMENT, this.UseForToString.ToString(CultureInfo.InvariantCulture).ToLowerInvariant());
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush(); 
            }

            return stringBuilder.ToString();
        }

        public override int GetHashCode() => GenericArgumentName.GetHashCode();
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;

            return IsEqualTo(obj as ObjectGenericConfig);
            bool IsEqualTo(ObjectGenericConfig? config)
            {
                if (config == null) return false;
                return this.GenericArgumentName == config.GenericArgumentName
                    && this.ObjectType == config.ObjectType;
            }
        }
        #endregion Methods
    }
}
