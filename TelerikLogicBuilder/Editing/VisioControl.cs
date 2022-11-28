using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace;
using ABIS.LogicBuilder.FlowBuilder.Editing.Forms;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Telerik.WinControls;
using Visio = Microsoft.Office.Interop.Visio;
using VisOCX = AxMicrosoft.Office.Interop.VisOcx;

namespace ABIS.LogicBuilder.FlowBuilder.Editing
{
    internal partial class VisioControl : UserControl, IDocumentEditor, IDrawingControl
    {
        private readonly IFormInitializer _formInitializer;
        private readonly IMainWindow _mainWindow;
        private readonly IPathHelper _pathHelper;
        private readonly IUiNotificationService _uiNotificationService;

        public VisioControl(
            IFormInitializer formInitializer, 
            IMainWindow mainWindow, 
            IPathHelper pathHelper, 
            IUiNotificationService uiNotificationService, 
            string visioSourceFile, 
            bool openedAsReadOnly)
        {
            _formInitializer = formInitializer;
            _mainWindow = mainWindow;
            _pathHelper = pathHelper;
            _uiNotificationService = uiNotificationService;

            InitializeComponent();
            this.openedAsReadOnly = openedAsReadOnly;
            this.parentForm = (IMDIParent)_mainWindow.Instance;
            Initialize();

            this.parentForm.CommandBarButtonSave.Enabled = !openedAsReadOnly;
            this.parentForm.RadMenuItemSave.Enabled = !openedAsReadOnly;
            this.parentForm.RadMenuItemDelete.Enabled = !openedAsReadOnly;
            this.parentForm.RadMenuItemUndo.Enabled = !openedAsReadOnly;
            this.parentForm.RadMenuItemRedo.Enabled = !openedAsReadOnly;

            this.visioSourceFile = visioSourceFile;

            CheckDocumentAvailability();

            axDrawingControl1.Src = visioSourceFile;
            axDrawingControl1.Document.Saved = true;

            LockDocument();

            this.Text = openedAsReadOnly
                ? string.Format
                (
                    CultureInfo.CurrentCulture, 
                    Strings.formCaptionReadOnlyFormat, 
                    _pathHelper.GetFileName(visioSourceFile)
                )
                : _pathHelper.GetFileName(visioSourceFile);

            toolTip.SetToolTip(titleBar1, visioSourceFile);
            titleBar1.Text = this.Text;
        }

        #region Constants
        private const string PROPERTYLOCK = "1";
        private const string PROPERTYUNLOCK = "0";
        private const string COMPANYNAME = "BPS";
        #endregion Constants

        private readonly RadToolTip toolTip = new();
        
        private readonly IMDIParent parentForm;
        private readonly bool openedAsReadOnly;
        private readonly string visioSourceFile = string.Empty;

        private Visio.Application _application;
        private FileStream? fileStream;
        private string flowDiagramStencilPath = string.Empty;
        private string applicationsStencilPath = string.Empty;
        private Visio.Shape? selectedShape;

        #region Properties
        bool IDocumentEditor.IsOpenReadOnly => openedAsReadOnly;
        string IDocumentEditor.SourceFile => visioSourceFile;
        string IDocumentEditor.Caption => this.titleBar1.Text;
        #endregion Properties

