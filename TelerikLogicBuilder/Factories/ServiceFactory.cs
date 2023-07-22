using ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete.Factories;
using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.GenericArguments;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Services;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal class ServiceFactory : IServiceFactory
    {
        public IApplicationDropDownList GetApplicationDropDownList(IApplicationHostControl applicationHostControl)
            => new ApplicationDropDownList
            (
                Program.ServiceProvider.GetRequiredService<IApplicationTypeInfoManager>(),
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                applicationHostControl
            );

        public IConnectorObjectTypeAutoCompleteManager GetConnectorObjectTypeAutoCompleteManager(IApplicationHostControl applicationHostControl, ITypeAutoCompleteTextControl textControl)
            => new ConnectorObjectTypeAutoCompleteManager
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ITypeAutoCompleteCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                applicationHostControl,
                textControl
            );

        public IProgressForm GetProgressForm(Progress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => new ProgressForm
            (
                Program.ServiceProvider.GetRequiredService<IFormInitializer>(),
                progress,
                cancellationTokenSource
            );

        public ISplashScreen GetSplashScreen()
        {
            return new SplashScreen
            (
                Program.ServiceProvider.GetRequiredService<IFormInitializer>()
            );
        }

        public ITreeViewXmlDocumentHelper GetTreeViewXmlDocumentHelper(SchemaName schema)
            => new TreeViewXmlDocumentHelper
            (
                Program.ServiceProvider.GetRequiredService<IXmlValidatorFactory>(),
                schema
            );

        public ITypeAutoCompleteManager GetTypeAutoCompleteManager(IApplicationHostControl applicationHostControl, ITypeAutoCompleteTextControl textControl)
            => new TypeAutoCompleteManager
            (
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<ITypeAutoCompleteCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                applicationHostControl,
                textControl
            );

        public IUpdateGenericArguments GetUpdateGenericArguments(IApplicationHostControl applicationHostControl)
            => new UpdateGenericArguments
            (
                Program.ServiceProvider.GetRequiredService<IExceptionHelper>(),
                Program.ServiceProvider.GetRequiredService<IGenericConfigManager>(),
                Program.ServiceProvider.GetRequiredService<ITypeHelper>(),
                Program.ServiceProvider.GetRequiredService<ITypeLoadHelper>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                applicationHostControl
            );
    }
}
