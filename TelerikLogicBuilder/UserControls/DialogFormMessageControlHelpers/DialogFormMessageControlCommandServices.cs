using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers;
using System;
using Telerik.WinControls.UI;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class DialogFormMessageControlCommandServices
    {
        internal static IServiceCollection AddDialogFormMessageControlCommands(this IServiceCollection services)
            => services.AddTransient<Func<RadLabel, CopyToClipboardCommand>>
                (
                    provider =>
                    radLabel => ActivatorUtilities.CreateInstance<CopyToClipboardCommand>
                    (
                        provider,
                        radLabel
                    )
                )
                .AddTransient<Func<RadLabel, OpenInTextViewerCommand>>
                (
                    provider =>
                    radLabel => ActivatorUtilities.CreateInstance<OpenInTextViewerCommand>
                    (
                        provider,
                        provider.GetRequiredService<IMainWindow>(),
                        radLabel
                    )
                );
    }
}
