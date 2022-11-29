using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class OpenFileOperations : IOpenFileOperations
    {
        private readonly ICheckVisioConfiguration _checkVisioConfiguration;
        private readonly IDocumentEditorFactory _documentEditorFactory;
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;

        public OpenFileOperations(
            ICheckVisioConfiguration checkVisioConfiguration,
            IDocumentEditorFactory documentEditorFactory,
            IFileIOHelper fileIOHelper,
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            IPathHelper pathHelper)
        {
            _checkVisioConfiguration = checkVisioConfiguration;
            _documentEditorFactory = documentEditorFactory;
            _fileIOHelper = fileIOHelper;
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
        }

        public void OpenTableFile(string fileFullname)
        {
            if (GetOpenFile(fileFullname) != null)
                return;

            OpenFile(fileFullname, _documentEditorFactory.GetTableControl, ((IMDIParent)_mainWindow.Instance).AddTableControl);
        }

        public void OpenVisioFile(string fileFullname)
        {
            if (GetOpenFile(fileFullname) != null)
                return;

            IList<string> configErrors = _checkVisioConfiguration.Check();
            if (configErrors.Count > 0)
            {
                using IScopedDisposableManager<TextViewer> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<TextViewer>>();
                TextViewer textViewer = disposableManager.ScopedService;
                textViewer.SetText(configErrors.ToArray());
                textViewer.ShowDialog(_mainWindow.Instance);
                return;
            }

            try
            {
                OpenFile(fileFullname, _documentEditorFactory.GetVisioControl, ((IMDIParent)_mainWindow.Instance).AddVisioControl);
            }
            catch (COMException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        private IDocumentEditor? GetOpenFile(string fullName)
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            if (mdiParent.EditControl == null)
                return null;

            if (string.Compare(fullName, mdiParent.EditControl.SourceFile, StringComparison.InvariantCultureIgnoreCase) == 0)
                return mdiParent.EditControl;

            return null;
        }

        private void OpenFile(string fullName, Func<string, bool, IDocumentEditor> getControl, Action<IDocumentEditor> addControl)
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            mdiParent.EditControl?.Close();

            if (!_fileIOHelper.GetNewFileInfo(fullName).IsReadOnly)
            {
                addControl(getControl(fullName, false));
                return;
            }

            DialogResult dialogResult = DisplayMessage.ShowYesNoCancel
            (
                _mainWindow.Instance,
                string.Format(CultureInfo.CurrentCulture, Strings.fileNotWriteableFormat, _pathHelper.GetFileName(fullName)),
                _mainWindow.RightToLeft
            );

            if (dialogResult == DialogResult.Yes)
            {
                _fileIOHelper.SetWritable(fullName, true);
                addControl(getControl(fullName, false));
            }
            else if (dialogResult == DialogResult.No)
            {
                addControl(getControl(fullName, true));
            }
            else if (dialogResult == DialogResult.Cancel)
            {
            }
            else
            {
                throw _exceptionHelper.CriticalException("{AD5B3313-DF31-4ED4-B7E5-306344EC4621}");
            }
        }
    }
}
