using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Reactive.Subjects;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal interface IUiNotificationService
    {
        Subject<int> DocumentExplorerErrorCountChangedSubject { get; }
        Subject<LogicBuilderException> LogicBuilderExceptionSubject { get; }
        Subject<string> ParameterChangedSubject { get; }
        Subject<bool> RulesExplorerRefreshSubject { get; }

        void NotifyDocumentExplorerErrorCountChanged(int errorCount);
        void NotifyLogicBuilderException(LogicBuilderException exception);
        void NotifyParameterChanged(string parameterName);
        void RequestRulesExplorerRefresh(bool refresh);
    }
}