        #region Methods
        private void CheckDocumentAvailability()
        {
            FileStream? fStream = null;
            try
            {//throws IOException if the file is opened for editing
                if (openedAsReadOnly)
                    fStream = new FileStream(this.visioSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                else
                    fStream = new FileStream(this.visioSourceFile, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            finally
            {
                if (fStream != null)
                    fStream.Close();
            }
        }

        private void CloseControl()
        {
            SaveAndReleaseDocument();
            this.parentForm.RemoveEditControl();
            this.parentForm.CommandBar.Enabled = false;
            this.parentForm.SetEditControlMenuStates(false, false);
            _application.MarkerEvent -= MarkerEventEventHandler;
        }

        private void CloseOtherStencils()
        {
            this.axDrawingControl1.Focus();
            List<Visio.Document> docsToClose = new();
            foreach (Visio.Document document in axDrawingControl1.Document.Application.Documents)
            {//Need to check if stencils are open.  Simply closing all stencils and reopening fails - i.e. window count remains the same after calling axDrawingControl1.Document.Application.Documents.OpenEx(stencilPath, Flags)
                if (document.Type == Visio.VisDocumentTypes.visTypeStencil
                    //Removed until we support multiple applications
                    && string.Compare(document.FullName.Trim(), applicationsStencilPath, true, CultureInfo.InvariantCulture) != 0
                    && string.Compare(document.FullName.Trim(), flowDiagramStencilPath, true, CultureInfo.InvariantCulture) != 0)
                    docsToClose.Add(document);
            }

            foreach (Visio.Document document in docsToClose)
                document.Close();
        }

        private void CloseOtherWindows()
        {
            if (axDrawingControl1.Window == null)
                return;

            this.axDrawingControl1.Focus();
            foreach (Visio.Window window in axDrawingControl1.Window.Windows)
            {
                //Without this check your stencils may be hidden
                if (!Enum.IsDefined(typeof(Visio.VisWinTypes), window.ID) && window.Document != null && window.Document.Company == COMPANYNAME)
                    continue;

                if (window.ID != (int)Visio.VisWinTypes.visWinIDShapeSearch && window.ID != (int)Visio.VisWinTypes.visWinIDPanZoom)
                {
                    window.Visible = false;
                }
            }
        }

        private void DeleteSelection()
        {
            if (axDrawingControl1.Window.Selection.Count > 0)
                axDrawingControl1.Window.Selection.Delete();
        }

        private static void DisplayIndexInformation()
        {
            //if (axDrawingControl1.Window.Selection.Count < 1)
            //    return;

            //Visio.Shape shape = (Visio.Shape)axDrawingControl1.Window.Selection[1];
            //Visio.Page page = (Visio.Page)axDrawingControl1.Window.Page;
            //using (IndexInformation indexInformation = new IndexInformation(page.Index, shape.Index))
            //{
            //    indexInformation.StartPosition = FormStartPosition.CenterParent;
            //    indexInformation.ShowDialog();
            //}
        }

        private void Edit()
        {
            //Ensure that selectedShape reference is null if a shape is not currently selected
            if (axDrawingControl1.Window.Selection.Count > 0)
                selectedShape = axDrawingControl1.Window.Selection[1];
            else
                selectedShape = null;

            EditShape();
        }

        private void EditShape()
        {
            if (selectedShape == null)
                return;

            //ShapeEditor shapeEditor;
            switch (selectedShape.Master.NameU)
            {
                case UniversalMasterName.APP01CONNECTOBJECT:
                case UniversalMasterName.APP02CONNECTOBJECT:
                case UniversalMasterName.APP03CONNECTOBJECT:
                case UniversalMasterName.APP04CONNECTOBJECT:
                case UniversalMasterName.APP05CONNECTOBJECT:
                case UniversalMasterName.APP06CONNECTOBJECT:
                case UniversalMasterName.APP07CONNECTOBJECT:
                case UniversalMasterName.APP08CONNECTOBJECT:
                case UniversalMasterName.APP09CONNECTOBJECT:
                case UniversalMasterName.APP10CONNECTOBJECT:
                case UniversalMasterName.OTHERSCONNECTOBJECT:
                    break;
                case UniversalMasterName.CONNECTOBJECT:
                    //shapeEditor = new ConnectorEditor(selectedShape, this, parentForm);
                    //shapeEditor.EditShape();
                    break;
                case UniversalMasterName.ACTION:
                case UniversalMasterName.DIALOG:
                case UniversalMasterName.CONDITIONOBJECT:
                case UniversalMasterName.DECISIONOBJECT:
                case UniversalMasterName.JUMPOBJECT:
                case UniversalMasterName.MODULE:
                case UniversalMasterName.WAITDECISIONOBJECT:
                case UniversalMasterName.WAITCONDITIONOBJECT:
                    //shapeEditor = new ShapeEditor(selectedShape, this, parentForm);
                    //shapeEditor.EditShape();
                    break;
                case UniversalMasterName.BEGINFLOW:
                case UniversalMasterName.ENDFLOW:
                case UniversalMasterName.MODULEBEGIN:
                case UniversalMasterName.MODULEEND:
                case UniversalMasterName.TERMINATE:
                case UniversalMasterName.COMMENT:
                    break;
                default:
                    throw new LogicBuilderException(Strings.invalidShapeMessage);
            }
        }

        private void EnsureStencilWindowVisible()
        {
            if (axDrawingControl1.Window == null)
                return;

            foreach (Visio.Window window in axDrawingControl1.Window.Windows)
            {
                if (window.ID == (int)Visio.VisWinTypes.visWinIDShapeSearch && !window.Visible)
                {
                    window.Visible = true;
                    return;
                }
            }
        }

        private void FindAndReplaceConstructor()
        {
            using IScopedDisposableManager<IFindReplaceConstructorInShape> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IFindReplaceConstructorInShape>>();
            IFindReplaceConstructorInShape findReplace = disposableManager.ScopedService;
            findReplace.Setup(axDrawingControl1.Document);
            findReplace.StartPosition = FormStartPosition.Manual;
            findReplace.Location = new Point(100, 50);
            findReplace.ShowDialog(this.parentForm);
            Save();
        }

        private void FindAndReplaceFunction()
        {
            using IScopedDisposableManager<IFindReplaceFunctionInShape> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IFindReplaceFunctionInShape>>();
            IFindReplaceFunctionInShape findReplace = disposableManager.ScopedService;
            findReplace.Setup(axDrawingControl1.Document);
            findReplace.StartPosition = FormStartPosition.Manual;
            findReplace.Location = new Point(100, 50);
            findReplace.ShowDialog(this.parentForm);
            Save();
        }

        private void FindAndReplaceText()
        {
            using IScopedDisposableManager<IFindReplaceTextInShape> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IFindReplaceTextInShape>>();
            IFindReplaceTextInShape findReplaceText = disposableManager.ScopedService;
            findReplaceText.Setup(axDrawingControl1.Document);
            findReplaceText.StartPosition = FormStartPosition.Manual;
            findReplaceText.Location = new Point(100, 50);
            findReplaceText.ShowDialog(this.parentForm);
            Save();
        }

        private void FindAndReplaceVariable()
        {
            using IScopedDisposableManager<IFindReplaceVariableInShape> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IFindReplaceVariableInShape>>();
            IFindReplaceVariableInShape findReplace = disposableManager.ScopedService;
            findReplace.Setup(axDrawingControl1.Document);
            findReplace.StartPosition = FormStartPosition.Manual;
            findReplace.Location = new Point(100, 50);
            findReplace.ShowDialog(this.parentForm);
            Save();
        }

        private void FindConstructor()
        {
            using IScopedDisposableManager<IFindConstructorInShape> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IFindConstructorInShape>>();
            IFindConstructorInShape findConstructor = disposableManager.ScopedService;
            findConstructor.Setup(axDrawingControl1.Document);
            findConstructor.StartPosition = FormStartPosition.Manual;
            findConstructor.Location = new Point(100, 50);
            findConstructor.ShowDialog(this.parentForm);
        }

        private void FindFunction()
        {
            using IScopedDisposableManager<IFindFunctionInShape> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IFindFunctionInShape>>();
            IFindFunctionInShape findFunction = disposableManager.ScopedService;
            findFunction.Setup(axDrawingControl1.Document);
            findFunction.StartPosition = FormStartPosition.Manual;
            findFunction.Location = new Point(100, 50);
            findFunction.ShowDialog(this.parentForm);
        }

        private void FindShape()
        {
            using IScopedDisposableManager<FindShape> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<FindShape>>();
            FindShape findShape = disposableManager.ScopedService;
            findShape.StartPosition = FormStartPosition.Manual;
            findShape.Location = new Point(100, 50);
            findShape.Setup(axDrawingControl1.Document);
            findShape.ShowDialog(this);
        }

        public void FindShape(int pageIndex, int shapeIndex, int pageId, int shapeId)
        {
            if (axDrawingControl1.Document.Pages.Count < pageIndex)
            {
                DisplayMessage.Show
                (
                    (IWin32Window)this.parentForm,
                    string.Format(CultureInfo.CurrentCulture, Strings.pageIndexIsInvalidFormat, pageIndex), 
                    string.Empty,
                    this._mainWindow.RightToLeft
                );
                return;
            }

            if (axDrawingControl1.Document.Pages[pageIndex].ID != pageId)
            {
                DisplayMessage.Show
                (
                    (IWin32Window)this.parentForm,
                    Strings.pageMovedOrDeleted, 
                    string.Empty,
                    this._mainWindow.RightToLeft
                );
                return;
            }

            if (axDrawingControl1.Document.Pages[pageIndex].Shapes.Count < shapeIndex)
            {
                DisplayMessage.Show
                (
                    (IWin32Window)this.parentForm,
                    string.Format(CultureInfo.CurrentCulture, Strings.shapeIndexIsInvalidFormat, shapeIndex, pageIndex),
                    _mainWindow.RightToLeft
                );
                return;
            }

            Visio.Shape shape = axDrawingControl1.Document.Pages[pageIndex].Shapes[shapeIndex];

            if (shape.ID == shapeId)
            {
                double xCoordinate = shape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowXFormOut, (short)Visio.VisCellIndices.visXFormPinX).ResultIU;
                double yCoordinate = shape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowXFormOut, (short)Visio.VisCellIndices.visXFormPinY).ResultIU;
                axDrawingControl1.Window.Page = axDrawingControl1.Document.Pages[pageIndex];
                axDrawingControl1.Window.Select(shape, (short)Visio.VisSelectArgs.visSelect);
                axDrawingControl1.Window.ScrollViewTo(xCoordinate, yCoordinate);
            }
            else
            {
                Visio.Shape? shapeFromId = null;
                try
                {
                    shapeFromId = axDrawingControl1.Document.Pages[pageIndex].Shapes.get_ItemFromID(shapeId);
                }
                catch (System.Runtime.InteropServices.COMException)
                {
                }

                if (shapeFromId == null || shapeFromId.Index > axDrawingControl1.Document.Pages[pageIndex].Shapes.Count)
                {
                    DisplayMessage.Show
                    (
                        (IWin32Window)this.parentForm,
                        Strings.shapeDeleted,
                        _mainWindow.RightToLeft
                    );
                    return;
                }
                else
                {
                    double xCoordinate = shapeFromId.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowXFormOut, (short)Visio.VisCellIndices.visXFormPinX).ResultIU;
                    double yCoordinate = shapeFromId.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowXFormOut, (short)Visio.VisCellIndices.visXFormPinY).ResultIU;
                    axDrawingControl1.Window.Page = axDrawingControl1.Document.Pages[pageIndex];
                    axDrawingControl1.Window.Select(shapeFromId, (short)Visio.VisSelectArgs.visSelect);
                    axDrawingControl1.Window.ScrollViewTo(xCoordinate, yCoordinate);
                }
            }
        }

