using ABIS.LogicBuilder.FlowBuilder.Editing.EditPriority;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditCell
{
    internal class PriorityCellEditor : IPriorityCellEditor
    {
        private readonly ICellXmlHelper _cellXmlHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public PriorityCellEditor(
            ICellXmlHelper cellXmlHelper,
            IMainWindow mainWindow,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _cellXmlHelper = cellXmlHelper;
            _mainWindow = mainWindow;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Edit(DataSet dataSet, GridViewCellInfo currentCell)
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IEditPriorityForm editPriorityForm = disposableManager.GetEditPriorityForm(GetXmlDocument());

            editPriorityForm.ShowDialog(_mainWindow.Instance);
            if (editPriorityForm.DialogResult != DialogResult.OK)
                return;

            _cellXmlHelper.SetXmlString
            (
                dataSet,
                currentCell,
                editPriorityForm.ShapeXml,
                editPriorityForm.ShapeVisibleText
            );

            XmlDocument? GetXmlDocument()
            {
                string xmlString = _cellXmlHelper.GetXmlString(currentCell);
                if (xmlString.Length == 0)
                    return null;

                return _xmlDocumentHelpers.ToXmlDocument(xmlString);
            }
        }
    }
}
