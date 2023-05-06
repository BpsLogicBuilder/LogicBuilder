using System.Collections.Generic;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal interface IRadDropDownListHelper
    {
        void ClearAutoComplete(RadDropDownList dropDownList);
        void LoadBooleans(RadDropDownList dropDownList, RadDropDownStyle dropDownStyle = RadDropDownStyle.DropDownList);
        void LoadComboItems<T>(RadDropDownList dropDownList, RadDropDownStyle dropDownStyle = RadDropDownStyle.DropDownList, T[]? excludedItems = null);
        // where T : struct, Enum causes exception in Dotfuscator Community
        //System.Security.VerificationException: Method System.Enum.GetValues: type argument 'a' violates the constraint of type parameter 'TEnum'.
        void LoadTextItems(RadDropDownList dropDownList, IEnumerable<string> items, RadDropDownStyle dropDownStyle = RadDropDownStyle.DropDownList);
        void SetAutoCompleteSuggestHelper(RadDropDownList dropDownList);
    }
}
