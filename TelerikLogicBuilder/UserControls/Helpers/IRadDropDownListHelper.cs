﻿using System;
using System.Collections.Generic;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal interface IRadDropDownListHelper
    {
        void ClearAutoComplete(RadDropDownList dropDownList);
        void LoadBooleans(RadDropDownList dropDownList, RadDropDownStyle dropDownStyle = RadDropDownStyle.DropDownList);
        void LoadComboItems<T>(RadDropDownList dropDownList, RadDropDownStyle dropDownStyle = RadDropDownStyle.DropDownList, T[]? excludedItems = null) where T : struct, Enum;
        void LoadTextItems(RadDropDownList dropDownList, IEnumerable<string> items, RadDropDownStyle dropDownStyle = RadDropDownStyle.DropDownList);
        void SetAutoCompleteSuggestHelper(RadDropDownList dropDownList);
    }
}
