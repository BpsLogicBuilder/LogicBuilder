using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using System.Reactive.Subjects;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal class UiNotificationService
    {
        #region Properties
        public Subject<LogicBuilderException> LogicBuilderExceptionSubject { get; } = new();
        #endregion Properties

        public void NotifyLogicBuilderException(LogicBuilderException exception)
        {
            this.LogicBuilderExceptionSubject.OnNext(exception);
        }
    }
}
