using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal interface IDiagramValidatorFactory
    {
        IDiagramValidator GetDiagramValidator(string sourceFile,
            Document document,
            ApplicationTypeInfo application,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource);
    }
}
