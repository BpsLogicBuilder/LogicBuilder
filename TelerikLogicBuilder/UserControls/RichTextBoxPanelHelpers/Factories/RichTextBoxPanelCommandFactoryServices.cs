using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Commands;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Factories;
using System;
using System.Windows.Forms;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class RichTextBoxPanelCommandFactoryServices
    {
        internal static IServiceCollection AddRichTextBoxPanelCommandFactories(this IServiceCollection services)
        {
            return services
                .AddTransient<Func<RichTextBox, RichTextBoxCopySelectedTextCommand>>
                (
                    provider =>
                    richTextBox => new RichTextBoxCopySelectedTextCommand
                    (
                        richTextBox
                    )
                )
                .AddTransient<Func<RichTextBox, RichTextBoxCutSelectedTextCommand>>
                (
                    provider =>
                    richTextBox => new RichTextBoxCutSelectedTextCommand
                    (
                        richTextBox
                    )
                )
                .AddTransient<IRichTextBoxPanelCommandFactory, RichTextBoxPanelCommandFactory>()
                .AddTransient<Func<RichTextBox, RichTextBoxPasteTextCommand>>
                (
                    provider =>
                    richTextBox => new RichTextBoxPasteTextCommand
                    (
                        richTextBox
                    )
                )
                .AddTransient<Func<RichTextBox, RichTextBoxSelectAllCommand>>
                (
                    provider =>
                    richTextBox => new RichTextBoxSelectAllCommand
                    (
                        richTextBox
                    )
                )
                .AddTransient<Func<RichTextBox, SelectFragmentCommand>>
                (
                    provider =>
                    richTextBox => new SelectFragmentCommand
                    (
                        provider.GetRequiredService<IConfigurationService>(),
                        provider.GetRequiredService<IExceptionHelper>(),
                        richTextBox
                    )
                );
        }
    }
}
