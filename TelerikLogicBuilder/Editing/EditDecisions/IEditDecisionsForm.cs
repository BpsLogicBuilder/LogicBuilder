using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.ListBox;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions
{
    internal interface IEditDecisionsForm : IShapeEditForm, IApplicationForm
    {
        HelperButtonTextBox TxtEditDecision { get; }
        IRadListBoxManager<IDecisionListBoxItem> RadListBoxManager { get; }
        void UpdateDecisionsList(string? xmlString);
    }
}
