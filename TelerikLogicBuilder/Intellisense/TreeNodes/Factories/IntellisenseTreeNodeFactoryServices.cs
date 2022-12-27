using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class IntellisenseTreeNodeFactoryServices
    {
        internal static IServiceCollection AddIntellisenseTreeNodeFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<MethodInfo, int, IVariableTreeNode?, IApplicationForm, CustomVariableConfiguration?, ArrayIndexerTreeNode>>
                (
                    provider =>
                    (getMethodInfo, rank, parentNode, applicationForm, customVariableConfiguration) => new ArrayIndexerTreeNode
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        getMethodInfo,
                        rank,
                        parentNode,
                        applicationForm,
                        customVariableConfiguration
                    )
                )
                .AddTransient<Func<FieldInfo, IVariableTreeNode?, IApplicationForm, CustomVariableConfiguration?, FieldTreeNode>>
                (
                    provider =>
                    (fInfo, parentNode, applicationForm, customVariableConfiguration) => new FieldTreeNode
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        fInfo,
                        parentNode,
                        applicationForm,
                        customVariableConfiguration
                    )
                )
                .AddTransient<Func<MethodInfo, IVariableTreeNode?, FunctionTreeNode>>
                (
                    provider =>
                    (mInfo, parentNode) => new FunctionTreeNode
                    (
                        mInfo,
                        parentNode
                    )
                )
                .AddTransient<Func<PropertyInfo, IVariableTreeNode?, Type, IApplicationForm, CustomVariableConfiguration?, IndexerTreeNode>>
                (
                    provider =>
                    (pInfo, parentNode, indexType, applicationForm, customVariableConfiguration) => new IndexerTreeNode
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        pInfo,
                        parentNode,
                        indexType,
                        applicationForm,
                        customVariableConfiguration
                    )
                )
                .AddTransient<IIntellisenseTreeNodeFactory, IntellisenseTreeNodeFactory>()
                .AddTransient<Func<PropertyInfo, IVariableTreeNode?, IApplicationForm, CustomVariableConfiguration?, PropertyTreeNode>>
                (
                    provider =>
                    (pInfo, parentNode, applicationForm, customVariableConfiguration) => new PropertyTreeNode
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        pInfo,
                        parentNode,
                        applicationForm,
                        customVariableConfiguration
                    )
                );
        }
    }
}
