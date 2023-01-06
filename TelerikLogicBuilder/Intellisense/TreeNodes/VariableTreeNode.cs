using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Globalization;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal abstract class VariableTreeNode : BaseTreeNode, IVariableTreeNode
    {
        private readonly IApplicationForm applicationForm;
        private readonly bool isCastNode;
        protected readonly ITypeLoadHelper _typeLoadHelper;

        protected VariableTreeNode(
            ITypeLoadHelper typeLoadHelper,
            string text,
            IVariableTreeNode? parentNode,
            Type memberType,
            IApplicationForm applicationForm,
            CustomVariableConfiguration? customVariableConfiguration = null) : base(text, parentNode)
        {
            _typeLoadHelper = typeLoadHelper;
            this.CustomVariableConfiguration = customVariableConfiguration;
            this.CastAs = customVariableConfiguration?.CastAs ?? MiscellaneousConstants.TILDE;
            isCastNode = this.CastAs != MiscellaneousConstants.TILDE;
            UnCastMemberType = memberType;
            this.applicationForm = applicationForm;
            ToolTipText = string.Format(CultureInfo.CurrentCulture, Strings.variableTypeFormat, MemberType.ToString());
        }

        public CustomVariableConfiguration? CustomVariableConfiguration { get; }

        protected bool HasCustomMemberName => !string.IsNullOrEmpty(CustomVariableConfiguration?.MemberName)
                                && CustomVariableConfiguration.MemberName != MiscellaneousConstants.TILDE;

        public abstract MemberInfo MemberInfo { get; }

        public abstract string ReferenceDefinition { get; }

        public abstract VariableCategory VariableCategory { get; }

        public string CastAs { get; set; }

        public string CastReferenceDefinition
        {
            get
            {
                string castToTypeName = isCastNode
                                ? $"{MiscellaneousConstants.DOUBLEQUOTE}{CastAs}{MiscellaneousConstants.DOUBLEQUOTE}"
                                : MiscellaneousConstants.TILDE;

                return ParentNode == null
                                ? castToTypeName
                                : $"{ParentNode.CastReferenceDefinition}{MiscellaneousConstants.PERIODSTRING}{castToTypeName}";
            }
        }

        public string CastVariableDefinition => isCastNode ? CastAs : string.Empty;

        public override string MemberText => HasCustomMemberName
                                                ? CustomVariableConfiguration!.MemberName
                                                : base.MemberText;

        public virtual string ReferenceName
            => ParentNode == null
                ? MemberText
                : $"{ParentNode.ReferenceName}{MiscellaneousConstants.PERIODSTRING}{MemberText}";

        public Type UnCastMemberType { get; }

        public override Type MemberType
            => isCastNode && _typeLoadHelper.TryGetSystemType(CastAs, applicationForm.Application, out Type? type)
                ? type
                : UnCastMemberType;

        #region Methods
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;

            VariableTreeNode item = (VariableTreeNode)obj;
            return ReferenceName == item.ReferenceName;
        }

        public override int GetHashCode()
        {
            return MemberText.GetHashCode();
        }
        #endregion Methods
    }
}
