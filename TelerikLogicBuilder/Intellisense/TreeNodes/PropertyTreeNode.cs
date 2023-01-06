using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal class PropertyTreeNode : VariableTreeNode
    {
        private readonly IEnumHelper _enumHelper;

        public PropertyTreeNode(
            IEnumHelper enumHelper,
            ITypeLoadHelper typeLoadHelper,
            PropertyInfo pInfo,
            IVariableTreeNode? parentNode,
            IApplicationForm applicationForm,
            CustomVariableConfiguration? customVariableConfiguration = null)
            : base(typeLoadHelper, pInfo.Name, parentNode, pInfo.PropertyType, applicationForm, customVariableConfiguration)
        {
            _enumHelper = enumHelper;
            MemberInfo = pInfo;
            ImageIndex = ImageIndexes.PROPERTYIMAGEINDEX;
        }

        public override MemberInfo MemberInfo { get; }

        public override string ReferenceDefinition
            => ParentNode == null
                ? _enumHelper.GetVisibleEnumText(ValidIndirectReference.Property)
                : $"{ParentNode.ReferenceDefinition}{MiscellaneousConstants.PERIODSTRING}{_enumHelper.GetVisibleEnumText(ValidIndirectReference.Property)}";

        public override VariableCategory VariableCategory => VariableCategory.Property;
    }
}
