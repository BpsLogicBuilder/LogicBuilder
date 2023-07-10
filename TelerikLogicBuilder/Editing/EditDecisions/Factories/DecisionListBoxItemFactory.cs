using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions.Factories
{
    internal class DecisionListBoxItemFactory : IDecisionListBoxItemFactory
    {
        public IDecisionListBoxItem GetDecisionListBoxItem(string visibleText, string hiddenText, IApplicationControl applicationControl)
            => new DecisionListBoxItem
            (
                Program.ServiceProvider.GetRequiredService<IDecisionElementValidator>(),
                Program.ServiceProvider.GetRequiredService<IXmlDocumentHelpers>(),
                visibleText,
                hiddenText,
                applicationControl
            );
    }
}
