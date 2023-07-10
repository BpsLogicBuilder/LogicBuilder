using ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditBooleanFunction.Factories
{
    internal class EditBooleanFunctionCommandFactory : IEditBooleanFunctionCommandFactory
    {
        public EditBooleanFunctionFormXmlCommand GetEditBooleanFunctionFormXmlCommand(IEditBooleanFunctionForm editFunctionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IFunctionDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editFunctionForm
            );

        public SelectBooleanFunctionCommand GetSelectBooleanFunctionCommand(IEditBooleanFunctionForm editFunctionForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                editFunctionForm
            );
    }
}
