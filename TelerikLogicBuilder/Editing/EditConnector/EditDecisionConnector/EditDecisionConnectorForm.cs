using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDecisionConnector
{
    internal partial class EditDecisionConnectorForm : Telerik.WinControls.UI.RadForm, IEditDecisionConnectorForm
    {
        private readonly IConnectorDataParser _connectorDataParser;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IFormInitializer _formInitializer;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public EditDecisionConnectorForm(
            IConnectorDataParser connectorDataParser,
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IFormInitializer formInitializer,
            IRadDropDownListHelper radDropDownListHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            short connectorIndexToSelect,
            XmlDocument? connectorXmlDocument)
        {
            InitializeComponent();
            _connectorDataParser = connectorDataParser;
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();
            _formInitializer = formInitializer;
            _radDropDownListHelper = radDropDownListHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            Initialize();
            UpdateSelection(connectorIndexToSelect, connectorXmlDocument);
            ValidateOk();
        }

        public string ShapeXml
        {
            get
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.CONNECTORELEMENT);
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, ((short)cmbIndex.SelectedValue).ToString(CultureInfo.InvariantCulture));
                    xmlTextWriter.WriteAttributeString(XmlDataConstants.CONNECTORCATEGORYATTRIBUTE, ((short)ConnectorCategory.Decision).ToString(CultureInfo.InvariantCulture));
                    xmlTextWriter.WriteStartElement(XmlDataConstants.TEXTELEMENT);
                    xmlTextWriter.WriteRaw(cmbIndex.SelectedItem.Text);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                }
                return stringBuilder.ToString();
            }
        }

        public string ShapeVisibleText => cmbIndex.SelectedItem.Text;

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

            this.ActiveControl = cmbIndex;
            this.AcceptButton = radButtonOk;

            LoadDropDownList();
            cmbIndex.SelectedIndexChanged += CmbIndex_SelectedIndexChanged;
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelCommandButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void InitializeGroupBoxPrompt()
        {
            ControlsLayoutUtility.LayoutSingleRowGroupBox(radPanelTop, radGroupBoxPrompt);
        }

        private void LoadDropDownList()
        {
            _radDropDownListHelper.LoadComboItems<DecisionOption>(cmbIndex);
        }

        private void UpdateSelection(short connectorIndexToSelect, XmlDocument? connectorXmlDocument)
        {
            cmbIndex.SelectedValue = Enum.IsDefined(typeof(DecisionOption), connectorIndexToSelect)
                    ? (DecisionOption)connectorIndexToSelect
                    : DecisionOption.Yes;

            if (connectorXmlDocument == null)
                return;

            ConnectorData connectorData = _connectorDataParser.Parse
            (
                _xmlDocumentHelpers.GetDocumentElement(connectorXmlDocument)
            );
            if (connectorData.ConnectorCategory != ConnectorCategory.Decision)
                return;

            cmbIndex.SelectedValue = (DecisionOption)connectorData.Index;
        }

        private void ValidateOk()
        {
            radButtonOk.Enabled = cmbIndex.SelectedIndex != -1;
        }

        #region Event Handlers
        private void CmbIndex_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            ValidateOk();
        }
        #endregion Event Handlers
    }
}