        private void FindText()
        {
            using IScopedDisposableManager<IFindTextInShape> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IFindTextInShape>>();
            IFindTextInShape findText = disposableManager.ScopedService;
            findText.Setup(axDrawingControl1.Document);
            findText.StartPosition = FormStartPosition.Manual;
            findText.Location = new Point(100, 50);
            findText.ShowDialog(this.parentForm);
        }

        private void FindVariable()
        {
            using IScopedDisposableManager<IFindVariableInShape> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<IFindVariableInShape>>();
            IFindVariableInShape findVariable = disposableManager.ScopedService;
            findVariable.Setup(axDrawingControl1.Document);
            findVariable.StartPosition = FormStartPosition.Manual;
            findVariable.Location = new Point(100, 50);
            findVariable.ShowDialog(this.parentForm);
        }

        void IDocumentEditor.Close() => CloseControl();

        void IDocumentEditor.Delete() => DeleteSelection();

        void IDocumentEditor.Edit() => Edit();

        void IDocumentEditor.FindText() => FindText();

        void IDocumentEditor.FindConstructor() => FindConstructor();

        void IDocumentEditor.FindFunction() => FindFunction();

        void IDocumentEditor.FindVariable() => FindVariable();

        void IDocumentEditor.ReplaceText() => FindAndReplaceText();

