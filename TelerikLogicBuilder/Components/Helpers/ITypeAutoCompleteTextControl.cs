using ABIS.LogicBuilder.FlowBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Components.Helpers
{
    internal interface ITypeAutoCompleteTextControl
    {
        event EventHandler? Disposed;
        event MouseEventHandler? MouseDown;
        event EventHandler? TextChanged;

        bool Enabled { get; }
        string SelectedText { get; set; }
        string Text { get; set; }

        void EnableAddUpdateGenericArguments(bool enable);
        void ResetTypesList(IList<string>? items);
        void SetAddUpdateGenericArgumentsCommand(IClickCommand command);
        void SetContextMenus(RadContextMenuManager radContextMenuManager, RadContextMenu radContextMenu);
    }
}
