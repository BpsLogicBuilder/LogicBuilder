using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Commands;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor.Factories
{
    internal class EditConstructorCommandFactory : IEditConstructorCommandFactory
    {
        public EditConstructorFormXmlCommand GetEditFormXmlCommand(IEditConstructorForm editConstructorForm)
            => new
            (
                Program.ServiceProvider.GetRequiredService<IConstructorDataParser>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                editConstructorForm
            );

        public SelectConstructorCommand GetSelectConstructorCommand(IEditConstructorForm editConstructorForm)
            => new
            (
                editConstructorForm
            );
    }
}
