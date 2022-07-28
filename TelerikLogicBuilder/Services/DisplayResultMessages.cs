using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Services
{
    internal class DisplayResultMessages : IDisplayResultMessages
    {
        private readonly IMainWindow _mainWindow;

        public DisplayResultMessages(IMainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        private Form? _mainWindowInstance;
        private Form MainWindowInstance
        {
            get
            {
                if (_mainWindowInstance == null)
                {//_mainWindow.Instance may be unavailable in the constructor depending on when the container initializes DisplayResultMessages
                    _mainWindowInstance = _mainWindow.Instance;
                    _mainWindowInstance.Disposed += MDIParent_Disposed;
                }

                return _mainWindowInstance;
            }
        }

        public void AppendMessage(ResultMessage message, MessageTab messageTab)
        {
            IMessages messagePanel = ((IMDIParent)MainWindowInstance).Messages;
            messagePanel.GoToNextEmptyLine(messageTab);
            if (string.IsNullOrEmpty(message.LinkHiddenText))
            {
                messagePanel.InsertText(message.Message, messageTab);
            }
            else
            {
                messagePanel.InsertLink(message.LinkVisibleText, message.LinkHiddenText, LinkType.Function, messageTab);
                messagePanel.InsertText(Strings.spaceString, messageTab);
                messagePanel.InsertText(message.Message, messageTab);
            }

            messagePanel.InsertText(Environment.NewLine, messageTab);
            messagePanel.SelectedMessageTab = messageTab;
            messagePanel.Visible = true;
        }

        public void Clear(MessageTab messageTab)
        {
            IMessages messagePanel = ((IMDIParent)MainWindowInstance).Messages;
            messagePanel.Clear(messageTab);
        }

        public void DisplayMessages(IList<ResultMessage> results, MessageTab messageTab)
        {
            IMessages messagePanel = ((IMDIParent)MainWindowInstance).Messages;
            messagePanel.Clear(messageTab);
            foreach (ResultMessage message in results)
            {
                if (string.IsNullOrEmpty(message.LinkHiddenText))
                    messagePanel.InsertText(message.Message, messageTab);
                else
                {
                    messagePanel.InsertLink(message.LinkVisibleText, message.LinkHiddenText, LinkType.Function, messageTab);
                    messagePanel.InsertText(Strings.spaceString, messageTab);
                    messagePanel.InsertText(message.Message, messageTab);
                }

                messagePanel.InsertText(Environment.NewLine, messageTab);
            }
            messagePanel.Select(0, 0, messageTab);
            messagePanel.SelectedMessageTab = messageTab;
            messagePanel.Visible = true;
        }

        public void DisplayMessages(IList<string> results, MessageTab messageTab)
        {
            IMessages messagePanel = ((IMDIParent)MainWindowInstance).Messages;
            messagePanel.Clear(messageTab);
            foreach (string message in results)
            {
                messagePanel.InsertText(message, messageTab);
                messagePanel.InsertText(Environment.NewLine, messageTab);
            }
            messagePanel.Select(0, 0, messageTab);
            messagePanel.SelectedMessageTab = messageTab;
            ((Control)messagePanel).Visible = true;
        }

        #region Event Handlers
        private void MDIParent_Disposed(object? sender, EventArgs e)
        {
        }
        #endregion Event Handlers
    }
}
