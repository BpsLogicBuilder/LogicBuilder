using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
{
    internal partial class TextViewer : RadForm
    {
        private readonly IFormInitializer _formInitializer;
        public TextViewer(IFormInitializer formInitializer)
        {
            _formInitializer = formInitializer;
            InitializeComponent();
            Initialize();
        }

        internal void SetText(string[] lines)
        {
            richTextBoxViewerPanel1.RichTextBox.Lines = lines;
        }

        internal void SetText(string viewText)
        {
            richTextBoxViewerPanel1.RichTextBox.Text = viewText;
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 600);
        }
    }
}