        void IDocumentEditor.ReplaceConstructor() => FindAndReplaceConstructor();

        void IDocumentEditor.ReplaceFunction() => FindAndReplaceFunction();

        void IDocumentEditor.ReplaceVariable() => FindAndReplaceVariable();

        void IDocumentEditor.Save() => Save();

        void IDrawingControl.DisplayIndexInformation() => DisplayIndexInformation();

        void IDrawingControl.FindShape() => FindShape();

        void IDrawingControl.PageSetup() => PageSetup();

        void IDrawingControl.Redo() => Redo();

        void IDrawingControl.ShowApplicationsStencil() => OpenApplicationsStencil();

        void IDrawingControl.ShowFlowDiagramStencil() => OpenFlowDiagramStencil();

        void IDrawingControl.ShowPanAndZoom() => ShowPanAndZoom();

        void IDrawingControl.Undo() => Undo();

        [MemberNotNull(nameof(_application), nameof(DocumentSaveAsEventHandler), nameof(ShapeAddedEventHandler), nameof(DocumentOpenedEventHandler), nameof(MarkerEventEventHandler))]
        private void Initialize()
        {
            parentForm.RadMenuItemIndexInformation.Enabled = false;
            parentForm.CommandBar.Enabled = true;
            _formInitializer.SetToolTipDefaults(toolTip);

            axDrawingControl1.Dock = DockStyle.Fill;
            this.parentForm.FormClosing += new FormClosingEventHandler(ParentForm_FormClosing);
            this.titleBar1.CloseClick += new Components.TitleBar.CloseClickHandler(TitleBar1_CloseClick);

            DocumentSaveAsEventHandler = new VisOCX.EVisOcx_DocumentSavedAsEventHandler(DocumentSavedAs_EventHandler);
            axDrawingControl1.DocumentSavedAs += DocumentSaveAsEventHandler;
            ShapeAddedEventHandler = new VisOCX.EVisOcx_ShapeAddedEventHandler(ShapeAdded_EventHandler);
            axDrawingControl1.ShapeAdded += ShapeAddedEventHandler;

            _application = axDrawingControl1.Window.Application;
            MarkerEventEventHandler = new Visio.EApplication_MarkerEventEventHandler(Application_MarkerEvent);
            _application.MarkerEvent -= MarkerEventEventHandler;
            _application.MarkerEvent += MarkerEventEventHandler;

            DocumentOpenedEventHandler = new VisOCX.EVisOcx_DocumentOpenedEventHandler(DocumentOpened_EventHandler);
            axDrawingControl1.DocumentOpened += DocumentOpenedEventHandler;

            SetStencilPaths();

            axDrawingControl1.Document.Application.Settings.EnableAutoConnect = false;
        }

