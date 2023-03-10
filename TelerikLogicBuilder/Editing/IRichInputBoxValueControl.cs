using ABIS.LogicBuilder.FlowBuilder.Components;
using System;
using System.Collections.Generic;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal interface IRichInputBoxValueControl : IValueControl
    {
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
        RichInputBox RichInputBox { get; }
    }
}
