using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables
{
    internal abstract class VariableBase
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IStringHelper _stringHelper;

        internal VariableBase(string name, string memberName, VariableCategory variableCategory, string castVariableAs, string typeName, string referenceName, string referenceDefinition, string castReferenceAs, ReferenceCategories referenceCategory, string comments, IContextProvider contextProvider)
        {
            this.Name = name;
            this.MemberName = memberName;
            this.VariableCategory = variableCategory;
            this.CastVariableAs = castVariableAs;
            this.TypeName = typeName;
            this.ReferenceNameString = referenceName;
            this.ReferenceDefinitionString = referenceDefinition;
            this.ReferenceCategory = referenceCategory;
            this.CastReferenceAs = castReferenceAs;
            this.Comments = comments;
            _enumHelper = contextProvider.EnumHelper;
            _stringHelper = contextProvider.StringHelper;
        }

        #region Properties
        /// <summary>
        /// Unique name for this Decision
        /// </summary>
        internal string Name { get; set; }

        /// <summary>
        /// VariableName for this Decision
        /// </summary>
        internal string MemberName { get; set; }

        /// <summary>
        /// Is the variable a field or property
        /// </summary>
        internal VariableCategory VariableCategory { get; private set; }

        /// <summary>
        /// Used when the variable must be cast to a different type. 
        /// </summary>
        internal string CastVariableAs { get; private set; }

        /// <summary>
        /// Used when the variable is a member of a Type or when the variable is an instance member of a static  reference
        /// </summary>
        internal string TypeName { get; private set; }

        /// <summary>
        /// Name of the reference variable when the ReferenceCategory is "Field" or "Property" or combination of field and property
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
        internal List<string> ReferenceName => _stringHelper.SplitWithQuoteQualifier(this.ReferenceNameString, MiscellaneousConstants.PERIODSTRING)
                    .ToList();

        /// <summary>
        /// Used when the variable is an instance field
        /// or property of an instance field or property e.g. i.e. Property.Field.Property etc.
        /// </summary>
        internal string ReferenceDefinitionString { get; private set; }

        /// <summary>
        /// Used when the variable is an instance field
        /// or property of an instance field or property e.g. i.e. Property.Field.Property etc. represented as a list of ValidIndirectReference
        /// </summary>
        internal List<ValidIndirectReference> ReferenceDefinition => this.ReferenceDefinitionString
                    //.BuildValidReferenceDefinition()
                    .Split(new char[] { MiscellaneousConstants.PERIOD }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => (ValidIndirectReference)Enum.Parse(typeof(ValidIndirectReference), s))
                    .ToList();

        /// <summary>
        /// Is the reference an instance of the current class, a type, an instance reference
        /// or a static reference.
        /// </summary>
        internal ReferenceCategories ReferenceCategory { get; private set; }

        /// <summary>
        /// Used when the variable is an instance field or property and the declared member of the reference must be cast to a different type. 
        /// </summary>
        internal string CastReferenceAs { get; private set; }

        /// <summary>
        /// Used when the variable is an instance field or property and the declared member of the reference must be cast to a different type. 
        /// </summary>
        internal List<string> CastReferenceAsList
            => CastReferenceAs.Trim().Length == 0
                ? ReferenceName.Select(r => MiscellaneousConstants.TILDE).ToList()
                : _stringHelper.SplitWithQuoteQualifier(CastReferenceAs.Trim(), MiscellaneousConstants.PERIODSTRING).ToList();

        /// <summary>
        /// Purpose for this variable
        /// </summary>
        internal string Comments { get; private set; }

        internal abstract VariableTypeCategory VariableTypeCategory { get; }

        internal abstract string ToXml { get; }

        internal abstract string ObjectTypeString { get; }
        #endregion Properties

        #region Methods
        protected void WriteBaseElementString(XmlWriter xmlTextWriter)
        {
            xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, this.Name);
            xmlTextWriter.WriteElementString(XmlDataConstants.MEMBERNAMEELEMENT, this.MemberName);
            xmlTextWriter.WriteElementString(XmlDataConstants.VARIABLECATEGORYELEMENT, Enum.GetName(typeof(VariableCategory), this.VariableCategory));
            xmlTextWriter.WriteElementString(XmlDataConstants.CASTVARIABLEASELEMENT, this.CastVariableAs);
            xmlTextWriter.WriteElementString(XmlDataConstants.TYPENAMEELEMENT, this.TypeName);
            xmlTextWriter.WriteElementString(XmlDataConstants.REFERENCENAMEELEMENT, this.ReferenceNameString);
            xmlTextWriter.WriteElementString(XmlDataConstants.REFERENCEDEFINITIONELEMENT, _enumHelper.BuildValidReferenceDefinition(this.ReferenceDefinitionString));
            xmlTextWriter.WriteElementString(XmlDataConstants.CASTREFERENCEASELEMENT, this.CastReferenceAs);
            xmlTextWriter.WriteElementString(XmlDataConstants.REFERENCECATEGORYELEMENT, Enum.GetName(typeof(ReferenceCategories), this.ReferenceCategory));
            xmlTextWriter.WriteElementString(XmlDataConstants.EVALUATIONELEMENT, Enum.GetName(typeof(DecisionsEvaluation), DecisionsEvaluation.Implemented));
            xmlTextWriter.WriteElementString(XmlDataConstants.COMMENTSELEMENT, this.Comments);
            xmlTextWriter.WriteElementString(XmlDataConstants.METADATAELEMENT, string.Empty);
        }
        #endregion Methods
    }
}