        private bool IsStencilDocked(string stencilPath)
        {
            if (axDrawingControl1.Window == null)
                return true;

            axDrawingControl1.Window.DockedStencils(out Array astrStencilNames);
            for (int i = 0; i < astrStencilNames.GetLength(0); i++)
            {
                string path = astrStencilNames.GetValue(i)?.ToString() ?? "";
                if (stencilPath.ToLower(CultureInfo.CurrentCulture).Trim() == path.ToLower(CultureInfo.CurrentCulture).Trim())
                    return true;
            }
            return false;
        }

        private void LockDocument()
        {
            try
            {
                if (openedAsReadOnly)
                    fileStream = new FileStream(this.visioSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                else
                    fileStream = new FileStream(this.visioSourceFile, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }

        private void OpenApplicationsStencil()
        {
            if (!File.Exists(applicationsStencilPath))
            {
                DisplayMessage.Show((IWin32Window)this.parentForm, string.Format(CultureInfo.CurrentCulture, Strings.fileNotFoundFormat, applicationsStencilPath), _mainWindow.RightToLeft);
                return;
            }

            this.axDrawingControl1.Focus();
            EnsureStencilWindowVisible();
            if (!IsStencilDocked(applicationsStencilPath))
            {
                axDrawingControl1.Document.Application.Documents.OpenEx(applicationsStencilPath, (short)Visio.VisOpenSaveArgs.visOpenDocked + (short)Visio.VisOpenSaveArgs.visOpenRO);
                RemoveHourGlass();
            }
        }

        private void OpenFlowDiagramStencil()
        {
            if (!File.Exists(flowDiagramStencilPath))
            {
                DisplayMessage.Show
                (
                    (IWin32Window)this.parentForm,
                    string.Format(CultureInfo.CurrentCulture, Strings.fileNotFoundFormat, flowDiagramStencilPath),
                    _mainWindow.RightToLeft
                );
                return;
            }

            this.axDrawingControl1.Focus();
            EnsureStencilWindowVisible();
            if (!IsStencilDocked(flowDiagramStencilPath))
            {
                axDrawingControl1.Document.Application.Documents.OpenEx(flowDiagramStencilPath, (short)Visio.VisOpenSaveArgs.visOpenDocked + (short)Visio.VisOpenSaveArgs.visOpenRO);
                RemoveHourGlass();
            }
        }

        private void OpenStencils()
        {
            this.axDrawingControl1.Focus();
            CloseOtherWindows();
            CloseOtherStencils();
            OpenApplicationsStencil();
            OpenFlowDiagramStencil();
        }

        private void PageSetup()
        {
            try
            {
                axDrawingControl1.Document.Application.DoCmd((short)Visio.VisUICmds.visCmdOptionsPageSetup);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                RemoveHourGlass();
            }
        }

        private void Redo()
        {
            if (!axDrawingControl1.Document.Application.IsUndoingOrRedoing)
                axDrawingControl1.Document.Application.Redo();
        }

        private void RemoveHourGlass()
        {//this hack makes the hour glass go away when the user does not make modifications in page setup
            axDrawingControl1.Window.Zoom = axDrawingControl1.Window.Zoom + 1;
            axDrawingControl1.Window.Zoom = axDrawingControl1.Window.Zoom - 1;
        }

        private void Save()
        {
            try
            {
                SaveStayOpen();
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(new LogicBuilderException(ex.Message, ex));
            }
        }

        private void SaveAndReleaseDocument()
        {
            if (this.parentForm == null)
                return;

            if (this.parentForm.EditControl != this)
                return;

            if (!openedAsReadOnly && this.parentForm.EditControl == this && !axDrawingControl1.Document.Saved)
            {
                DialogResult dialogResult = DisplayMessage.ShowYesNo
                (
                    (IWin32Window)this.parentForm,
                    string.Format
                    (
                        CultureInfo.CurrentCulture,
                        Strings.saveFormQuestionFormat,
                        _pathHelper.GetFileName(this.visioSourceFile)
                    ),
                    string.Empty,
                    _mainWindow.RightToLeft
                );
                if (dialogResult == DialogResult.Yes)
                    SaveOnClose();
                else
                    UnlockDocument();
            }
            else
            {
                UnlockDocument();
            }
        }

        private void SaveOnClose()
        {
            if (axDrawingControl1.Document.Saved)
                return;

            UnlockDocument();

            if (openedAsReadOnly)
                return;

            try
            {
                axDrawingControl1.Document.SaveAs(this.visioSourceFile);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                axDrawingControl1.Document.Saved = true;
            }
        }

        private void SaveStayOpen()
        {
            if (axDrawingControl1.Document.Saved)
                return;

            if (openedAsReadOnly)
                return;

            UnlockDocument();

            try
            {
                axDrawingControl1.Document.SaveAs(this.visioSourceFile);
            }
            catch (System.Runtime.InteropServices.COMException)
            {
                axDrawingControl1.Document.Saved = true;
            }

            LockDocument();
        }

        private void SetStencilPaths()
        {
            string specificCulture = Thread.CurrentThread.CurrentUICulture.Name;
            string neutralCulture = Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName;
            if (Directory.Exists(_pathHelper.CombinePaths(ApplicationProperties.ApplicationPath, specificCulture)))
            {
                applicationsStencilPath = _pathHelper.CombinePaths(ApplicationProperties.ApplicationPath, specificCulture, ApplicationProperties.ApplicationsStencil);
                flowDiagramStencilPath = _pathHelper.CombinePaths(ApplicationProperties.ApplicationPath, specificCulture, ApplicationProperties.FlowDiagramStencil);
            }
            else if (Directory.Exists(_pathHelper.CombinePaths(ApplicationProperties.ApplicationPath, neutralCulture)))
            {
                applicationsStencilPath = _pathHelper.CombinePaths(ApplicationProperties.ApplicationPath, neutralCulture, ApplicationProperties.ApplicationsStencil);
                flowDiagramStencilPath = _pathHelper.CombinePaths(ApplicationProperties.ApplicationPath, neutralCulture, ApplicationProperties.FlowDiagramStencil);
            }
            else
            {
                applicationsStencilPath = _pathHelper.CombinePaths(ApplicationProperties.ApplicationPath, ApplicationProperties.ApplicationsStencil);
                flowDiagramStencilPath = _pathHelper.CombinePaths(ApplicationProperties.ApplicationPath, ApplicationProperties.FlowDiagramStencil);
            }
        }

        private void ShapeAdded(VisOCX.EVisOcx_ShapeAddedEvent e)
        {
            if (this.openedAsReadOnly)
            {
                e.shape.Delete();
                return;
            }

            Visio.Cell doubleClickCell = e.shape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowEvent, (short)Visio.VisCellIndices.visEvtCellDblClick);
            Visio.Cell lockTextEditCell = e.shape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockTextEdit);
            Visio.Cell lockCustPropCell = e.shape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockCustProp);
            doubleClickCell.FormulaU = string.Format(CultureInfo.InvariantCulture, VisioDoubleClick.CELLFORMULAFORMAT, e.shape.ID);
            lockTextEditCell.FormulaU = PROPERTYLOCK;
            lockCustPropCell.FormulaU = PROPERTYLOCK;
        }

