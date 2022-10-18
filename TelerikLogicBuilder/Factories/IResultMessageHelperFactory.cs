using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Factories
{
    internal interface IResultMessageHelperFactory
    {
        IResultMessageHelper GetResultMessageHelper(string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> validationErrors);
    }
}
