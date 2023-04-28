using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions;
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
    internal class ConditionsCellEditor : IConditionsCellEditor
    {
        private readonly ICellXmlHelper _cellXmlHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConditionsCellEditor(ICellXmlHelper cellXmlHelper, IMainWindow mainWindow, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _cellXmlHelper = cellXmlHelper;
            _mainWindow = mainWindow;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Edit(DataSet dataSet, GridViewCellInfo currentCell)
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditConditionFunctionsForm editConditionFunctionsForm = disposableManager.GetEditConditionFunctionsForm
            (
                GetXmlDocument()
            );

            editConditionFunctionsForm.ShowDialog(_mainWindow.Instance);
            if (editConditionFunctionsForm.DialogResult != DialogResult.OK)
                return;

            _cellXmlHelper.SetXmlString
            (
                dataSet,
                currentCell,
                editConditionFunctionsForm.ShapeXml,
                editConditionFunctionsForm.ShapeVisibleText
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
