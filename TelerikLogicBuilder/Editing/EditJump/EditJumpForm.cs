using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditJump
{
    internal partial class EditJumpForm : Telerik.WinControls.UI.RadForm, IEditJumpForm
    {
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IFormInitializer _formInitializer;
        private readonly IJumpDataParser _jumpDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public EditJumpForm(
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IFormInitializer formInitializer,
            IJumpDataParser jumpDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            XmlDocument? jumpXmlDocument)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();
            _formInitializer = formInitializer;
            _jumpDataParser = jumpDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            Initialize();
            UpdateJumpText(jumpXmlDocument);
        }

        public string ShapeXml
        {
            get
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.SHAPEDATAELEMENT);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, UniversalMasterName.JUMPOBJECT);
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

        private void UpdateJumpText(XmlDocument? jumpXmlDocument)
        {
            if (jumpXmlDocument == null)
                return;

            string jumpText = _jumpDataParser.Parse
            (
                _xmlDocumentHelpers.GetDocumentElement(jumpXmlDocument)
            );

            if (string.IsNullOrEmpty(jumpText))
                return;

            radTextBoxInput.Text = jumpText;
        }

        private void ValidateOk()
        {
            radButtonOk.Enabled = radTextBoxInput.Text.Trim().Length > 0;
        }

        #region Event Handlers
        private void RadTextBoxInput_TextChanged(object? sender, EventArgs e) => ValidateOk();
        #endregion Event Handlers
    }
}
