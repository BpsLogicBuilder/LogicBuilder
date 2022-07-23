using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal class CustomAutoCompleteSuggestHelper : AutoCompleteSuggestHelper
    {
        public CustomAutoCompleteSuggestHelper(RadDropDownListElement owner) : base(owner)
        {
        }

        protected override bool DefaultFilter(RadListDataItem item) 
            => item.Text.ToLower().Contains(this.Filter.ToLower());
    }
}
