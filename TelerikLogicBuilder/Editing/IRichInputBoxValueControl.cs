using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IRichInputBoxValueControl : IValueControl
    {
        ApplicationTypeInfo Application { get; }
        Type AssignedTo { get; }
        IList<RadButton> CommandButtons { get; }
        RadMenuItem MnuItemInsert { get; }
        RadMenuItem MnuItemInsertConstructor { get; }
        RadMenuItem MnuItemInsertFunction { get; }
        RadMenuItem MnuItemInsertVariable { get; }
        RadMenuItem MnuItemDelete { get; }
        RadMenuItem MnuItemClear { get; }
        RadMenuItem MnuItemCopy { get; }
        RadMenuItem MnuItemCut { get; }
        RadMenuItem MnuItemPaste { get; }
        RadMenuItem MnuItemToCamelCase { get; }
        IRichInputBox RichInputBox { get; }

        void ClearMessage();
        void SetErrorMessage(string message);
        void SetMessage(string message, string title = "");
    }
}
