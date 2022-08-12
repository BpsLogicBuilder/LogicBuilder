using ABIS.LogicBuilder.FlowBuilder.Enums;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface IGetSourceFilesForDocumentSearch
    {
        IList<string> GetSourceFiles(string searchPattern, SearchOptions searchOptions);
    }
}
