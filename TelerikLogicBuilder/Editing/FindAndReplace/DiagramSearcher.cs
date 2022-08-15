﻿using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
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

        public DiagramSearcher(IContextProvider contextProvider, IShapeXmlHelper shapeXmlHelper)
        {
            _contextProvider = contextProvider;
            _shapeXmlHelper = shapeXmlHelper;
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
                _shapeXmlHelper
            ).Search();
    }
}