        private void ShowPanAndZoom()
        {
            if (axDrawingControl1.Window == null)
                return;

            foreach (Visio.Window window in axDrawingControl1.Window.Windows)
            {
                if (window.ID == (int)Visio.VisWinTypes.visWinIDPanZoom && !window.Visible)
                {
                    window.Visible = true;
                    return;
                }
            }
        }

        private void Undo()
        {
            if (!axDrawingControl1.Document.Application.IsUndoingOrRedoing)
                axDrawingControl1.Document.Application.Undo();
        }

        private void UnlockDocument()
        {
            if (fileStream != null)
                fileStream.Close();
        }
        #endregion Methods

        #region Event Handlers
        private VisOCX.EVisOcx_DocumentSavedAsEventHandler DocumentSaveAsEventHandler;
        private VisOCX.EVisOcx_ShapeAddedEventHandler ShapeAddedEventHandler;
        private VisOCX.EVisOcx_DocumentOpenedEventHandler DocumentOpenedEventHandler;
        private Visio.EApplication_MarkerEventEventHandler MarkerEventEventHandler;

        void DocumentOpened_EventHandler(object sender, VisOCX.EVisOcx_DocumentOpenedEvent e)
        {
            SetDoubleClick();
        }

        private void SetDoubleClick()
        {
            for (int i = 1; i <= this.axDrawingControl1.Document.Pages.Count; i++)
            {
                Visio.Page page = this.axDrawingControl1.Document.Pages[i];

                for (int j = 1; j <= page.Shapes.Count; j++)
                {
                    Visio.Shape shape = page.Shapes[j];
                    Visio.Cell doubleClickCell = shape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowEvent, (short)Visio.VisCellIndices.visEvtCellDblClick);
                    doubleClickCell.FormulaU = string.Format(CultureInfo.InvariantCulture, VisioDoubleClick.CELLFORMULAFORMAT, shape.ID);
                }
            }

