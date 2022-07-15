using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal interface IAddNewFileOperations
    {
        void AddNewTableFile(string fullName);
        void AddNewVisioFile(string fullName);
    }
}
