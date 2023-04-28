using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditPriority
{
    internal partial class EditPriorityForm : Telerik.WinControls.UI.RadForm, IEditPriorityForm
    {
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IFormInitializer _formInitializer;
        private readonly IPriorityDataParser _priorityDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public EditPriorityForm(
            IDialogFormMessageControl dialogFormMessageControl,
            IFormInitializer formInitializer,
            IPriorityDataParser priorityDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            XmlDocument? priorityXmlDocument)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControl;
            _formInitializer = formInitializer;
            _priorityDataParser = priorityDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            Initialize();
            UpdatePriorityText(priorityXmlDocument);
            ValidateOk(false);
        }

        public string ShapeXml
        {
            get
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.SHAPEDATAELEMENT);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, TableColumnName.PRIORITYCOLUMN);
                        xmlTextWriter.WriteElementString(XmlDataConstants.VALUEELEMENT, ShapeVisibleText);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                }

                return stringBuilder.ToString();
            }
        }

        public string ShapeVisibleText => radTextBoxInput.Text.ToUpper(CultureInfo.CurrentCulture).Trim();

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            InitializeGroupBoxPrompt();

            radPanelFill.Size = new Size(radPanelFill.Width, 0);
            radPanelFill.Margin = new Padding(0);
            _formInitializer.SetFormDefaults
            (
                this,
                this.Size.Height - this.ClientSize.Height
                    + radPanelTop.Height
                    + radPanelBottom.Height
            );
            this.Size = new Size(this.Width, 0);

            ControlsLayoutUtility.CollapsePanelBorder(radPanelTop);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelFill);
            ControlsLayoutUtility.CollapsePanelBorder(radPanelBottom);

            radButtonOk.Enabled = false;
            radButtonOk.DialogResult = DialogResult.OK;
            radButtonCancel.DialogResult = DialogResult.Cancel;

            this.ActiveControl = radTextBoxInput;
            this.AcceptButton = radButtonOk;

            radTextBoxInput.TextChanged += RadTextBoxInput_TextChanged;
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelCommandButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeGroupBoxPrompt()
        {
            ControlsLayoutUtility.LayoutSingleRowGroupBox(radPanelTop, radGroupBoxPrompt);
        }

        private void SetPriorityText(int priority)
        {
            radTextBoxInput.TextChanged -= RadTextBoxInput_TextChanged;
            radTextBoxInput.Text = string.Format(CultureInfo.CurrentCulture, "{0}", priority);
            radTextBoxInput.TextChanged += RadTextBoxInput_TextChanged;
        }

        private void UpdatePriorityText(XmlDocument? priorityXmlDocument)
        {
            if (priorityXmlDocument == null)
                return;

            int? priority = _priorityDataParser.Parse
            (
                _xmlDocumentHelpers.GetDocumentElement(priorityXmlDocument)
            );

            if (!priority.HasValue)
                return;

            SetPriorityText(priority.Value);
        }

        private void ValidateOk(bool showErrors = true)
        {
            _dialogFormMessageControl.ClearMessage();
            if (int.TryParse(radTextBoxInput.Text.Trim(), out int testParse) && testParse > 0)
            {
                radButtonOk.Enabled = true;
            }
            else
            {
                radButtonOk.Enabled = false;
                if (showErrors)
                    _dialogFormMessageControl.SetErrorMessage(Strings.priorityMustBeGreaterThanZero);
            }
        }

        #region Event Handlers
        private void RadTextBoxInput_TextChanged(object? sender, EventArgs e) => ValidateOk();
        #endregion Event Handlers
    }
}
