using ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditValueFunction.Factories
{
    internal class EditValueFunctionCommandFactory : IEditValueFunctionCommandFactory
    {
        public EditValueFunctionFormXmlCommand GetEditValueFunctionFormXmlCommand(IEditValueFunctionForm editFunctionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editFunctionForm
            );

        public SelectValueFunctionCommand GetSelectValueFunctionCommand(IEditValueFunctionForm editFunctionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                editFunctionForm
            );
    }
}
