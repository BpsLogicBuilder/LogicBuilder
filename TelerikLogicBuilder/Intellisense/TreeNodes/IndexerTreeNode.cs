using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal class IndexerTreeNode : VariableTreeNode
    {
        private readonly IEnumHelper _enumHelper;
        private readonly ITypeHelper _typeHelper;

        public IndexerTreeNode(
            IEnumHelper enumHelper,
            ITypeHelper typeHelper,
            ITypeLoadHelper typeLoadHelper,
            PropertyInfo pInfo,
            IVariableTreeNode? parentNode,
            Type indexType,
            IApplicationForm applicationForm,
            CustomVariableConfiguration? customVariableConfiguration = null)
            : base(typeLoadHelper, enumHelper.GetVisibleEnumText(enumHelper.GetIndexReferenceDefinition(indexType)), parentNode, pInfo.PropertyType, applicationForm, customVariableConfiguration)
        {
            _enumHelper = enumHelper;
            _typeHelper = typeHelper;
            ImageIndex = ImageIndexes.INDEXERIMAGEINDEX;
            MemberInfo = pInfo;
            this.IndexType = indexType;
        }

        private bool HasCustomVariableCategory => CustomVariableConfiguration?.VariableCategory == VariableCategory.VariableKeyIndexer;

        public Type IndexType { get; }

        public override MemberInfo MemberInfo { get; }

        public override string MemberText
        {
            get
            {
                if (HasCustomMemberName)
                    return CustomVariableConfiguration!.MemberName;

                if (VariableCategory == VariableCategory.StringKeyIndexer && _typeHelper.IsLiteralType(MemberType))
                {
                    return MemberInfo.ReflectedType != null 
                        ? $"{MemberInfo.ReflectedType.Name}{MiscellaneousConstants.UNDERSCORE}{MemberInfo.Name}"
                        : MemberInfo.Name;
                }

                return _typeHelper.GetIndexReferenceDefault(IndexType, MemberType);
            }
        }

        //Need path in addition to reference name to provide a consistent identifier that does not change with reference name or CastAs class name.
        public string Path
            => ParentNode == null
                ? IntellisenseConstants.INDEXREFERENCENAME
                : $"{ParentNode.ReferenceName}{MiscellaneousConstants.PERIODSTRING}{IntellisenseConstants.INDEXREFERENCENAME}";

        public override string ReferenceDefinition
        {
            get
            {
                return ParentNode == null
                        ? _enumHelper.GetVisibleEnumText(GetValidIndirectReference())
                        : $"{((VariableTreeNode)ParentNode).ReferenceDefinition}{MiscellaneousConstants.PERIODSTRING}{_enumHelper.GetVisibleEnumText(GetValidIndirectReference())}";

                ValidIndirectReference GetValidIndirectReference()
                    => HasCustomVariableCategory
                    ? _enumHelper.GetValidIndirectReference(VariableCategory.VariableKeyIndexer)
                    : _enumHelper.GetIndexReferenceDefinition(IndexType);
            }
        }

        public override string ReferenceName
        {
            get
            {
                string referenceName = MemberText;

                if (referenceName.Contains(MiscellaneousConstants.PERIODSTRING))
                    referenceName = $"{MiscellaneousConstants.DOUBLEQUOTE}{referenceName}{MiscellaneousConstants.DOUBLEQUOTE}";

                return ParentNode == null
                    ? referenceName
                    : $"{ParentNode.ReferenceName}{MiscellaneousConstants.PERIODSTRING}{referenceName}";
            }
        }

        public override VariableCategory VariableCategory => HasCustomVariableCategory 
                                                                ? CustomVariableConfiguration!.VariableCategory!.Value
                                                                : _enumHelper.GetIndexVariableCategory(IndexType);

        #region Methods
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;

            IndexerTreeNode item = (IndexerTreeNode)obj;
            return Path == item.Path;
        }

        public override int GetHashCode() => MemberText.GetHashCode();
        #endregion Methods
    }
}
