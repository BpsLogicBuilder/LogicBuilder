using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms;
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
        private readonly IFileIOHelper _fileIOHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly Func<string, bool, TableControl> _getTableControl;
        private readonly Func<string, bool, VisioControl> _getVisioControl;

        public OpenFileOperations(
            ICheckVisioConfiguration checkVisioConfiguration,
            IFileIOHelper fileIOHelper,
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            IPathHelper pathHelper,
            Func<string, bool, TableControl> getTableControl,
            Func<string, bool, VisioControl> getVisioControl)
        {
            _checkVisioConfiguration = checkVisioConfiguration;
            _fileIOHelper = fileIOHelper;
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _getTableControl = getTableControl;
            _getVisioControl = getVisioControl;
        }

        public void OpenTableFile(string fileFullname)
        {
            if (GetOpenFile(fileFullname) != null)
                return;

            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            mdiParent.ChangeCursor(Cursors.WaitCursor);
            OpenFile(fileFullname, _getTableControl, mdiParent.AddTableControl);
            mdiParent.ChangeCursor(Cursors.Default);
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
                IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
                mdiParent.ChangeCursor(Cursors.WaitCursor);
                OpenFile(fileFullname, _getVisioControl, mdiParent.AddVisioControl);
                mdiParent.ChangeCursor(Cursors.Default);
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
