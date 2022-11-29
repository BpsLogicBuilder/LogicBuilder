using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Windows.Forms;
using Telerik.WinControls;

namespace ABIS.LogicBuilder.FlowBuilder.Prompts
{
    internal static class DisplayMessage
    {
        #region Methods
        internal static DialogResult Show(string message)
        {
            return Show(message, ApplicationProperties.Name);
        }

        internal static DialogResult Show(string message, string caption)
        {
            if (message.Length < MiscellaneousConstants.MESSAGEBOXLENGTH)
            {
                return RadMessageBox.Show(message, caption, MessageBoxButtons.OK, RadMessageIcon.None, MessageBoxDefaultButton.Button1);
            }
            else
            {
                using IScopedDisposableManager<ITextViewer> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<ITextViewer>>();
                ITextViewer textViewer = disposableManager.ScopedService;
                textViewer.Text = caption;
                textViewer.SetText(message);
                textViewer.StartPosition = FormStartPosition.CenterParent;
                textViewer.ShowDialog();
                return textViewer.DialogResult;
            }
        }

        internal static DialogResult Show(IWin32Window parent, string message, RightToLeft rightToLeft = RightToLeft.No)
        {
            return Show(parent, message, ApplicationProperties.Name, rightToLeft);
        }

        internal static DialogResult Show(IWin32Window parent, string message, string caption, RightToLeft rightToLeft = RightToLeft.No)
        {
            if (message.Length < MiscellaneousConstants.MESSAGEBOXLENGTH)
            {
                return RadMessageBox.Show(parent, message, caption, MessageBoxButtons.OK, RadMessageIcon.None, MessageBoxDefaultButton.Button1, rightToLeft);
            }
            else
            {
                using IScopedDisposableManager<ITextViewer> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<ITextViewer>>();
                ITextViewer textViewer = disposableManager.ScopedService;
                textViewer.Text = caption;
                textViewer.SetText(message);
                textViewer.StartPosition = FormStartPosition.CenterParent;
                textViewer.ShowDialog(parent);
                return textViewer.DialogResult;
            }
        }

        internal static DialogResult ShowQuestion(string message) 
            => ShowQuestion(message, ApplicationProperties.Name);

        internal static DialogResult ShowQuestion(string message, string caption)
        {
            message = message.Length > MiscellaneousConstants.MESSAGEBOXLENGTH
                ? string.Format(CultureInfo.CurrentCulture, Strings.truncatedMessageTextFormat, message[..(MiscellaneousConstants.MESSAGEBOXLENGTH - 1)])
                : message;

            return RadMessageBox.Show(message, caption, MessageBoxButtons.OKCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button1);
        }

        internal static DialogResult ShowQuestion(IWin32Window parent, string message, RightToLeft rightToLeft = RightToLeft.No, RadMessageIcon icon = RadMessageIcon.Question)
            => ShowQuestion(parent, message, ApplicationProperties.Name, rightToLeft, icon);

        internal static DialogResult ShowQuestion(IWin32Window parent, string message, string caption, RightToLeft rightToLeft = RightToLeft.No, RadMessageIcon icon = RadMessageIcon.Question)
        {
            message = message.Length > MiscellaneousConstants.MESSAGEBOXLENGTH
                ? string.Format(CultureInfo.CurrentCulture, Strings.truncatedMessageTextFormat, message[..(MiscellaneousConstants.MESSAGEBOXLENGTH - 1)])
                : message;

            return RadMessageBox.Show(parent, message, caption, MessageBoxButtons.OKCancel, icon, MessageBoxDefaultButton.Button1, rightToLeft);
        }

        internal static DialogResult ShowYesNo(string message)
            => ShowYesNo(message, ApplicationProperties.Name);

        internal static DialogResult ShowYesNo(string message, string caption)
        {
            message = message.Length > MiscellaneousConstants.MESSAGEBOXLENGTH
                ? string.Format(CultureInfo.CurrentCulture, Strings.truncatedMessageTextFormat, message[..(MiscellaneousConstants.MESSAGEBOXLENGTH - 1)])
                : message;

            return RadMessageBox.Show(message, caption, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button1);
        }

        internal static DialogResult ShowYesNo(IWin32Window parent, string message, RightToLeft rightToLeft = RightToLeft.No)
            => ShowYesNo(parent, message, ApplicationProperties.Name, rightToLeft);

        internal static DialogResult ShowYesNo(IWin32Window parent, string message, string caption, RightToLeft rightToLeft = RightToLeft.No)
        {
            message = message.Length > MiscellaneousConstants.MESSAGEBOXLENGTH
                ? string.Format(CultureInfo.CurrentCulture, Strings.truncatedMessageTextFormat, message[..(MiscellaneousConstants.MESSAGEBOXLENGTH - 1)])
                : message;

            return RadMessageBox.Show(parent, message, caption, MessageBoxButtons.YesNo, RadMessageIcon.Question, MessageBoxDefaultButton.Button1, rightToLeft);
        }

        internal static DialogResult ShowYesNoCancel(string message)
            => ShowYesNoCancel(message, ApplicationProperties.Name);

        internal static DialogResult ShowYesNoCancel(string message, string caption)
        {
            message = message.Length > MiscellaneousConstants.MESSAGEBOXLENGTH
                ? string.Format(CultureInfo.CurrentCulture, Strings.truncatedMessageTextFormat, message[..(MiscellaneousConstants.MESSAGEBOXLENGTH - 1)])
                : message;

            return RadMessageBox.Show(message, caption, MessageBoxButtons.YesNoCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button1);
        }

        internal static DialogResult ShowYesNoCancel(IWin32Window parent, string message, RightToLeft rightToLeft = RightToLeft.No)
            => ShowYesNoCancel(parent, message, ApplicationProperties.Name, rightToLeft);

        internal static DialogResult ShowYesNoCancel(IWin32Window parent, string message, string caption, RightToLeft rightToLeft = RightToLeft.No)
        {
            message = message.Length > MiscellaneousConstants.MESSAGEBOXLENGTH
                ? string.Format(CultureInfo.CurrentCulture, Strings.truncatedMessageTextFormat, message[..(MiscellaneousConstants.MESSAGEBOXLENGTH - 1)])
                : message;

            return RadMessageBox.Show(parent, message, caption, MessageBoxButtons.YesNoCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button1, rightToLeft);
        }
        #endregion Methods
    }
}
