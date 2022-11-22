using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers
{
    internal class RadDropDownListHelper : IRadDropDownListHelper
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;

        public RadDropDownListHelper(IEnumHelper enumHelper, IExceptionHelper exceptionHelper)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
        }

        public void ClearAutoComplete(RadDropDownList dropDownList)
        {
            dropDownList.Items.Clear();
            dropDownList.DropDownListElement.AutoCompleteSuggest = null;
        }

        public void LoadBooleans(RadDropDownList dropDownList, RadDropDownStyle dropDownStyle = RadDropDownStyle.DropDownList)
        {
            dropDownList.DropDownStyle = dropDownStyle;
            dropDownList.Items.Clear();
            dropDownList.Items.AddRange
            (
                new bool[] { true, false }
                    .Select
                    (
                        b => new RadListDataItem
                        (
                            b.ToString(CultureInfo.CurrentCulture).ToLowerInvariant(),
                            b
                        )
                    )
            );
        }

        public void LoadComboItems<T>(RadDropDownList dropDownList, RadDropDownStyle dropDownStyle = RadDropDownStyle.DropDownList, T[]? excludedItems = null) where T : struct, Enum
        {
            if (!typeof(Enum).IsAssignableFrom(typeof(T)))
                throw _exceptionHelper.CriticalException("{AFB58CEB-D1A8-4ADC-A49F-6E9C46D03AA6}");

            if (dropDownStyle == RadDropDownStyle.DropDown)
                SetAutoCompleteSuggestHelper(dropDownList);

            HashSet<T> excludedItemsSet = excludedItems?.ToHashSet() ?? new HashSet<T>();

            dropDownList.DropDownStyle = dropDownStyle;
            dropDownList.Items.Clear();
            dropDownList.Items.AddRange
            (
                Enum.GetValues<T>()
                    .Where(v => !excludedItemsSet.Contains(v))
                    .Select
                    (
                        v => new RadListDataItem
                        (
                            _enumHelper.GetEnumResourceString(Enum.GetName(typeof(T), v)),
                            v
                        )
                    )
            );
        }

        public void LoadTextItems(RadDropDownList dropDownList, IEnumerable<string> items, RadDropDownStyle dropDownStyle = RadDropDownStyle.DropDownList)
        {
            if (dropDownStyle == RadDropDownStyle.DropDown)
                SetAutoCompleteSuggestHelper(dropDownList);

            dropDownList.DropDownStyle = dropDownStyle;
            dropDownList.Items.Clear();
            dropDownList.Items.AddRange(items);
        }

        public void SetAutoCompleteSuggestHelper(RadDropDownList dropDownList)
        {
            dropDownList.DropDownStyle = RadDropDownStyle.DropDown;
            dropDownList.AutoCompleteMode = AutoCompleteMode.None;
            dropDownList.DropDownListElement.AutoCompleteSuggest = new CustomAutoCompleteSuggestHelper(dropDownList.DropDownListElement);
        }
    }
}
