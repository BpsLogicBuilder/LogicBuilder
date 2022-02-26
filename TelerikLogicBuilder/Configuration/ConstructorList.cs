using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Configuration
{
    internal class ConstructorList
    {
        public ConstructorList(IDictionary<string, Constructor> constructors, TreeFolder constructorsTreeFolder)
        {
            Constructors = constructors;
            ConstructorsTreeFolder = constructorsTreeFolder;
        }

        internal IDictionary<string, Constructor> Constructors { get; }
        internal TreeFolder ConstructorsTreeFolder { get; }
    }
}
