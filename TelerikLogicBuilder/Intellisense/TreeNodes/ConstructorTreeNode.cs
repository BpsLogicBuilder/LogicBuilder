using ABIS.LogicBuilder.FlowBuilder.Constants;
using System.Linq;
using System.Reflection;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes
{
    internal class ConstructorTreeNode : RadTreeNode
    {
        internal ConstructorTreeNode(ConstructorInfo cInfo)
            : base(cInfo.DeclaringType?.Name ?? string.Empty)
        {
            ImageIndex = ImageIndexes.CONSTRUCTORIMAGEINDEX;
            this.Parameters = cInfo.GetParameters();
            this.ToolTipText = ParametersList;
            this.CInfo = cInfo;
        }

        public ConstructorInfo CInfo { get; }
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

            ConstructorTreeNode item = (ConstructorTreeNode)obj;
            return this.Name == item.Name
                && this.ParametersList == item.ParametersList;
        }

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }
        #endregion Methods
    }
}
