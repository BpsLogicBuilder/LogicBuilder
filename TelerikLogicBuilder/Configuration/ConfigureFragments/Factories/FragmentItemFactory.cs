﻿using System;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration.ConfigureFragments.Factories
{
    internal class FragmentItemFactory : IFragmentItemFactory
    {
        private readonly Func<string, string, Fragment> _getFragment;

        public FragmentItemFactory(
            Func<string, string, Fragment> getFragment)
        {
            _getFragment = getFragment;
        }

        public Fragment GetFragment(string name, string xml)
            => _getFragment(name, xml);
    }
}
