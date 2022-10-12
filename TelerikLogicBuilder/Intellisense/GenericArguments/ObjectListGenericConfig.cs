using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments
{
    internal class ObjectListGenericConfig : GenericConfigBase
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal ObjectListGenericConfig(
            IXmlDocumentHelpers xmlDocumentHelpers,
            string genericArgumentName,
            string objectType,
            ListType listType,
            ListParameterInputStyle control)
            : base(genericArgumentName)
        {
            this.ObjectType = objectType;
            this.ListType = listType;
            this.Control = control;
            this._xmlDocumentHelpers = xmlDocumentHelpers;
        }

        #region Properties
        internal string ObjectType { get; private set; }
        internal ListType ListType { get; private set; }
        internal ListParameterInputStyle Control { get; private set; }

        internal override GenericConfigCategory GenericConfigCategory => GenericConfigCategory.ObjectList;
        internal override string ToXml => this.BuildXml();
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.OBJECTLISTPARAMETERELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.GENERICARGUMENTNAMEATTRIBUTE, this.GenericArgumentName);
                    xmlTextWriter.WriteElementString(XmlDataConstants.OBJECTTYPEELEMENT, this.ObjectType);
                    xmlTextWriter.WriteElementString(XmlDataConstants.LISTTYPEELEMENT, Enum.GetName(typeof(ListType), this.ListType));
                    xmlTextWriter.WriteElementString(XmlDataConstants.CONTROLELEMENT, Enum.GetName(typeof(ListParameterInputStyle), this.Control));
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

            return IsEqualTo(obj as ObjectListGenericConfig);
            bool IsEqualTo(ObjectListGenericConfig? config)
            {
                if (config == null) return false;
                return this.GenericArgumentName == config.GenericArgumentName
                    && this.ObjectType == config.ObjectType
                    && this.ListType == config.ListType;
            }
        }
        #endregion Methods
    }
}
