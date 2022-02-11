using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions
{
    internal class Function
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IStringHelper _stringHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        internal Function(string name, string memberName, FunctionCategories functionCategory, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, ParametersLayout parametersLayout, List<ParameterBase> parameters, List<string> genericArguments, ReturnTypeBase returnType, string summary, IContextProvider contextProvider)
        {
            this.Name = name;
            this.MemberName = memberName;
            this.FunctionCategory = functionCategory;
            this.TypeName = typeName;
            this.ReferenceNameString = referenceName;
            this.ReferenceDefinitionString = referenceDefinition;
            this.CastReferenceAs = castReferenceAs;
            this.ReferenceCategory = referenceCategory;
            this.ParametersLayout = parametersLayout;
            this.Parameters = parameters;
            this.GenericArguments = genericArguments;
            this.ReturnType = returnType;
            this.Summary = summary;

            _enumHelper = contextProvider.EnumHelper;
            _stringHelper = contextProvider.StringHelper;
            _xmlDocumentHelpers = contextProvider.XmlDocumentHelpers;
        }

        #region Properties
        /// <summary>
        /// Unique name for this function
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        /// CodeName for this function
        /// </summary>
        internal string MemberName { get; private set; }

        /// <summary>
        /// Category for this function
        /// </summary>
        internal FunctionCategories FunctionCategory { get; private set; }

        /// <summary>
        /// Used when the function is a member of a Type or when the function is an instance member of a static  reference
        /// </summary>
        internal string TypeName { get; private set; }

        /// <summary>
        /// Name of the reference function when the ReferenceCategory is "Field" or "Property" or combination of field and property
        /// i.e. foo.bar.boz etc.
        /// Full Qualified Class Name when the ReferenceCategory is "Type"
        /// This field is ignored and should be an empty string when the ReferenceCategory is "This"
        /// </summary>
        internal string ReferenceNameString { get; private set; }

        /// <summary>
        /// Name of the reference variable when the ReferenceCategory is "Field" or "Property" or combination of field and property
        /// i.e. foo.bar.boz etc. represented as a list of strings
        /// Full Qualified Class Name when the ReferenceCategory is "Type"
        /// This field is ignored and should be an empty string when the ReferenceCategory is "This"
        /// </summary>
        internal List<string> ReferenceName => _stringHelper
                    .SplitWithQuoteQualifier(this.ReferenceNameString, MiscellaneousConstants.PERIODSTRING)
                    .ToList();

        /// <summary>
        /// Used when the function is an member of an instance field or property e.g. i.e. Property.Field.Property etc.
        /// </summary>
        internal string ReferenceDefinitionString { get; private set; }

        /// <summary>
        /// Used when the function is an instance member of an instance field or property e.g. i.e. Property.Field.Property etc. represented as a list of ValidIndirectReference
        /// </summary>
        internal List<ValidIndirectReference> ReferenceDefinition => this.ReferenceDefinitionString
                    //.BuildValidReferenceDefinition()
                    .Split(new char[] { MiscellaneousConstants.PERIOD }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => (ValidIndirectReference)Enum.Parse(typeof(ValidIndirectReference), s))
                    .ToList();

        /// <summary>
        /// Used when the function is an instance member and the declared member of the reference must be cast to a different type. 
        /// </summary>
        internal string CastReferenceAs { get; private set; }

        /// <summary>
        /// Used when the function is an instance member and the declared member of the reference must be cast to a different type. 
        /// </summary>
        internal List<string> CastReferenceAsList
            => CastReferenceAs.Trim().Length == 0
                ? ReferenceName.Select(r => MiscellaneousConstants.TILDE).ToList()
                : _stringHelper.SplitWithQuoteQualifier(CastReferenceAs.Trim(), MiscellaneousConstants.PERIODSTRING).ToList();

        /// <summary>
        /// Is the reference an instance of the current class, a type, an instance reference
        /// or a static reference.
        /// </summary>
        internal ReferenceCategories ReferenceCategory { get; private set; }

        /// <summary>
        /// Defines how the function is presented to the user on the data input forms
        /// </summary>
        internal ParametersLayout ParametersLayout { get; private set; }

        /// <summary>
        /// Parameters for this function
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
        /// Return type for this function
        /// </summary>
        internal ReturnTypeBase ReturnType { get; private set; }

        /// <summary>
        /// Comments about this function
        /// </summary>
        internal string Summary { get; private set; }

        internal string ToXml => this.BuildXml();
        #endregion Properties

        #region Methods
        private string BuildXml()
        {
            StringBuilder stringBuilder = new();
            using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
            {
                xmlTextWriter.WriteStartElement(XmlDataConstants.FUNCTIONELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
                    xmlTextWriter.WriteElementString(XmlDataConstants.MEMBERNAMEELEMENT, this.MemberName);
                    xmlTextWriter.WriteElementString(XmlDataConstants.FUNCTIONCATEGORYELEMENT, this.FunctionCategory == FunctionCategories.Unknown ? Enum.GetName(typeof(FunctionCategories), FunctionCategories.Standard) : Enum.GetName(typeof(FunctionCategories), this.FunctionCategory));
                    xmlTextWriter.WriteElementString(XmlDataConstants.TYPENAMEELEMENT, this.TypeName);
                    xmlTextWriter.WriteElementString(XmlDataConstants.REFERENCENAMEELEMENT, this.ReferenceNameString);
                    xmlTextWriter.WriteElementString(XmlDataConstants.REFERENCEDEFINITIONELEMENT, _enumHelper.BuildValidReferenceDefinition(this.ReferenceDefinitionString));
                    xmlTextWriter.WriteElementString(XmlDataConstants.CASTREFERENCEASELEMENT, this.CastReferenceAs);
                    xmlTextWriter.WriteElementString(XmlDataConstants.REFERENCECATEGORYELEMENT, Enum.GetName(typeof(ReferenceCategories), this.ReferenceCategory));
                    xmlTextWriter.WriteElementString(XmlDataConstants.PARAMETERSLAYOUTELEMENT, Enum.GetName(typeof(ParametersLayout), this.ParametersLayout));
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
                    xmlTextWriter.WriteStartElement(XmlDataConstants.RETURNTYPEELEMENT);
                        xmlTextWriter.WriteRaw(this.ReturnType.ToXml);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteElementString(XmlDataConstants.SUMMARYELEMENT, this.Summary);
                xmlTextWriter.WriteEndElement();
                xmlTextWriter.Flush();
            }

            return stringBuilder.ToString();
        }

        public override string ToString() 
            => string.Format
            (
                CultureInfo.CurrentCulture, Strings.functionToStringFormat,
                this.Name,
                this.TypeName,
                string.Join(", ", this.Parameters.Select(parameter => parameter.ToString()))
            );
        #endregion Methods
    }
}
