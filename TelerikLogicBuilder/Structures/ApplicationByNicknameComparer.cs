using ABIS.LogicBuilder.FlowBuilder.Configuration;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class ApplicationByNicknameComparer : IComparer<Application>
    {
        public int Compare(Application applicationA, Application applicationB)
        {
            return string.Compare(applicationA.Nickname.Trim(), applicationB.Nickname.Trim(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
