using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Structures;
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
                throw new ArgumentOutOfRangeException(nameof(message), message);
                //using (TextViewer textViewer = new TextViewer(message))
                //{
                //    textViewer.Text = caption;
                //    textViewer.StartPosition = FormStartPosition.CenterParent;
                //    textViewer.ShowDialog();
                //    return textViewer.DialogResult;
                //}
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
                throw new ArgumentOutOfRangeException(nameof(message), message);
                //using (TextViewer textViewer = new TextViewer(message))
                //{
                //    textViewer.Text = caption;
                //    textViewer.StartPosition = FormStartPosition.CenterParent;
                //    textViewer.ShowDialog();
                //    return textViewer.DialogResult;
                //}
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

        internal static DialogResult ShowQuestion(IWin32Window parent, string message, RightToLeft rightToLeft = RightToLeft.No)
            => ShowQuestion(parent, message, ApplicationProperties.Name, rightToLeft);

        internal static DialogResult ShowQuestion(IWin32Window parent, string message, string caption, RightToLeft rightToLeft = RightToLeft.No)
        {
            message = message.Length > MiscellaneousConstants.MESSAGEBOXLENGTH
                ? string.Format(CultureInfo.CurrentCulture, Strings.truncatedMessageTextFormat, message[..(MiscellaneousConstants.MESSAGEBOXLENGTH - 1)])
                : message;

            return RadMessageBox.Show(parent, message, caption, MessageBoxButtons.OKCancel, RadMessageIcon.Question, MessageBoxDefaultButton.Button1, rightToLeft);
        }
        #endregion Methods
    }
}
