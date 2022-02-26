using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class FragmentList
    {
        public FragmentList(IDictionary<string, Fragment> fragments, TreeFolder fragmentsTreeFolder)
        {
            Fragments = fragments;
            FragmentsTreeFolder = fragmentsTreeFolder;
        }

        internal IDictionary<string, Fragment> Fragments { get; }
        internal TreeFolder FragmentsTreeFolder { get; }
    }
}
