using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Prompts
{
    internal static class DisplayMessage
    {
        #region Methods
        internal static DialogResult Show(string message, MessageBoxOptions MessageBoxOptions)
        {
            return Show(message, ApplicationProperties.Name, MessageBoxOptions);
        }

        internal static DialogResult Show(string message, string caption, MessageBoxOptions MessageBoxOptions)
        {
            if (message.Length < MiscellaneousConstants.MESSAGEBOXLENGTH)
            {
                return MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1, MessageBoxOptions);
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

        internal static DialogResult ShowQuestion(string message, MessageBoxOptions MessageBoxOptions) 
            => ShowQuestion(message, ApplicationProperties.Name, MessageBoxOptions);

        internal static DialogResult ShowQuestion(string message, string caption, MessageBoxOptions MessageBoxOptions)
        {
            message = message.Length > MiscellaneousConstants.MESSAGEBOXLENGTH
                ? string.Format(CultureInfo.CurrentCulture, Strings.truncatedMessageTextFormat, message[..(MiscellaneousConstants.MESSAGEBOXLENGTH - 1)])
                : message;

            return MessageBox.Show(message, caption, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions);
        }
        #endregion Methods
    }
}
