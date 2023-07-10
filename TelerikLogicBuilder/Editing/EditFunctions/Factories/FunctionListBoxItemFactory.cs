using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions.Factories
{
    internal class FunctionListBoxItemFactory : IFunctionListBoxItemFactory
    {
        public IFunctionListBoxItem GetFunctionListBoxItem(string visibleText, string hiddenText, Type assignedTo, IApplicationControl applicationControl)
            => new FunctionListBoxItem
            (
                Program.ServiceProvider.GetRequiredService<IAssertFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IConfigurationService>(),
                Program.ServiceProvider.GetRequiredService<IFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IFunctionHelper>(),
                Program.ServiceProvider.GetRequiredService<IRetractFunctionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                visibleText,
                hiddenText,
                assignedTo,
                applicationControl
            );
    }
}
