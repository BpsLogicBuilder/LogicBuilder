using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System.Globalization;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.IndexInformation
{
    internal partial class IndexInformationForm : Telerik.WinControls.UI.RadForm, IIndexInformationForm
    {
        private readonly IFormInitializer _formInitializer;

        public IndexInformationForm(IFormInitializer formInitializer)
        {
            InitializeComponent();
            _formInitializer = formInitializer;
            Initialize();
        }

        public void SetIndexes(int pageIndex, int shapeIndex)
        {
            txtPageIndex.Text = pageIndex.ToString(CultureInfo.CurrentCulture);
            txtShapeIndex.Text = shapeIndex.ToString(CultureInfo.CurrentCulture);
        }

        private void Initialize()
        {
            InitializeTableLayoutPanel();

            txtPageIndex.ReadOnly = true;
            txtShapeIndex.ReadOnly = true;

            Padding groupBoxPadding = PerFontSizeConstants.GroupBoxPadding;
            _formInitializer.SetFormDefaults
            (
                this,
                this.Size.Height - this.ClientSize.Height
                    + groupBoxPadding.Top
                    + groupBoxPadding.Bottom
                    + (int)(2 * PerFontSizeConstants.BoundarySize)
                    + (int)(2 * PerFontSizeConstants.SeparatorLineHeight)
                    + (int)(2 * PerFontSizeConstants.SingleLineHeight)
                    + (int)PerFontSizeConstants.BottomPanelHeight
            );
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons);

            ControlsLayoutUtility.CollapsePanelBorder(radPanelTableParent);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelValues);

            btnClose.CausesValidation = false;
            btnClose.DialogResult = DialogResult.Cancel;
        }

        private void InitializeTableLayoutPanel()
        {
            ControlsLayoutUtility.LayoutControls
            (
                radGroupBoxValues,
                radPanelValues,
                radPanelTableParent,
                tableLayoutPanel,
                2
            );
        }
    }
}
