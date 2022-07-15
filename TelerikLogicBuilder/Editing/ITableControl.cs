namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface ITableControl
    {
        void FindCell();
        void SetEvaluationFull();
        void SetEvaluationNone();
        void SetEvaluationUpdateOnly();
        void ToggleReevaluateAll();
        void ToggleActivateAll();
    }
}
