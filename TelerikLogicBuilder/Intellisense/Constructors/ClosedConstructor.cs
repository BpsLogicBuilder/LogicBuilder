using ABIS.LogicBuilder.FlowBuilder.Intellisense.GenericArguments;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors
{
    internal class ClosedConstructor
    {
        public ClosedConstructor(Constructor constructor, IList<GenericConfigBase> genericArguments)
        {
            Constructor = constructor;
            GenericArguments = genericArguments;
        }

        public Constructor Constructor { get; set; }
        public IList<GenericConfigBase> GenericArguments { get; set; }
    }
}
