using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
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

        private static void AddContextMenuClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
        }

        public void Create()
        {
            AddContextMenuClickCommand(MnuItemInsertConstructor, _fieldControlCommandFactory.GetEditRichInputBoxConstructorCommand(this.richInputBoxValueControl));
            AddContextMenuClickCommand(MnuItemInsertFunction, _fieldControlCommandFactory.GetEditRichInputBoxFunctionCommand(this.richInputBoxValueControl));
            AddContextMenuClickCommand(MnuItemInsertVariable, _fieldControlCommandFactory.GetEditRichInputBoxVariableCommand(this.richInputBoxValueControl));
            AddContextMenuClickCommand(MnuItemDelete, _fieldControlCommandFactory.GetDeleteRichInputBoxTextCommand(this.richInputBoxValueControl));
            AddContextMenuClickCommand(MnuItemClear, _fieldControlCommandFactory.GetClearRichInputBoxTextCommand(this.richInputBoxValueControl));
            AddContextMenuClickCommand(MnuItemCopy, _fieldControlCommandFactory.GetCopyRichInputBoxTextCommand(this.richInputBoxValueControl));
            AddContextMenuClickCommand(MnuItemCut, _fieldControlCommandFactory.GetCutRichInputBoxTextCommand(this.richInputBoxValueControl));
            AddContextMenuClickCommand(MnuItemPaste, _fieldControlCommandFactory.GetPasteRichInputBoxTextCommand(this.richInputBoxValueControl));
            AddContextMenuClickCommand(MnuItemToCamelCase, _fieldControlCommandFactory.GetToCamelCaseRichInputBoxCommand(this.richInputBoxValueControl));

            MnuItemInsert.Items.AddRange
            (
                new RadItem[]
                {
                    MnuItemInsertConstructor,
                    MnuItemInsertFunction,
                    MnuItemInsertVariable
                }
            );

            RadContextMenu radContextMenu = new()
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
        }

        #region Event Handlers
        private void RichInputBox_Disposed(object? sender, System.EventArgs e)
        {
            radContextMenuManager.Dispose();
        }
        #endregion Event Handlers
    }
}
