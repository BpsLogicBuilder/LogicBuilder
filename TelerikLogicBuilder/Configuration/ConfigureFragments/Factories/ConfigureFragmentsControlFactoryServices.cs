﻿using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragment;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.ConfigureFragmentsFolder;
using ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ConfigureFragmentsControlFactoryServices
    {
        internal static IServiceCollection AddConfigureFragmentsControlFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IConfigureFragmentsForm, IConfigureFragmentControl>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<RichTextBoxPanel>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFragmentsForm
                    )
                )
                .AddTransient<IConfigureFragmentsControlFactory, ConfigureFragmentsControlFactory>()
                .AddTransient<Func<IConfigureFragmentsForm, IConfigureFragmentsFolderControl>>
                (
                    provider =>
                    configureFragmentsForm => new ConfigureFragmentsFolderControl
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<ITreeViewService>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        configureFragmentsForm
                    )
                );
        }
    }
}