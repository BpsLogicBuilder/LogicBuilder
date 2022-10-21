using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class DiagramResultMessageHelper : IDiagramResultMessageHelper
    {
        private readonly IResultMessageBuilder _resultMessageBuilder;

        public DiagramResultMessageHelper(
            IResultMessageBuilder resultMessageBuilder,
            IVisioFileSourceFactory visioFileSourceFactory,
            string sourceFile,
            Page page,
            Shape shape,
            List<ResultMessage> resultMessages)
        {
            _resultMessageBuilder = resultMessageBuilder;
            visioFileSource = visioFileSourceFactory.GetVisioFileSource
            (
                sourceFile,
                page.ID,
                page.Index,
                shape.Master.Name,
                shape.ID,
                shape.Index
            );
            this.resultMessages = resultMessages;
        }

        private readonly List<ResultMessage> resultMessages;
        private readonly VisioFileSource visioFileSource;

        public void AddValidationMessage(string message)
            => resultMessages.Add(GetResultMessage(message));

        public ResultMessage GetResultMessage(string message)
            => _resultMessageBuilder.BuilderMessage(visioFileSource, message);
    }
}
