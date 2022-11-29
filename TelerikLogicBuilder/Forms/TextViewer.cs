using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Forms
{
    internal partial class TextViewer : RadForm, ITextViewer
    {
        private readonly IFormInitializer _formInitializer;
        public TextViewer(IFormInitializer formInitializer)
        {
            _formInitializer = formInitializer;
            InitializeComponent();
            Initialize();
        }

        public void SetText(string[] lines)
        {
            richTextBoxViewerPanel1.RichTextBox.Lines = lines;
        }

        public void SetText(string viewText)
        {
            richTextBoxViewerPanel1.RichTextBox.Text = viewText;
        }

        private void Initialize()
        {
            _formInitializer.SetFormDefaults(this, 600);
        }
    }
}
