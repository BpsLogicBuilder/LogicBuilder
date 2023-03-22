using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using Microsoft.Office.Interop.Visio;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Forms
{
    internal partial class FindShape : Telerik.WinControls.UI.RadForm, IFindShape
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFormInitializer _formInitializer;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private Document? visioDocument;

        public FindShape(
            IExceptionHelper exceptionHelper,
            IFormInitializer formInitializer,
            IDialogFormMessageControl dialogFormMessageControl)
        {
            _exceptionHelper = exceptionHelper;
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControl;
            InitializeComponent();
            InitializeDialogFormMessageControl();
            Initialize();
        }

        #region Properties
        internal int PageIndex
            => int.Parse(radTextBoxPageIndex.Text.Trim(), CultureInfo.CurrentCulture);

        internal int ShapeIndex
            => int.Parse(radTextBoxShapeIndex.Text.Trim(), CultureInfo.CurrentCulture);
        #endregion Properties

        #region Methods
        public void Setup(Document visioDocument)
        {
            this.visioDocument = visioDocument;
        }

        private void Find(int pageIndex, int shapeIndex)
        {
            if (this.visioDocument == null)
                return;

            Shape shape = this.visioDocument.Pages[pageIndex].Shapes[shapeIndex];
            double xCoordinate = shape.get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowXFormOut, (short)VisCellIndices.visXFormPinX).ResultIU;
            double yCoordinate = shape.get_CellsSRC((short)VisSectionIndices.visSectionObject, (short)VisRowIndices.visRowXFormOut, (short)VisCellIndices.visXFormPinY).ResultIU;
            visioDocument.Application.ActiveWindow.Page = visioDocument.Pages[pageIndex];
            visioDocument.Application.ActiveWindow.Select(shape, (short)VisSelectArgs.visSelect);
            visioDocument.Application.ActiveWindow.ScrollViewTo(xCoordinate, yCoordinate);
        }

        private void Initialize()
        {
            this.AcceptButton = radButtonFind;
            _formInitializer.SetFormDefaults(this, PerFontSizeConstants.FindShapeOrCellFormMinimumHeight);
            this.radButtonCancel.DialogResult = DialogResult.Cancel;
            ValidateOk();
        }

        private void InitializeDialogFormMessageControl()
        {
            this.SuspendLayout();
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelCommandButtons, tableLayoutPanelButtons, _dialogFormMessageControl, false);
            this.ResumeLayout(true);
        }

        private void ValidateOk()
        {
            if (radTextBoxShapeIndex.Text.Trim().Length > 0 && !TryParse(radTextBoxShapeIndex.Text, out _))
            {
                _dialogFormMessageControl.SetErrorMessage(Strings.integersOnly);
                radButtonFind.Enabled = false;
            }
            else if (radTextBoxPageIndex.Text.Trim().Length > 0 && !TryParse(radTextBoxPageIndex.Text, out _))
            {
                _dialogFormMessageControl.SetErrorMessage(Strings.integersOnly);
                radButtonFind.Enabled = false;
            }
            else if (TryParse(radTextBoxShapeIndex.Text, out _) && TryParse(radTextBoxPageIndex.Text, out _))
            {
                _dialogFormMessageControl.ClearMessage();
                radButtonFind.Enabled = true;
            }
            else if (radTextBoxShapeIndex.Text.Trim().Length == 0 || radTextBoxPageIndex.Text.Trim().Length == 0)
            {
                _dialogFormMessageControl.ClearMessage();
                radButtonFind.Enabled = false;
            }
            else
            {
                throw _exceptionHelper.CriticalException("{2B4EF6B8-DB45-4642-B9FD-84A4D3ADAE02}");
            }

            if (this.visioDocument == null)
                return;

            if (!TryParse(radTextBoxShapeIndex.Text, out int shapeIndex) || !TryParse(radTextBoxPageIndex.Text, out int pageIndex))
                return;

            if (visioDocument.Pages.Count < pageIndex)
            {
                _dialogFormMessageControl.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.pageIndexIsInvalidFormat, pageIndex));
                radButtonFind.Enabled = false;
                return;
            }

            if (visioDocument.Pages[pageIndex].Shapes.Count < shapeIndex)
            {
                _dialogFormMessageControl.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.shapeIndexIsInvalidFormat, shapeIndex, pageIndex));
                radButtonFind.Enabled = false;
                return;
            }
        }

        private static bool TryParse(string text, out int number)
            => int.TryParse(text.Trim(), NumberStyles.Integer, CultureInfo.CurrentCulture, out number);
        #endregion Methods

        #region Event Handlers
        private void RadButtonFind_Click(object sender, EventArgs e)
        {
            Find(PageIndex, ShapeIndex);
        }

        private void RadButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void RadTextBoxPageIndex_TextChanged(object sender, EventArgs e)
        {
            ValidateOk();
        }

        private void RadTextBoxShapeIndex_TextChanged(object sender, EventArgs e)
        {
            ValidateOk();
        }
        #endregion Event Handlers
    }
}
