using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories
{
    internal class IntellisenseTreeNodeFactory : IIntellisenseTreeNodeFactory
    {
        private readonly Func<MethodInfo, int, IVariableTreeNode?, IApplicationForm, CustomVariableConfiguration?, ArrayIndexerTreeNode> _getArrayIndexerTreeNode;
        private readonly Func<FieldInfo, IVariableTreeNode?, IApplicationForm, CustomVariableConfiguration?, FieldTreeNode> _getFieldTreeNode;
        private readonly Func<MethodInfo, IVariableTreeNode?, FunctionTreeNode> _getFunctionTreeNode;
        private readonly Func<PropertyInfo, IVariableTreeNode?, Type, IApplicationForm, CustomVariableConfiguration?, IndexerTreeNode> _getIndexerTreeNode;
        private readonly Func<PropertyInfo, IVariableTreeNode?, IApplicationForm, CustomVariableConfiguration?, PropertyTreeNode> _getPropertyTreeNode;

        public IntellisenseTreeNodeFactory(
            Func<MethodInfo, int, IVariableTreeNode?, IApplicationForm, CustomVariableConfiguration?, ArrayIndexerTreeNode> getArrayIndexerTreeNode,
            Func<FieldInfo, IVariableTreeNode?, IApplicationForm, CustomVariableConfiguration?, FieldTreeNode> getFieldTreeNode,
            Func<MethodInfo, IVariableTreeNode?, FunctionTreeNode> getFunctionTreeNode,
            Func<PropertyInfo, IVariableTreeNode?, Type, IApplicationForm, CustomVariableConfiguration?, IndexerTreeNode> getIndexerTreeNode,
            Func<PropertyInfo, IVariableTreeNode?, IApplicationForm, CustomVariableConfiguration?, PropertyTreeNode> getPropertyTreeNode)
        {
            _getArrayIndexerTreeNode = getArrayIndexerTreeNode;
            _getFieldTreeNode = getFieldTreeNode;
            _getFunctionTreeNode = getFunctionTreeNode;
            _getIndexerTreeNode = getIndexerTreeNode;
            _getPropertyTreeNode = getPropertyTreeNode;
        }

        public ArrayIndexerTreeNode GetArrayIndexerTreeNode(MethodInfo getMethodInfo, int rank, IVariableTreeNode? parentNode, IApplicationForm applicationForm, CustomVariableConfiguration? customVariableConfiguration = null)
            => _getArrayIndexerTreeNode(getMethodInfo, rank, parentNode, applicationForm, customVariableConfiguration);

        public FieldTreeNode GetFieldTreeNode(FieldInfo fInfo, IVariableTreeNode? parentNode, IApplicationForm applicationForm, CustomVariableConfiguration? customVariableConfiguration = null)
            => _getFieldTreeNode(fInfo, parentNode, applicationForm, customVariableConfiguration);

        public FunctionTreeNode GetFunctionTreeNode(MethodInfo mInfo, IVariableTreeNode? parentNode)
            => _getFunctionTreeNode(mInfo, parentNode);

        public IndexerTreeNode GetIndexerTreeNode(PropertyInfo pInfo, IVariableTreeNode? parentNode, Type indexType, IApplicationForm applicationForm, CustomVariableConfiguration? customVariableConfiguration = null)
            => _getIndexerTreeNode(pInfo, parentNode, indexType, applicationForm, customVariableConfiguration);

        public PropertyTreeNode GetPropertyTreeNode(PropertyInfo pInfo, IVariableTreeNode? parentNode, IApplicationForm applicationForm, CustomVariableConfiguration? customVariableConfiguration = null)
            => _getPropertyTreeNode(pInfo, parentNode, applicationForm, customVariableConfiguration);
    }
}
