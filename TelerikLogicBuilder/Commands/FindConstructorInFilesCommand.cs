﻿using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Commands
{
    internal class FindConstructorInFilesCommand : FindInFilesCommandBase<FindConstructorInFiles>
    {
        public FindConstructorInFilesCommand(
            ICheckVisioConfiguration checkVisioConfiguration,
            IGetSourceFilesForDocumentSearch getSourceFilesForDocumentSearch,
            IMainWindow mainWindow,
            ISearchFunctions searchFunctions,
            ISearchSelectedDocuments searchSelectedDocuments)
            : base(
                checkVisioConfiguration,
                getSourceFilesForDocumentSearch,
                mainWindow,
                searchFunctions,
                searchSelectedDocuments)
        {
        }

        protected override Func<string, string, bool, bool, IList<string>> MatchFunc => _searchFunctions.FindConstructorMatches;
    }
}
