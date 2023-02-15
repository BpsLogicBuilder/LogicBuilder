using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Reactive.Subjects;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal class UiNotificationService : IUiNotificationService
    {
        #region Properties
        public Subject<int> DocumentExplorerErrorCountChangedSubject { get; } = new();
        public Subject<LogicBuilderException> LogicBuilderExceptionSubject { get; } = new();
        public Subject<bool> RulesExplorerRefreshSubject { get; } = new();
        public Subject<string> ParameterChangedSubject { get; } = new();
        #endregion Properties

        public void NotifyDocumentExplorerErrorCountChanged(int errorCount)
        {
            this.DocumentExplorerErrorCountChangedSubject.OnNext(errorCount);
        }

        public void NotifyLogicBuilderException(LogicBuilderException exception)
        {
            this.LogicBuilderExceptionSubject.OnNext(exception);
        }

        public void NotifyParameterChanged(string parameterName)
        {
            this.ParameterChangedSubject.OnNext(parameterName);
        }

        public void RequestRulesExplorerRefresh(bool refresh)
        {
            this.RulesExplorerRefreshSubject.OnNext(refresh);
        }
    }
}
