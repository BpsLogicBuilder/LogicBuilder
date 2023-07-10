using ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDialogFunction.Factories
{
    internal class EditDialogFunctionCommandFactory : IEditDialogFunctionCommandFactory
    {
        public EditDialogFunctionFormXmlCommand GetEditDialogFunctionFormXmlCommand(IEditDialogFunctionForm editDialogFunctionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editDialogFunctionForm
            );

        public SelectDialogFunctionCommand GetSelectDialogFunctionCommand(IEditDialogFunctionForm editDialogFunctionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                editDialogFunctionForm
            );
    }
}
