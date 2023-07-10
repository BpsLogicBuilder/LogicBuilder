using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories
{
    internal class DialogFormMessageControlFactory : IDialogFormMessageControlFactory
    {
        public IDialogFormMessageControl GetDialogFormMessageControl()
        {
            return new DialogFormMessageControl
            (
                Program.ServiceProvider.GetRequiredService<IDialogFormMessageCommandFactory>(),
                Program.ServiceProvider.GetRequiredService<IImageListService>()
            );
        }
    }
}
