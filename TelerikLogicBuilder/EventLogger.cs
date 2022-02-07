using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder
{
    internal static class EventLogger
    {
        #region Constants
        private const int MAXMESSAGELENGTH = 32765;
        #endregion Constants

        /// <summary>
        /// Writes an error message and stack trace to the event log given the source and exception
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ex"></param>
        internal static void WriteEntry(string source, Exception ex)
        {
            WriteEntry(source, ex, EventLogEntryType.Error);
        }

        /// <summary>
        /// Writes an error message and stack trace to the event log given the source, exception and entry type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ex"></param>
        /// <param name="eventLogEntryType"></param>
        internal static void WriteEntry(string source, Exception ex, EventLogEntryType eventLogEntryType)
        {
            string message = ex.ToString();
            if (message.Length > MAXMESSAGELENGTH)
                message = message[..MAXMESSAGELENGTH];

            try
            {
#if DEBUG
                EventLog.WriteEntry(source, message, eventLogEntryType);
#endif
            }
            catch (System.ComponentModel.InvalidEnumArgumentException e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (ArgumentException e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (InvalidOperationException e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (System.Security.SecurityException e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (Exception e)
            {
                DisplayMessage.Show(string.Format(CultureInfo.CurrentCulture, Strings.unhandledEventLoggerExceptionFormat, e.GetType().ToString(), e.Message), (MessageBoxOptions)0);
                throw;
            }
        }

        /// <summary>
        /// Writes an error message and stack trace to the event log given the source, message and entry type
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ex"></param>
        /// <param name="eventLogEntryType"></param>
        internal static void WriteEntry(string source, string message, EventLogEntryType eventLogEntryType)
        {
            if (message.Length > MAXMESSAGELENGTH)
                message = message[..MAXMESSAGELENGTH];

            try
            {
#if DEBUG
                EventLog.WriteEntry(source, message, eventLogEntryType);
#endif
            }
            catch (System.ComponentModel.InvalidEnumArgumentException e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (ArgumentException e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (InvalidOperationException e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (System.Security.SecurityException e)
            {
                DisplayMessage.Show(e.Message, (MessageBoxOptions)0);
            }
            catch (Exception e)
            {
                DisplayMessage.Show(string.Format(CultureInfo.CurrentCulture, Strings.unhandledEventLoggerExceptionFormat, e.GetType().ToString(), e.Message), (MessageBoxOptions)0);
                throw;
            }
        }
    }
}
