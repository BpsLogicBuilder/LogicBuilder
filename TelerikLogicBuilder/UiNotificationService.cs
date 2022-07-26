using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Reactive.Subjects;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal class UiNotificationService
    {
        #region Properties
        public Subject<int> DocumentExplorerErrorCountChangedSubject { get; } = new();
        public Subject<LogicBuilderException> LogicBuilderExceptionSubject { get; } = new();
        public Subject<bool> RulesExplorerRefreshSubject { get; } = new();
        #endregion Properties

        public void NotifyDocumentExplorerErrorCountChanged(int errorCount)
        {
            this.DocumentExplorerErrorCountChangedSubject.OnNext(errorCount);
        }

        public void NotifyLogicBuilderException(LogicBuilderException exception)
        {
            this.LogicBuilderExceptionSubject.OnNext(exception);
        }

        public void RequestRulesExplorerRefresh(bool refresh)
        {
            this.RulesExplorerRefreshSubject.OnNext(refresh);
        }
    }
}
