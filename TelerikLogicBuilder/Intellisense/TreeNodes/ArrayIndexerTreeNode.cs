﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal class ArrayIndexerTreeNode : VariableTreeNode
    {
        private readonly IEnumHelper _enumHelper;

        public ArrayIndexerTreeNode(
            IEnumHelper enumHelper,
            ITypeLoadHelper typeLoadHelper,
            MethodInfo getMethodInfo,
            int rank,
            IVariableTreeNode? parentNode,
            IApplicationForm applicationForm,
            CustomVariableConfiguration? customVariableConfiguration = null) 
            : base(typeLoadHelper, enumHelper.GetVisibleEnumText(VariableCategory.ArrayIndexer), parentNode, getMethodInfo.ReturnType, applicationForm, customVariableConfiguration)
        {
            _enumHelper = enumHelper;
            Rank = rank;
            MemberInfo = getMethodInfo;
            ImageIndex = ImageIndexes.INDEXERIMAGEINDEX;
        }

        private bool HasCustomVariableCategory => CustomVariableConfiguration?.VariableCategory == VariableCategory.VariableArrayIndexer;

        public int Rank { get; }

        public override MemberInfo MemberInfo { get; }

        public override string MemberText => HasCustomMemberName
                                                ? CustomVariableConfiguration!.MemberName
                                                : string.Join
                                                (
                                                    MiscellaneousConstants.COMMASTRING,
                                                    new string[Rank].Select(a => IntellisenseConstants.DEFAULTINDEX)
                                                );

        public override string ReferenceDefinition
        {
            get
            {
                return ParentNode == null
                        ? _enumHelper.GetVisibleEnumText(GetValidIndirectReference())
                        : $"{ParentNode.ReferenceDefinition}{MiscellaneousConstants.PERIODSTRING}{_enumHelper.GetVisibleEnumText(GetValidIndirectReference())}";

                ValidIndirectReference GetValidIndirectReference()
                    => HasCustomVariableCategory
                    ? ValidIndirectReference.VariableArrayIndexer
                    : ValidIndirectReference.ArrayIndexer;
            }
        }

        public override string ReferenceName => ParentNode == null
                        ? MemberText
                        : $"{ParentNode.ReferenceName}{MiscellaneousConstants.PERIODSTRING}{MemberText}";

        public override VariableCategory VariableCategory => HasCustomVariableCategory
                                                                ? CustomVariableConfiguration!.VariableCategory!.Value
                                                                : VariableCategory.ArrayIndexer;
    }
}
