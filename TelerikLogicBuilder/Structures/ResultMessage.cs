namespace ABIS.LogicBuilder.FlowBuilder.Structures
{
    internal class ResultMessage
    {

        public ResultMessage(string linkHiddenText, string linkVisibleText, string message)
        {
            LinkHiddenText = linkHiddenText;
            LinkVisibleText = linkVisibleText;
            Message = message;
        }

        public ResultMessage(string message)
        {
            Message = message;
            LinkHiddenText = string.Empty;
            LinkVisibleText = string.Empty;
        }

        #region Properties
        /// <summary>
        /// Hidden XML containing error source
        /// </summary>
        internal string LinkHiddenText { get; }

        /// <summary>
        /// Visble Link Text displaying the error source
        /// </summary>
        internal string LinkVisibleText { get; }

        /// <summary>
        /// Explanation of the error
        /// </summary>
        internal string Message { get; }
        #endregion Properties
    }
}