            axDrawingControl1.Document.Saved = true;
        }

        void Application_MarkerEvent(Visio.Application app, int SequenceNum, string ContextString)
        {
            if (this.parentForm == null)
            {//This shouldm't happen because we've detached the event handler in ClosControl (_application.MarkerEvent -= MarkerEventEventHandler;)
                return;//but just for safety given that it's an application event.
            }

            if (ContextString != null && ContextString.Contains(VisioDoubleClick.COMMANDSTRING))
            {
                string sourceTag = VisioDoubleClick.SOURCETAG;
                string shapeIndexString = ContextString[(ContextString.IndexOf(sourceTag) + sourceTag.Length)..];
                int shapeId = int.Parse(shapeIndexString, CultureInfo.InvariantCulture);

                selectedShape = _application.ActivePage.Shapes.get_ItemFromID(shapeId);
                EditShape();
            }
        }

        private void DocumentSavedAs_EventHandler(object sender, VisOCX.EVisOcx_DocumentSavedAsEvent e)
        {
            axDrawingControl1.Document.Saved = true;
        }

        private void ShapeAdded_EventHandler(object sender, VisOCX.EVisOcx_ShapeAddedEvent e)
        {
            ShapeAdded(e);
        }

        private void VisioControl_Load(object sender, EventArgs e)
        {
        }

        private static void AxDrawingControl1_MouseDownEvent(object sender, AxMicrosoft.Office.Interop.VisOcx.EVisOcx_MouseDownEvent e)
        {
        }

        void TitleBar1_CloseClick()
        {
            CloseControl();
        }

        void ParentForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            SaveAndReleaseDocument();
        }

        private void AxDrawingControl1_SelectionChanged(object sender, AxMicrosoft.Office.Interop.VisOcx.EVisOcx_SelectionChangedEvent e)
        {
            if (axDrawingControl1.Window.Selection.Count > 0)
            {
                selectedShape = axDrawingControl1.Window.Selection[1];
                this.parentForm.RadMenuItemIndexInformation.Enabled = true;

                Visio.Cell lockTextEditCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockTextEdit);
                Visio.Cell lockCustPropCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockCustProp);
                Visio.Cell lockMoveXCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockMoveX);
                Visio.Cell lockMoveYCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockMoveY);
                Visio.Cell lockHeightCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockHeight);
                Visio.Cell lockWidthCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockWidth);
                Visio.Cell lockBeginCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockBegin);
                Visio.Cell lockEndCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockEnd);
                Visio.Cell lockDeleteCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockDelete);
                Visio.Cell lockAspectCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockAspect);
                Visio.Cell lockRotateCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockRotate);
                Visio.Cell lockCropCell = selectedShape.get_CellsSRC((short)Visio.VisSectionIndices.visSectionObject, (short)Visio.VisRowIndices.visRowLock, (short)Visio.VisCellIndices.visLockCrop);

                if (selectedShape.Master.NameU == UniversalMasterName.COMMENT)
                {
                    lockTextEditCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                    lockCustPropCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                }
                else
                {
                    lockTextEditCell.FormulaU = PROPERTYLOCK;
                    lockCustPropCell.FormulaU = PROPERTYLOCK;
                }

                lockMoveXCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                lockMoveYCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                lockHeightCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                lockWidthCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                lockBeginCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                lockEndCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                lockDeleteCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                lockAspectCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                lockRotateCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
                lockCropCell.FormulaU = this.openedAsReadOnly ? PROPERTYLOCK : PROPERTYUNLOCK;
            }
            else
            {
                this.parentForm.RadMenuItemIndexInformation.Enabled = false;
                selectedShape = null;
            }
        }

        private static void AxDrawingControl1_MouseUpEvent(object sender, AxMicrosoft.Office.Interop.VisOcx.EVisOcx_MouseUpEvent e)
        {
        }

        private void AxDrawingControl1_DocumentOpened(object sender, VisOCX.EVisOcx_DocumentOpenedEvent e)
        {
            this.axDrawingControl1.Focus();
            OpenStencils();
        }
        #endregion Event Handlers
    }
}
