using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using System;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class DataGraphEditingFormEventsHelper : IDataGraphEditingFormEventsHelper
    {
        private readonly IDataGraphEditingHostEventsHelper _dataGraphEditingHostEventsHelper;
        private readonly IDataGraphEditingManager _dataGraphEditingManager;

        private readonly IDataGraphEditingForm dataGraphEditingForm;

        public DataGraphEditingFormEventsHelper(
            IEditingFormHelperFactory editingFormHelperFactory,
            IDataGraphEditingForm dataGraphEditingForm)
        {
            _dataGraphEditingHostEventsHelper = editingFormHelperFactory.GetDataGraphEditingHostEventsHelper(dataGraphEditingForm);
            _dataGraphEditingManager = editingFormHelperFactory.GetDataGraphEditingManager(dataGraphEditingForm);
            this.dataGraphEditingForm = dataGraphEditingForm;
        }

        private RadTreeView TreeView => dataGraphEditingForm.TreeView;

        public void RequestDocumentUpdate(IEditingControl editingControl) 
            => _dataGraphEditingHostEventsHelper.RequestDocumentUpdate(editingControl);

        public void Setup()
        {
            dataGraphEditingForm.FormClosing += DataGraphEditingForm_FormClosing;
            _dataGraphEditingHostEventsHelper.Setup();
        }

        #region Event Handlers
        private void DataGraphEditingForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            try
            {
                if (TreeView.SelectedNode != null 
                    && dataGraphEditingForm.DialogResult == DialogResult.OK)
                {
                    dataGraphEditingForm.ClearMessage();
                    _dataGraphEditingManager.UpdateXmlDocument(TreeView.SelectedNode);
                }
            }
            catch (XmlException ex)
            {
                e.Cancel = true;
                dataGraphEditingForm.SetErrorMessage(ex.Message);
            }
            catch (LogicBuilderException ex)
            {
                e.Cancel = true;
                dataGraphEditingForm.SetErrorMessage(ex.Message);
            }
        }
        #endregion Event Handlers
    }
}
