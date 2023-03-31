using ABIS.LogicBuilder.FlowBuilder.Structures;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    internal interface ISelectFragmentForm : IApplicationForm
    {
        string FragmentName { get; }
        void SetFragment(string fragmentName);
    }
}
