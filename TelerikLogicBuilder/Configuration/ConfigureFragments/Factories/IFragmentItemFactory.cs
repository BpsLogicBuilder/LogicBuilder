namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories
{
    internal interface IFragmentItemFactory
    {
        Fragment GetFragment(string name, string xml);
    }
}
