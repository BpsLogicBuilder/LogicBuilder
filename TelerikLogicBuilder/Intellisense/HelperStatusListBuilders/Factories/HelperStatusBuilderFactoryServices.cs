using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.HelperStatusListBuilders.Factories;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.TreeNodes.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Parameters;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class HelperStatusBuilderFactoryServices
    {
        internal static IServiceCollection AddHelperStatusBuilderFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureConstructorsForm, IConstructorHelperStatusBuilder>>
                (
                    provider =>
                    configureConstructorsForm => new ConstructorHelperStatusBuilder
                    (
                        provider.GetRequiredService<IConstructorNodeBuilder>(),
                        provider.GetRequiredService<IParametersXmlParser>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, IFunctionHelperStatusBuilder>>
                (
                    provider =>
                    configureFunctionsForm => new FunctionHelperStatusBuilder
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionNodeBuilder>(),
                        provider.GetRequiredService<IFunctionXmlParser>(),
                        provider.GetRequiredService<IHelperStatusBuilderFactory>(),
                        provider.GetRequiredService<IReferenceInfoListBuilder>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<IHelperStatusBuilderFactory, HelperStatusBuilderFactory>()
                .AddTransient<Func<IConfigurationForm, IReferenceNodeListBuilder>>
                (
                    provider =>
                    configurationForm => new ReferenceNodeListBuilder
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<IIntellisenseTreeNodeFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configurationForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, IVariableHelperStatusBuilder>>
                (
                    provider =>
                    configureVariablesForm => new VariableHelperStatusBuilder
                    (
                        provider.GetRequiredService<IEnumHelper>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IHelperStatusBuilderFactory>(),
                        provider.GetRequiredService<IReferenceInfoListBuilder>(),
                        provider.GetRequiredService<IStringHelper>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigurationForm, IVariableNodeBuilder>>
                (
                    provider =>
                    configurationForm => new VariableNodeBuilder
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IIntellisenseHelper>(),
                        provider.GetRequiredService<IIntellisenseTreeNodeFactory>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        configurationForm
                    )
                );
        }
    }
}
