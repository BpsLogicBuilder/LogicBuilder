using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.UserControls;
using ABIS.LogicBuilder.FlowBuilder.UserControls.DialogFormMessageControlHelpers.Factories;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector.EditDialogConnector
{
    internal partial class EditDialogConnectorForm : Telerik.WinControls.UI.RadForm, IEditDialogConnectorForm
    {
        private readonly IApplicationDropDownList _applicationDropDownList;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;
        private readonly IEditDialogConnectorControl _editDialogConnectorControl;
        private readonly IFormInitializer _formInitializer;

        private ApplicationTypeInfo _application;

        public EditDialogConnectorForm(
            IDialogFormMessageControlFactory dialogFormMessageControlFactory,
            IEditDialogConnectorControlFactory editDialogConnectorControlFactory,
            IFormInitializer formInitializer,
            IServiceFactory serviceFactory,
            short connectorIndexToSelect,
            XmlDocument? connectorXmlDocument)
        {
            InitializeComponent();
            _dialogFormMessageControl = dialogFormMessageControlFactory.GetDialogFormMessageControl();//_applicationDropDownList may try to set messages so do this first
            _applicationDropDownList = serviceFactory.GetApplicationDropDownList(this);
            _application = _applicationDropDownList.Application;
            _editDialogConnectorControl = editDialogConnectorControlFactory.GetEditDialogConnectorControl(this, connectorIndexToSelect, connectorXmlDocument);
            _formInitializer = formInitializer;
            Initialize();
        }

        public RadButton RadButtonOk => btnOk;

        public string ShapeXml => _editDialogConnectorControl.XmlResult.OuterXml;

        public string ShapeVisibleText => _editDialogConnectorControl.VisibleText;

        public ApplicationTypeInfo Application => _application;

        public event EventHandler<ApplicationChangedEventArgs>? ApplicationChanged;

        public void ClearMessage()
        {
            _dialogFormMessageControl.ClearMessage();
        }

        public void SetErrorMessage(string message)
        {
            _dialogFormMessageControl.SetErrorMessage(message);
        }

        public void SetMessage(string message, string title = "")
        {
            _dialogFormMessageControl.SetMessage(message, title);
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            CollapsePanelBorder(radPanelApplication);
            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFill);
            CollapsePanelBorder(radPanelMessages);

            InitializeDialogFormMessageControl();
            InitializeApplicationDropDownList();
            InitializeEditDialogConnectorControl();

            _applicationDropDownList.ApplicationChanged += ApplicationDropDownList_ApplicationChanged;
            this.FormClosing += EditDialogConnectorForm_FormClosing;

            Padding groupBoxPadding = PerFontSizeConstants.GroupBoxPadding;
            _formInitializer.SetFormDefaults
            (
                this,
                this.Size.Height - this.ClientSize.Height
                    + groupBoxPadding.Top
                    + groupBoxPadding.Bottom
                    + PerFontSizeConstants.ApplicationGroupBoxHeight
                    + (int)(2 * PerFontSizeConstants.BoundarySize)
                    + (int)(4 * PerFontSizeConstants.SeparatorLineHeight)
                    + (int)(4 * PerFontSizeConstants.SingleLineHeight)
                    + (int)PerFontSizeConstants.BottomPanelHeight
            );
            btnCancel.CausesValidation = false;
            btnOk.DialogResult = DialogResult.OK;
            btnCancel.DialogResult = DialogResult.Cancel;
        }

        private void InitializeApplicationDropDownList()
        {
            ControlsLayoutUtility.LayoutApplicationGroupBox(this, radPanelApplication, radGroupBoxApplication, _applicationDropDownList);
        }

        private void InitializeEditDialogConnectorControl()
        {
            ((ISupportInitialize)this.radPanelFill).BeginInit();
            this.radPanelFill.SuspendLayout();

            _editDialogConnectorControl.Dock = DockStyle.Fill;
            _editDialogConnectorControl.Location = new Point(0, 0);
            this.radPanelFill.Controls.Add((Control)_editDialogConnectorControl);

            ((ISupportInitialize)this.radPanelFill).EndInit();
            this.radPanelFill.ResumeLayout(true);
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        #region Event Handlers
        private void ApplicationDropDownList_ApplicationChanged(object? sender, ApplicationChangedEventArgs e)
        {
            _application = e.Application;
            ApplicationChanged?.Invoke(this, e);
        }

        private void EditDialogConnectorForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                ClearMessage();
                if (this.DialogResult == DialogResult.OK)
                {
                    _editDialogConnectorControl.ValidateFields();
                }
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                SetErrorMessage(ex.Message);
            }
        }
        #endregion Event Handlers
    }
}
