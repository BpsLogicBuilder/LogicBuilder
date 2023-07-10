using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class CreateRichInputBoxContextMenu : ICreateRichInputBoxContextMenu
    {
        private readonly IFieldControlCommandFactory _fieldControlCommandFactory;
        private readonly IImageListService _imageListService;

        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public CreateRichInputBoxContextMenu(
            IFieldControlCommandFactory fieldControlCommandFactory,
            IImageListService imageListService,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _fieldControlCommandFactory = fieldControlCommandFactory;
            _imageListService = imageListService;
            this.richInputBoxValueControl = richInputBoxValueControl;
            radContextMenuManager = new RadContextMenuManager();
            this.richInputBoxValueControl.RichInputBox.Disposed += RichInputBox_Disposed;
        }

        private RadMenuItem MnuItemInsert => richInputBoxValueControl.MnuItemInsert;
        private RadMenuItem MnuItemInsertConstructor => richInputBoxValueControl.MnuItemInsertConstructor;
        private RadMenuItem MnuItemInsertFunction => richInputBoxValueControl.MnuItemInsertFunction;
        private RadMenuItem MnuItemInsertVariable => richInputBoxValueControl.MnuItemInsertVariable;
        private RadMenuItem MnuItemDelete => richInputBoxValueControl.MnuItemDelete;
        private RadMenuItem MnuItemClear => richInputBoxValueControl.MnuItemClear;
        private RadMenuItem MnuItemCopy => richInputBoxValueControl.MnuItemCopy;
        private RadMenuItem MnuItemCut => richInputBoxValueControl.MnuItemCut;
        private RadMenuItem MnuItemPaste => richInputBoxValueControl.MnuItemPaste;
        private RadMenuItem MnuItemToCamelCase => richInputBoxValueControl.MnuItemToCamelCase;
        private readonly RadContextMenuManager radContextMenuManager;
        private RadContextMenu? radContextMenu;
        private EventHandler? mnuItemInsertConstructorClickHandler;
        private EventHandler? mnuItemInsertFunctionClickHandler;
        private EventHandler? mnuItemInsertVariableClickHandler;
        private EventHandler? mnuItemDeleteClickHandler;
        private EventHandler? mnuItemClearClickHandler;
        private EventHandler? mnuItemCopyClickHandler;
        private EventHandler? mnuItemCutClickHandler;
        private EventHandler? mnuItemPasteClickHandler;
        private EventHandler? mnuItemToCamelCaseClickHandler;

        private static EventHandler AddContextMenuClickCommand(IClickCommand command)
        {
            return (sender, args) => command.Execute();
        }

        public void Create()
        {
            mnuItemInsertConstructorClickHandler = AddContextMenuClickCommand(_fieldControlCommandFactory.GetEditRichInputBoxConstructorCommand(this.richInputBoxValueControl));
            mnuItemInsertFunctionClickHandler = AddContextMenuClickCommand(_fieldControlCommandFactory.GetEditRichInputBoxFunctionCommand(this.richInputBoxValueControl));
            mnuItemInsertVariableClickHandler = AddContextMenuClickCommand(_fieldControlCommandFactory.GetEditRichInputBoxVariableCommand(this.richInputBoxValueControl));
            mnuItemDeleteClickHandler = AddContextMenuClickCommand(_fieldControlCommandFactory.GetDeleteRichInputBoxTextCommand(this.richInputBoxValueControl));
            mnuItemClearClickHandler = AddContextMenuClickCommand(_fieldControlCommandFactory.GetClearRichInputBoxTextCommand(this.richInputBoxValueControl));
            mnuItemCopyClickHandler = AddContextMenuClickCommand(_fieldControlCommandFactory.GetCopyRichInputBoxTextCommand(this.richInputBoxValueControl));
            mnuItemCutClickHandler = AddContextMenuClickCommand(_fieldControlCommandFactory.GetCutRichInputBoxTextCommand(this.richInputBoxValueControl));
            mnuItemPasteClickHandler = AddContextMenuClickCommand(_fieldControlCommandFactory.GetPasteRichInputBoxTextCommand(this.richInputBoxValueControl));
            mnuItemToCamelCaseClickHandler = AddContextMenuClickCommand(_fieldControlCommandFactory.GetToCamelCaseRichInputBoxCommand(this.richInputBoxValueControl));

            MnuItemInsert.Items.AddRange
            (
                new RadItem[]
                {
                    MnuItemInsertConstructor,
                    MnuItemInsertFunction,
                    MnuItemInsertVariable
                }
            );

            radContextMenu = new()
            {
                ImageList = _imageListService.ImageList,
                Items =
                {
                    MnuItemInsert,
                    MnuItemDelete,
                    MnuItemClear,
                    new RadMenuSeparatorItem(),
                    MnuItemCopy,
                    MnuItemCut,
                    MnuItemPaste,
                    new RadMenuSeparatorItem(),
                    MnuItemToCamelCase
                }
            };

            radContextMenuManager.SetRadContextMenu(richInputBoxValueControl.RichInputBox, radContextMenu);
            AddClickCommands();
        }

        private void AddClickCommands()
        {
            RemoveClickCommands();
            MnuItemInsertConstructor.Click += mnuItemInsertConstructorClickHandler;
            MnuItemInsertFunction.Click += mnuItemInsertFunctionClickHandler;
            MnuItemInsertVariable.Click += mnuItemInsertVariableClickHandler;
            MnuItemDelete.Click += mnuItemDeleteClickHandler;
            MnuItemClear.Click += mnuItemClearClickHandler;
            MnuItemCopy.Click += mnuItemCopyClickHandler;
            MnuItemCut.Click += mnuItemCutClickHandler;
            MnuItemPaste.Click += mnuItemPasteClickHandler;
            MnuItemToCamelCase.Click += mnuItemToCamelCaseClickHandler;
        }

        private void RemoveClickCommands()
        {
            MnuItemInsertConstructor.Click -= mnuItemInsertConstructorClickHandler;
            MnuItemInsertFunction.Click -= mnuItemInsertFunctionClickHandler;
            MnuItemInsertVariable.Click -= mnuItemInsertVariableClickHandler;
            MnuItemDelete.Click -= mnuItemDeleteClickHandler;
            MnuItemClear.Click -= mnuItemClearClickHandler;
            MnuItemCopy.Click -= mnuItemCopyClickHandler;
            MnuItemCut.Click -= mnuItemCutClickHandler;
            MnuItemPaste.Click -= mnuItemPasteClickHandler;
            MnuItemToCamelCase.Click -= mnuItemToCamelCaseClickHandler;
        }

        #region Event Handlers
        private void RichInputBox_Disposed(object? sender, System.EventArgs e)
        {
            RemoveClickCommands();
            radContextMenuManager.SetRadContextMenu(richInputBoxValueControl.RichInputBox, null);
            if (radContextMenu != null)
                radContextMenu.ImageList = null;
            radContextMenu?.Dispose();
            radContextMenuManager.Dispose();
        }
        #endregion Event Handlers
    }
}
