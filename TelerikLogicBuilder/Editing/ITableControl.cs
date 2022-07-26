namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface ITableControl
    {
        void FindCell();
        void FindCell(int userRowIndex, int userColumnIndex);
        void SetEvaluationFull();
        void SetEvaluationNone();
        void SetEvaluationUpdateOnly();
        void ToggleReevaluateAll();
        void ToggleActivateAll();
    }
}
