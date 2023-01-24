using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureConstructors;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFunctions;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureGenericArguments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureProjectProperties;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureVariables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.StateImageSetters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.Services.XmlTreeViewSynchronizers;
using ABIS.LogicBuilder.FlowBuilder.XmlTreeViewSynchronizers.Factories;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class XmlTreeViewSynchronizerFactoryServices
    {
        internal static IServiceCollection AddXmlTreeViewSynchronizerFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigurationForm, IComparer<RadTreeNode>, IConfigurationFormXmlTreeViewSynchronizer>>
                (
                    provider =>
                    (configurationForm, treeNodeComparer) => new ConfigurationFormXmlTreeViewSynchronizer
                    (
                        provider.GetRequiredService<IConfigurationFolderStateImageSetter>(),
                        provider.GetRequiredService<IConfigureConstructorsStateImageSetter>(),
                        provider.GetRequiredService<IConfigureFunctionsStateImageSetter>(),
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configurationForm,
                        treeNodeComparer
                    )
                )
                .AddTransient<Func<IConfigureConstructorsForm, IConfigureConstructorsXmlTreeViewSynchronizer>>
                (
                    provider =>
                    configureConstructorsForm => new ConfigureConstructorsXmlTreeViewSynchronizer
                    (
                        provider.GetRequiredService<IConfigureConstructorsStateImageSetter>(),
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IConstructorsFormTreeNodeComparer>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureConstructorsForm
                    )
                )
                .AddTransient<Func<IConfigureFragmentsForm, IConfigureFragmentsXmlTreeViewSynchronizer>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsXmlTreeViewSynchronizer
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFragmentsFormTreeNodeComparer>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFragmentsForm
                    )
                )
                .AddTransient<Func<IConfigureFunctionsForm, IConfigureFunctionsXmlTreeViewSynchronizer>>
                (
                    provider =>
                    configureFunctionsForm => new ConfigureFunctionsXmlTreeViewSynchronizer
                    (
                        provider.GetRequiredService<IConfigureFunctionsStateImageSetter>(),
                        provider.GetRequiredService<IConfigureParametersStateImageSetter>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IFunctionsFormTreeNodeComparer>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureFunctionsForm
                    )
                )
                .AddTransient<Func<IConfigureGenericArgumentsForm, IConfigureGenericArgumentsXmlTreeViewSynchronizer>>
                (
                    provider =>
                    configureGenericArgumentsForm => new ConfigureGenericArgumentsXmlTreeViewSynchronizer
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureGenericArgumentsForm
                    )
                )
                .AddTransient<Func<IConfigureVariablesForm, IConfigureVariablesXmlTreeViewSynchronizer>>
                (
                    provider =>
                    configureVariablesForm => new ConfigureVariablesXmlTreeViewSynchronizer
                    (
                        provider.GetRequiredService<IConfigureVariablesStateImageSetter>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IVariablesFormTreeNodeComparer>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        provider.GetRequiredService<IXmlTreeViewSynchronizerFactory>(),
                        configureVariablesForm
                    )
                )
                .AddTransient<Func<IConfigureProjectPropertiesForm, IProjectPropertiesXmlTreeViewSynchronizer>>
                (
                    provider =>
                    configureProjectProperties => new ProjectPropertiesXmlTreeViewSynchronizer
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureProjectProperties
                    )
                )
                .AddSingleton<IXmlTreeViewSynchronizerFactory, XmlTreeViewSynchronizerFactory>();
        }
    }
}
