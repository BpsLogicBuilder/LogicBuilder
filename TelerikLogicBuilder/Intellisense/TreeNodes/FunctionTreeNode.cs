using ABIS.LogicBuilder.FlowBuilder.Constants;
using System;
using System.Linq;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal class FunctionTreeNode : BaseTreeNode
    {
        public FunctionTreeNode(
            MethodInfo mInfo,
            IVariableTreeNode? parentNode) 
            : base(mInfo.Name, parentNode)
        {
            MInfo = mInfo;
            Parameters = MInfo.GetParameters();
            this.ToolTipText = ParametersList;
        }

        private string ReferenceName
            => this.ParentNode == null
                ? this.Text
                : $"{ParentNode.ReferenceName}{MiscellaneousConstants.PERIODSTRING}{this.Text}";

        public override Type MemberType => MInfo.ReturnType;

        public MethodInfo MInfo { get; }
        public ParameterInfo[] Parameters { get; }
        public string ParametersList
            => string.Format
            (
                Strings.parameterListFormat,
                string.Join
                (
                    Strings.itemsCommaSeparator,
                    Parameters.Select
                    (
                        p => string.Format
                        (
                            Strings.initialParameterTypeNameFormat, 
                            p.ParameterType.Name, 
                            p.Name
                        )
                    )
                )
            );

        #region Methods
        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (this.GetType() != obj.GetType())
                return false;

            FunctionTreeNode item = (FunctionTreeNode)obj;
            return this.ReferenceName == item.ReferenceName;
        }

        public override int GetHashCode()
        {
            return this.MemberText.GetHashCode();
        }
        #endregion Methods
    }
}
