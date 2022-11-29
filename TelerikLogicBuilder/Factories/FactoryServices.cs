using ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete.Factories;
using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using System;
using System.Threading;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class FactoryServices
    {
        internal static IServiceCollection AddFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<IApplicationForm, IApplicationDropDownList>>
                (
                    provider =>
                    applicationForm => new ApplicationDropDownList
                    (
                        provider.GetRequiredService<IApplicationTypeInfoManager>(),
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        applicationForm
                    )
                )
                .AddTransient<Func<Progress<ProgressMessage>, CancellationTokenSource, IProgressForm>>
                (
                    provider =>
                    (progress, cancellationTokenSource) => new ProgressForm
                    (
                        provider.GetRequiredService<IFormInitializer>(),
                        progress, 
                        cancellationTokenSource
                    )
                )
                .AddTransient<IServiceFactory, ServiceFactory>()
                .AddTransient<Func<SchemaName, ITreeViewXmlDocumentHelper>>
                (
                    provider =>
                    schema => new TreeViewXmlDocumentHelper
                    (
                        provider.GetRequiredService<IXmlValidatorFactory>(),
                        schema
                    )
                )
                .AddTransient<Func<IApplicationForm, ITypeAutoCompleteTextControl, ITypeAutoCompleteManager>>
                (
                    provider =>
                    (applicationForm, textControl) => new TypeAutoCompleteManager
                    (
                        provider.GetRequiredService<IImageListService>(),
                        provider.GetRequiredService<ITypeAutoCompleteCommandFactory>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        applicationForm,
                        textControl
                    )
                )
                .AddTransient<Func<IApplicationForm, IUpdateGenericArguments>>
                (
                    provider =>
                    applicationForm => new UpdateGenericArguments
                    (
                        provider.GetRequiredService<IExceptionHelper>(),
                        provider.GetRequiredService<IGenericConfigManager>(),
                        provider.GetRequiredService<ITypeHelper>(),
                        provider.GetRequiredService<ITypeLoadHelper>(),
                        provider.GetRequiredService<IXmlDocumentHelpers>(),
                        applicationForm
                    )
                );
        }
    }
}
