using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Threading;

namespace ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories
{
    internal class DiagramValidatorFactory : IDiagramValidatorFactory
    {
        private readonly Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramValidator> _getDiagramValidator;

        public DiagramValidatorFactory(Func<string, Document, ApplicationTypeInfo, IProgress<ProgressMessage>, CancellationTokenSource, IDiagramValidator> getDiagramValidator)
        {
            _getDiagramValidator = getDiagramValidator;
        }

        public IDiagramValidator GetDiagramValidator(string sourceFile, Document document, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => _getDiagramValidator(sourceFile, document, application, progress, cancellationTokenSource);
    }
}
