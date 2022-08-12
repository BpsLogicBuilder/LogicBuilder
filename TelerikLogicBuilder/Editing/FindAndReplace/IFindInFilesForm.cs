using ABIS.LogicBuilder.FlowBuilder.Enums;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal interface IFindInFilesForm
    {
        SearchOptions SearchType { get; }

        string SearchPattern { get; }

        string SearchString { get; }

        bool MatchCase { get; }

        bool MatchWholeWord { get; }
    }
}
