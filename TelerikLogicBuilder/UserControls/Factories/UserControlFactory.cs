using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.RichTextBoxPanelHelpers.Factories;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Factories
{
    internal class UserControlFactory : IUserControlFactory
    {
        public EditXmlRichTextBoxPanel GetEditXmlRichTextBoxPanel()
        {
            return new EditXmlRichTextBoxPanel
            (
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<IRichTextBoxPanelCommandFactory>()
            );
        }

        public IRichTextBoxPanel GetRichTextBoxPanel()
        {
            return new RichTextBoxPanel
            (
                Program.ServiceProvider.GetRequiredService<IImageListService>(),
                Program.ServiceProvider.GetRequiredService<IRichTextBoxPanelCommandFactory>()
            );
        }
    }
}
