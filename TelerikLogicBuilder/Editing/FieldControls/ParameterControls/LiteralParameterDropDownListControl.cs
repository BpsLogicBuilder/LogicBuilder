using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.ParameterControls
{
    internal partial class LiteralParameterDropDownListControl : UserControl, ILiteralParameterDropDownListControl
    {
        private readonly ICreateLiteralParameterXmlElement _createLiteralParameterXmlElement;
        private readonly IRadDropDownListHelper _radDropDownListHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IDataGraphEditingControl dataGraphEditingControl;
        private readonly LiteralParameter literalParameter;
        private RadDropDownList radDropDownList;
        private bool modified;

        public LiteralParameterDropDownListControl(
            ICreateLiteralParameterXmlElement createLiteralParameterXmlElement,
            IRadDropDownListHelper radDropDownListHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IDataGraphEditingControl dataGraphEditingControl,
            LiteralParameter literalParameter)
        {
            InitializeComponent();
            _createLiteralParameterXmlElement = createLiteralParameterXmlElement;
            _radDropDownListHelper = radDropDownListHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.dataGraphEditingControl = dataGraphEditingControl;
            this.literalParameter = literalParameter;
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

        public XmlElement? XmlElement => _createLiteralParameterXmlElement.Create(literalParameter, MixedXml);

        public event EventHandler? Changed;

        public void HideControls() => radDropDownList.Visible = false;

        public void InvokeChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void RequestDocumentUpdate() => dataGraphEditingControl.RequestDocumentUpdate();

        public void ResetControl()
        {
            if (radDropDownList.Items.Count > 0)
                radDropDownList.SelectedIndex = 0;
        }

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
            if (literalParameter.Domain.ToHashSet().Contains(xmlElement.InnerText))
                radDropDownList.SelectedValue = xmlElement.InnerText;

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
            _radDropDownListHelper.LoadTextItems(radDropDownList, literalParameter.Domain);
            if (literalParameter.Domain.Count > 0)
                radDropDownList.SelectedIndex = 0;

            radDropDownList.SelectedIndexChanged += RadDropDownList_SelectedIndexChanged;
            radDropDownList.Validated += RadDropDownList_Validated;
        }

        [MemberNotNull(nameof(radDropDownList))]
        private void InitializeDropDownList()
        {
            this.radDropDownList = new RadDropDownList();

            ((ISupportInitialize)this.radPanelDropDownList).BeginInit();
            this.radPanelDropDownList.SuspendLayout();

            ((ISupportInitialize)this.radDropDownList).BeginInit();
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
        private void RadDropDownList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
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
