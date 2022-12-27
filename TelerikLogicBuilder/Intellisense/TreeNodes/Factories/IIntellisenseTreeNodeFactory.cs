using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Reflection;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories
{
    internal interface IIntellisenseTreeNodeFactory
    {
        ArrayIndexerTreeNode GetArrayIndexerTreeNode(MethodInfo getMethodInfo,
            int rank,
            IVariableTreeNode? parentNode,
            IApplicationForm applicationForm,
            CustomVariableConfiguration? customVariableConfiguration = null);

        FieldTreeNode GetFieldTreeNode(FieldInfo fInfo,
            IVariableTreeNode? parentNode,
            IApplicationForm applicationForm,
            CustomVariableConfiguration? customVariableConfiguration = null);

        FunctionTreeNode GetFunctionTreeNode(MethodInfo mInfo,
            IVariableTreeNode? parentNode);

        IndexerTreeNode GetIndexerTreeNode(PropertyInfo pInfo,
            IVariableTreeNode? parentNode,
            Type indexType,
            IApplicationForm applicationForm,
            CustomVariableConfiguration? customVariableConfiguration = null);

        PropertyTreeNode GetPropertyTreeNode(PropertyInfo pInfo,
            IVariableTreeNode? parentNode,
            IApplicationForm applicationForm,
            CustomVariableConfiguration? customVariableConfiguration = null);
    }
}
