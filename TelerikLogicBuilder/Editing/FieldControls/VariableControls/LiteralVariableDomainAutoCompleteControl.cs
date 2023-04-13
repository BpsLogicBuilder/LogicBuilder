using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Variables;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.VariableControls
{
    internal partial class LiteralVariableDomainAutoCompleteControl : UserControl, ILiteralVariableDomainAutoCompleteControl
    {
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingControl dataGraphEditingControl;
        private readonly LiteralVariable literalVariable;
        private RadDropDownList radDropDownList;
        private bool modified;

        public LiteralVariableDomainAutoCompleteControl(
            IRadDropDownListHelper radDropDownListHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingControl dataGraphEditingControl,
            LiteralVariable literalVariable)
        {
            InitializeComponent();
            _radDropDownListHelper = radDropDownListHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingControl = dataGraphEditingControl;
            this.literalVariable = literalVariable;
            Initialize();
        }

        private readonly HelpProvider helpProvider = new();
        private readonly RadToolTip toolTip = new();

        public bool IsEmpty => false;

        public string MixedXml
        {
            get
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFragmentXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteString(radDropDownList.Text);
                    xmlTextWriter.Flush();
                }

                return stringBuilder.ToString();
            }
        }

        public string VisibleText => radDropDownList.Text;

        public XmlElement? XmlElement => _xmlDocumentHelpers.ToXmlElement
        (
            _xmlDataHelper.BuildVariableValueXml(literalVariable, MixedXml)
        );

        public event EventHandler? Changed;

        public void HideControls() => radDropDownList.Visible = false;

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void RequestDocumentUpdate() => dataGraphEditingControl.RequestDocumentUpdate();

        public void ResetControl() => radDropDownList.Text = string.Empty;

        public void SetErrorBackColor()
            => SetDropDownBorderForeColor
            (
                ForeColorUtility.GetGroupBoxBorderErrorColor()
            );

        public void SetNormalBackColor()
            => SetDropDownBorderForeColor
            (
                ForeColorUtility.GetGroupBoxBorderColor(ThemeResolutionService.ApplicationThemeName)
            );

        public void SetToolTipHelp(string toolTipText)
        {
            helpProvider.SetHelpString(radPanelDropDownList, toolTipText);
            toolTip.SetToolTip(radPanelDropDownList, toolTipText);
        }

        public void ShowControls() => radDropDownList.Visible = true;

        public void Update(XmlElement xmlElement)
        {
            radDropDownList.Text = xmlElement.InnerText;
            modified = false;
        }

        void IValueControl.Focus() => radDropDownList.Select();

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        [MemberNotNull(nameof(radDropDownList))]
        private void Initialize()
        {
            InitializeDropDownList();
            CollapsePanelBorder(radPanelDropDownList);
            _radDropDownListHelper.LoadTextItems(radDropDownList, literalVariable.Domain, RadDropDownStyle.DropDown);

            radDropDownList.TextChanged += RadDropDownList_TextChanged;
            radDropDownList.Validated += RadDropDownList_Validated;
        }

        [MemberNotNull(nameof(radDropDownList))]
        private void InitializeDropDownList()
        {
            this.radDropDownList = new RadDropDownList();

            ((ISupportInitialize)this.radPanelDropDownList).BeginInit();
            this.radPanelDropDownList.SuspendLayout();

            ((ISupportInitialize)this.radDropDownList).BeginInit();
            ControlsLayoutUtility.SetDropDownListPadding(this.radDropDownList);
            this.radDropDownList.Dock = DockStyle.Fill;
            this.radDropDownList.AutoSize = false;
            this.radDropDownList.DropDownAnimationEnabled = true;
            this.radDropDownList.Location = new Point(0, 0);
            this.radDropDownList.Name = "radDropDownList";
            this.radDropDownList.Size = new Size(350, 28);
            this.radDropDownList.TabIndex = 0;
            ((ISupportInitialize)this.radDropDownList).EndInit();

            this.radPanelDropDownList.Controls.Add(radDropDownList);
            ((ISupportInitialize)this.radPanelDropDownList).EndInit();
            this.radPanelDropDownList.ResumeLayout(true);
        }

        private void SetDropDownBorderForeColor(Color color)
            => ((BorderPrimitive)radDropDownList.DropDownListElement.Children[0]).ForeColor = color;

        #region Event Handlers
        private void RadDropDownList_TextChanged(object? sender, EventArgs e)
        {
            modified = true;
            Changed?.Invoke(this, e);
        }

        private void RadDropDownList_Validated(object? sender, EventArgs e)
        {
            if (!modified)
                return;

            modified = false;
            RequestDocumentUpdate();
        }
        #endregion Event Handlers
    }
}
