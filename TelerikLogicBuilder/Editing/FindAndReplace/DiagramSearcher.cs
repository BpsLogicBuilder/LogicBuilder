using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class DiagramSearcher : IDiagramSearcher
    {
        private readonly IContextProvider _contextProvider;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IVisioFileSourceFactory _visioFileSourceFactory;

        public DiagramSearcher(IContextProvider contextProvider, IShapeXmlHelper shapeXmlHelper, IVisioFileSourceFactory visioFileSourceFactory)
        {
            _contextProvider = contextProvider;
            _shapeXmlHelper = shapeXmlHelper;
            _visioFileSourceFactory = visioFileSourceFactory;
        }

        public Task<SearchDiagramResults> Search(string sourceFile, Document visioDocument, string searchString, bool matchCase, bool matchWholeWord, Func<string, string, bool, bool, IList<string>> matchFunc, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
            => new DiagramSearcherUtility
            (
                sourceFile,
                visioDocument,
                searchString,
                matchCase,
                matchWholeWord,
                matchFunc,
                progress,
                cancellationTokenSource,
                _contextProvider,
                _shapeXmlHelper,
                _visioFileSourceFactory
            ).Search();
    }
}
