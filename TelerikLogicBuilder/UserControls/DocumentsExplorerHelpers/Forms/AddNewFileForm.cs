using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.UserControls.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers.Forms
{
    internal partial class AddNewFileForm : RadForm, IAddNewFileForm
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IImageListService _imageListService;
        private readonly IFormInitializer _formInitializer;
        private readonly IDialogFormMessageControl _dialogFormMessageControl;

        public AddNewFileForm(
            IExceptionHelper exceptionHelper,
            IImageListService imageListService,
            IFormInitializer formInitializer,
            IDialogFormMessageControl dialogFormMessageControl)
        {
            _exceptionHelper = exceptionHelper;
            _imageListService = imageListService;
            _formInitializer = formInitializer;
            _dialogFormMessageControl = dialogFormMessageControl;

            InitializeComponent();
            Initialize();
        }

        public string FileName
        {
            get
            {
                IDictionary<int, string> extentionsDictionary = new Dictionary<int, string>
                {
                    [ImageIndexes.VSDXFILEIMAGEINDEX] = FileExtensions.VSDXFILEEXTENSION,
                    [ImageIndexes.VISIOFILEIMAGEINDEX] = FileExtensions.VISIOFILEEXTENSION,
                    [ImageIndexes.TABLEFILEIMAGEINDEX] = FileExtensions.TABLEFILEEXTENSION
                };

                if (!extentionsDictionary.TryGetValue(radListView1.SelectedItems[0].ImageIndex, out string? extension))
                    throw _exceptionHelper.CriticalException("{7CECE580-2566-4702-8C34-67315FC8836B}");

                return radTextBoxFileName.Text.ToLowerInvariant().EndsWith(extension)
                        ? radTextBoxFileName.Text
                        : $"{radTextBoxFileName.Text}{extension}";
            }
        }

        private static void CollapsePanelBorder(RadPanel radPanel)
            => ((BorderPrimitive)radPanel.PanelElement.Children[1]).Visibility = ElementVisibility.Collapsed;

        private void Initialize()
        {
            InitializeDialogFormMessageControl();
            ControlsLayoutUtility.LayoutGroupBox(radPanelFill, radGroupBoxFileType);

            _formInitializer.SetFormDefaults(this, 557);

            radTextBoxFileName.TextChanged += RadTextBoxFileName_TextChanged;

            radListView1.VisualItemFormatting += RadListView1_VisualItemFormatting;
            radListView1.AllowEdit = false;
            radListView1.MultiSelect = false;
            radListView1.Padding = new Padding(30);
            radListView1.ImageList = _imageListService.ImageList;
            radListView1.ViewType = ListViewType.IconsView;
            radListView1.ItemSpacing = 20;
            radListView1.ItemSize = PerFontSizeConstants.AddNewFileIconSize;

            radListView1.Items.Add(GetListViewDataItem(Strings.listViewTextVsdx, ImageIndexes.VSDXFILEIMAGEINDEX));
            radListView1.Items.Add(GetListViewDataItem(Strings.listViewTextVsd, ImageIndexes.VISIOFILEIMAGEINDEX));
            radListView1.Items.Add(GetListViewDataItem(Strings.listViewTextTbl, ImageIndexes.TABLEFILEIMAGEINDEX));

            radListView1.SelectedIndex = 0;

            this.ActiveControl = radTextBoxFileName;
            this.AcceptButton = btnOk;

            this.btnOk.DialogResult = DialogResult.OK;
            this.btnCancel.DialogResult = DialogResult.Cancel;

            CollapsePanelBorder(radPanelBottom);
            CollapsePanelBorder(radPanelButtons);
            CollapsePanelBorder(radPanelFill);
            CollapsePanelBorder(radPanelMessages);
            CollapsePanelBorder(radPanelTextBox);

            static ListViewDataItem GetListViewDataItem(string text, int imageIndex)
                => new(text)
                {
                    ImageIndex = imageIndex,
                    ImageAlignment = ContentAlignment.MiddleCenter,
                    TextAlignment = ContentAlignment.MiddleCenter,
                    TextImageRelation = TextImageRelation.ImageAboveText
                };
        }

        private void InitializeDialogFormMessageControl()
        {
            ControlsLayoutUtility.LayoutBottomPanel(radPanelBottom, radPanelMessages, radPanelButtons, tableLayoutPanelButtons, _dialogFormMessageControl);
        }

        private void RadTextBoxFileName_TextChanged(object? sender, EventArgs e)
        {
            if (radTextBoxFileName.Text.Trim().Length == 0)
            {
                btnOk.Enabled = false;
            }
            else if (!Regex.IsMatch(radTextBoxFileName.Text, RegularExpressions.FILENAME))
            {
                _dialogFormMessageControl.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.invalidFileNameMessageFormat, radLabelFileName.Text));
                btnOk.Enabled = false;
            }
            else
            {
                _dialogFormMessageControl.ClearMessage();
                btnOk.Enabled = true;
            }
        }

        private void RadListView1_VisualItemFormatting(object sender, ListViewVisualItemEventArgs e)
        {
            if (e.VisualItem.Data.Image != null)
            {
                e.VisualItem.Image = e.VisualItem.Data.Image.GetThumbnailImage(32, 32, null, IntPtr.Zero);
            }
        }
    }
}
