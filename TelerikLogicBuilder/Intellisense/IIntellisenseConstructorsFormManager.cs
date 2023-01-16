using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;

namespace ABIS.LogicBuilder.FlowBuilder.Intellisense
{
    internal interface IIntellisenseConstructorsFormManager
    {
        void ApplicationChanged();
        void BuildTreeView(string classFullName);
        void ClearTreeView();
        void CmbClassTextChanged();
        void Initialize();
        void UpdateSelection(ConstructorHelperStatus? helperStatus);
    }
}
