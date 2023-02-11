using ABIS.LogicBuilder.FlowBuilder.Components;
using System;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls
{
    internal interface IRichInputBoxValueControl : IValueControl
    {
        Type AssignedTo { get; }
        RadButton BtnConstructor { get; }
        RadButton BtnFunction { get; }
        RadButton BtnVariable { get; }
        bool DenySpecialCharacters { get; set; }
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

        void RequestDocumentUpdate();
    }
}
