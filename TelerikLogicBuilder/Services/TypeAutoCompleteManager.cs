using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Commands.TypeAutoComplete.Factories;
using ABIS.LogicBuilder.FlowBuilder.Components.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class TypeAutoCompleteManager : ITypeAutoCompleteManager
    {
        private readonly IImageListService _imageListService;
        private readonly ITypeAutoCompleteCommandFactory _typeAutoCompleteCommandFactory;
        private readonly ITypeLoadHelper _typeLoadHelper;

        private readonly ITypeAutoCompleteTextControl textControl;
        private readonly IApplicationForm applicationForm;

        public TypeAutoCompleteManager(
            IImageListService imageListService,
            ITypeAutoCompleteCommandFactory typeAutoCompleteCommandFactory,
            ITypeLoadHelper typeLoadHelper,
            IApplicationForm applicationForm,
            ITypeAutoCompleteTextControl textControl)
        {
            _imageListService = imageListService;
            _typeAutoCompleteCommandFactory = typeAutoCompleteCommandFactory;
            _typeLoadHelper = typeLoadHelper;
            this.textControl = textControl;
            this.applicationForm = applicationForm;
            radContextMenuManager = new RadContextMenuManager();
            this.textControl.Disposed += Control_Disposed;
            this.textControl.MouseDown += Control_MouseDown;
            this.textControl.TextChanged += TextControl_TextChanged;
            this.applicationForm.ApplicationChanged += ApplicationForm_ApplicationChanged;
        }

        private readonly RadMenuItem mnuItemCopy = new(Strings.mnuItemCopyText) { ImageIndex = ImageIndexes.COPYIMAGEINDEX };
        private readonly RadMenuItem mnuItemCut = new(Strings.mnuItemCutText) { ImageIndex = ImageIndexes.CUTIMAGEINDEX };
        private readonly RadMenuItem mnuItemPaste = new(Strings.mnuItemPasteText);
        private readonly RadMenuItem mnuItemToAssemblyQualifiedName = new(Strings.mnuItemToAssemblyQualifiedName);
        private readonly RadMenuItem mnuItemAddUpdateGenericArguments = new(Strings.mnuItemAddUpdateGenericArguments);
        private readonly RadContextMenuManager radContextMenuManager;

        private Type? Type
        {
            get
            {
                _typeLoadHelper.TryGetSystemType
                (
                    textControl.Text,
                    applicationForm.Application,
                    out Type? type
                );

                return type;
            }
        }

        public void Setup()
        {
            SetupCommands();
            CreateContextMenu(); 
            SetTypesList();
            SetEnableAddUpdateGenericArguments();
        }

        private static void AddClickCommand(RadMenuItem radMenuItem, IClickCommand command)
        {
            radMenuItem.Click += (sender, args) => command.Execute();
        }

        private void ApplicationChanged()
        {
            SetTypesList();
        }

        private void CreateContextMenu()
        {
            RadContextMenu radContextMenu = new()
            {
                ImageList = _imageListService.ImageList,
                Items =
                {
                    mnuItemCopy,
                    mnuItemCut,
                    mnuItemPaste,
                    new RadMenuSeparatorItem(),
                    mnuItemToAssemblyQualifiedName,
                    new RadMenuSeparatorItem(),
                    mnuItemAddUpdateGenericArguments
                }
            };

            this.textControl.SetContextMenus(radContextMenuManager, radContextMenu);
        }

        private void SetEnableAddUpdateGenericArguments()
        {
            textControl.EnableAddUpdateGenericArguments(Type?.IsGenericType == true);
        }

        private void SetContextMenuState()
        {
            mnuItemToAssemblyQualifiedName.Enabled = textControl.Enabled && Type != null;
            mnuItemAddUpdateGenericArguments.Enabled = textControl.Enabled && Type != null && Type.IsGenericType;
            mnuItemPaste.Enabled = textControl.Enabled && Clipboard.GetText() != null;
            mnuItemCut.Enabled = textControl.Enabled && !string.IsNullOrEmpty(textControl.SelectedText);
            mnuItemCopy.Enabled = textControl.Enabled && !string.IsNullOrEmpty(textControl.SelectedText);
        }

        private void SetTypesList()
        {
            this.textControl.ResetTypesList
            (
                applicationForm.Application.AssemblyAvailable
                    ? applicationForm.Application.AllTypesList
                    : null
            );
        }

        private void SetupCommands()
        {
            AddClickCommand(mnuItemCopy, _typeAutoCompleteCommandFactory.GetCopySelectedTextCommand(textControl));
            AddClickCommand(mnuItemCut, _typeAutoCompleteCommandFactory.GetCutSelectedTextCommand(textControl));
            AddClickCommand(mnuItemPaste, _typeAutoCompleteCommandFactory.GetPasteTextCommand(textControl));
            AddClickCommand
            (
                mnuItemToAssemblyQualifiedName,
                _typeAutoCompleteCommandFactory.GetSetTextToAssemblyQualifiedNameCommand(applicationForm, textControl)
            );

            IClickCommand addUpdateGenericArgumentsCommand = _typeAutoCompleteCommandFactory.GetAddUpdateGenericArgumentsCommand(applicationForm, textControl);
            AddClickCommand
            (
                mnuItemAddUpdateGenericArguments,
                addUpdateGenericArgumentsCommand
            );

            this.textControl.SetAddUpdateGenericArgumentsCommand(addUpdateGenericArgumentsCommand);
        }

        private void TypeTextChanged()
        {
            SetEnableAddUpdateGenericArguments();
        }

        #region Event Handlers
        private void ApplicationForm_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
             => ApplicationChanged();

        private void Control_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                return;

            SetContextMenuState();
        }

        private void Control_Disposed(object? sender, EventArgs e)
        {
            radContextMenuManager.Dispose();
        }

        private void TextControl_TextChanged(object? sender, EventArgs e)
        {
            TypeTextChanged();
        }
        #endregion
    }
}
