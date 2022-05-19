using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator.ShapeValidators;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class DiagramValidator : IDiagramValidator
    {
        private readonly IContextProvider _contextProvider;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IShapeValidator _shapeValidator;

        public DiagramValidator(IContextProvider contextProvider, IJumpDataParser jumpDataParser, IShapeHelper shapeHelper, IShapeXmlHelper shapeXmlHelper, IShapeValidator shapeValidator)
        {
            _contextProvider = contextProvider;
            _jumpDataParser = jumpDataParser;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _shapeValidator = shapeValidator;
        }

        public Task<IList<ResultMessage>> Validate(string sourceFile, Document visioDocument, ApplicationTypeInfo application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            return new DiagramValidatorUtility
            (
                sourceFile,
                visioDocument,
                application,
                progress,
                cancellationTokenSource,
                _contextProvider,
                _jumpDataParser,
                _shapeHelper,
                _shapeXmlHelper,
                _shapeValidator
            ).Validate();
        }
    }
}
