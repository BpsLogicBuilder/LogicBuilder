using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal class FieldTreeNode : VariableTreeNode
    {
        private readonly IEnumHelper _enumHelper;

        public FieldTreeNode(
            IEnumHelper enumHelper,
            ITypeLoadHelper typeLoadHelper,
            FieldInfo fInfo,
            IVariableTreeNode? parentNode,
            IApplicationForm applicationForm,
            CustomVariableConfiguration? customVariableConfiguration = null)
            : base(typeLoadHelper, fInfo.Name, parentNode, fInfo.FieldType, applicationForm, customVariableConfiguration)
        {
            _enumHelper = enumHelper;
            ImageIndex = ImageIndexes.FIELDIMAGEINDEX;
            MemberInfo = fInfo;
        }

        public override MemberInfo MemberInfo { get; }

        public override string ReferenceDefinition
            => ParentNode == null
                ? _enumHelper.GetVisibleEnumText(ValidIndirectReference.Field)
                : $"{ParentNode.ReferenceDefinition}{MiscellaneousConstants.PERIODSTRING}{_enumHelper.GetVisibleEnumText(ValidIndirectReference.Field)}";

        public override VariableCategory VariableCategory => VariableCategory.Field;
    }
}
