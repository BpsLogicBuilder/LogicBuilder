using System;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.SelectFragment
{
    internal interface ISelectFragmentViewControl
    {
        event EventHandler? Changed;
        string FragmentName { get; }
        bool ItemSelected { get; }
        void SelectFragment(string fragmentName);
    }
}
