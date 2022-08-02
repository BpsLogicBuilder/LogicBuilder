using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers;
using System;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls
{
    internal partial class Messages : UserControl, IMessages
    {
        private readonly IDiagramErrorSourceDataParser _diagramErrorSourceDataParser;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IOpenFileOperations _openFileOperations;
        private readonly ITableErrorSourceDataParser _tableErrorSourceDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly UiNotificationService _uiNotificationService;
        private readonly RichInputBoxMessagePanel _richInputBoxMessagePanelDocuments;
        private readonly RichInputBoxMessagePanel _richInputBoxMessagePanelRules;
        private readonly RichInputBoxMessagePanel _richInputBoxMessagePanelMessages;
        private readonly RichInputBoxMessagePanel _richInputBoxMessagePanelPageSearchResults;


        public Messages(
            IDiagramErrorSourceDataParser diagramErrorSourceDataParser,
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            IOpenFileOperations openFileOperations,
            ITableErrorSourceDataParser tableErrorSourceDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            UiNotificationService uiNotificationService,
            RichInputBoxMessagePanel richInputBoxMessagePanelDocuments,
            RichInputBoxMessagePanel richInputBoxMessagePanelRules,
            RichInputBoxMessagePanel richInputBoxMessagePanelMessages,
            RichInputBoxMessagePanel richInputBoxMessagePanelPageSearchResults)
        {
            _diagramErrorSourceDataParser = diagramErrorSourceDataParser;
            _exceptionHelper = exceptionHelper;
            _tableErrorSourceDataParser = tableErrorSourceDataParser;
            _mainWindow = mainWindow;
            _openFileOperations = openFileOperations;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            _uiNotificationService = uiNotificationService;
            _richInputBoxMessagePanelDocuments = richInputBoxMessagePanelDocuments;
            _richInputBoxMessagePanelRules = richInputBoxMessagePanelRules;
            _richInputBoxMessagePanelMessages = richInputBoxMessagePanelMessages;
            _richInputBoxMessagePanelPageSearchResults = richInputBoxMessagePanelPageSearchResults;
            InitializeComponent();
            Initialize();
            InitializeEventHandlers();
        }

        public MessageTab SelectedMessageTab
        {
            set
            {
                if (!Enum.IsDefined(typeof(MessageTab), value))
                    throw _exceptionHelper.CriticalException("{67160345-9C22-42DA-A041-49857FF584E6}");

                radPageView1.SelectedPage = radPageView1.Pages[(int)value];
            }
        }

        bool IMessages.Visible
        {
            set 
            {
                ChangeVisibility(value);
            }
        }

        public void Clear(MessageTab messageTab) 
            => GetRichInputBox(messageTab).Clear();

        public void GoToNextEmptyLine(MessageTab messageTab)
        {
            RichInputBox richInputBox = GetRichInputBox(messageTab);
            if (richInputBox.Lines.Length != 0 && richInputBox.Lines[^1].Length != 0)
            {
                richInputBox.Select(richInputBox.TextLength, 0);
                richInputBox.InsertText(Environment.NewLine);
            }
            else
            {
                richInputBox.Select(richInputBox.TextLength, 0);
            }
        }

        public void InsertLink(string text, string hyperlink, LinkType linkType, MessageTab messageTab) 
            => InsertLink(text, hyperlink, GetRichInputBox(messageTab).SelectionStart, linkType, messageTab);

        public void InsertLink(string text, string hyperlink, int position, LinkType linkType, MessageTab messageTab) 
            => GetRichInputBox(messageTab).InsertLink(text, hyperlink, position, linkType);

        public void InsertText(string text, MessageTab messageTab) 
            => GetRichInputBox(messageTab).InsertText(text);

        public void InsertText(string text, int position, MessageTab messageTab) 
            => GetRichInputBox(messageTab).InsertText(text, position);

        public void Select(int start, int length, MessageTab messageTab)
        {
            RichInputBox richInputBox = GetRichInputBox(messageTab);
            richInputBox.Select(start, length);
            richInputBox.Focus();
        }

        private void ChangeVisibility(bool isVisible)
        {
            IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
            mdiParent.SplitPanelMessages.Collapsed = !isVisible;
            base.Visible = isVisible;
        }

        private void FindCell(string xmlString)
        {
            try
            {
                TableErrorSourceData cellDetails = _tableErrorSourceDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(xmlString)
                );
                _openFileOperations.OpenVisioFile(cellDetails.FileFullName);

                IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
                if (mdiParent.EditControl is ITableControl tableControl)
                {//mdiParent.EditControl could be null if user cancels in _openFileOperations.OpenTableFile
                    tableControl.FindCell
                    (
                        cellDetails.RowIndex,
                        cellDetails.ColumnIndex
                    );
                }
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private void FindShape(string xmlString)
        {
            try
            {
                DiagramErrorSourceData shapeDetails = _diagramErrorSourceDataParser.Parse
                (
                    _xmlDocumentHelpers.ToXmlElement(xmlString)
                );
                _openFileOperations.OpenVisioFile(shapeDetails.FileFullName);

                IMDIParent mdiParent = (IMDIParent)_mainWindow.Instance;
                if (mdiParent.EditControl is IDrawingControl visioControl)
                {//mdiParent.EditControl could be null if user cancels in _openFileOperations.OpenVisioFile
                    visioControl.FindShape
                    (
                        shapeDetails.PageIndex, 
                        shapeDetails.ShapeIndex, 
                        shapeDetails.PageId, 
                        shapeDetails.ShapeId
                    );
                }
            }
            catch (LogicBuilderException ex)
            {
                _uiNotificationService.NotifyLogicBuilderException(ex);
            }
        }

        private RichInputBox GetRichInputBox(MessageTab messageTab)
        {
            return messageTab switch
            {
                MessageTab.Documents => _richInputBoxMessagePanelDocuments.RichInputBox,
                MessageTab.Rules => _richInputBoxMessagePanelRules.RichInputBox,
                MessageTab.Messages => _richInputBoxMessagePanelMessages.RichInputBox,
                MessageTab.PageSearchResults => _richInputBoxMessagePanelPageSearchResults.RichInputBox,
                _ => throw _exceptionHelper.CriticalException("{15A5AF2B-90E5-4D7E-906F-E8966D24DDB5}"),
            };
        }

        private void Initialize()
        {
            this.radPageView1.SuspendLayout();
            this.radPageViewDocuments.SuspendLayout();
            this.radPageViewRules.SuspendLayout();
            this.radPageViewMessages.SuspendLayout();
            this.radPageViewPageSearchResults.SuspendLayout();

            _richInputBoxMessagePanelDocuments.Dock = DockStyle.Fill;
            _richInputBoxMessagePanelRules.Dock = DockStyle.Fill;
            _richInputBoxMessagePanelMessages.Dock = DockStyle.Fill;
            _richInputBoxMessagePanelPageSearchResults.Dock = DockStyle.Fill;

            this.radPageViewDocuments.Controls.Add(_richInputBoxMessagePanelDocuments);
            this.radPageViewRules.Controls.Add(_richInputBoxMessagePanelRules);
            this.radPageViewMessages.Controls.Add(_richInputBoxMessagePanelMessages);
            this.radPageViewPageSearchResults.Controls.Add(_richInputBoxMessagePanelPageSearchResults);

            this.radPageViewDocuments.ResumeLayout(false);
            this.radPageViewRules.ResumeLayout(false);
            this.radPageViewMessages.ResumeLayout(false);
            this.radPageViewPageSearchResults.ResumeLayout(false);

            this.radPageView1.ResumeLayout(false);
        }

        private void InitializeEventHandlers()
        {
            this.titleBar1.CloseClick += TitleBar1_CloseClick;
            _richInputBoxMessagePanelDocuments.RichInputBox.MouseClick += RichInputBox_MouseClick;
            _richInputBoxMessagePanelPageSearchResults.RichInputBox.MouseClick += RichInputBox_MouseClick;
        }

        #region Event Handlers
        private void RichInputBox_MouseClick(object? sender, MouseEventArgs e)
        {
            if (sender is not RichInputBox richInputBox)
                throw _exceptionHelper.CriticalException("{46048064-1960-4AC9-846C-D2BEA736A392}");

            int charIndex = richInputBox.GetCharIndexFromPosition(e.Location);
            LinkBoundaries? boundary = richInputBox.GetBoundary(charIndex);

            if (boundary == null)
                return;

            string xmlString = richInputBox.GetHiddenLinkText(charIndex);
            richInputBox.Select(boundary.Start, boundary.Finish - boundary.Start + 1);

            switch (_xmlDocumentHelpers.ToXmlElement(xmlString).Name)
            {
                case XmlDataConstants.DIAGRAMERRORSOURCE:
                    FindShape(xmlString);
                    break;
                case XmlDataConstants.TABLEERRORSOURCE:
                    FindCell(xmlString);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{2A44F145-C766-4849-AE27-5DE6BDC5775E}");
            }
        }

        private void TitleBar1_CloseClick()
        {
            ChangeVisibility(false);
        }
        #endregion Event Handlers
    }
}
