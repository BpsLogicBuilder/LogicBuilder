using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator
{
    internal interface IDiagramValidator
    {
        Task<IList<ResultMessage>> Validate(string sourceFile, Document visioDocument, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource);
    }
}
