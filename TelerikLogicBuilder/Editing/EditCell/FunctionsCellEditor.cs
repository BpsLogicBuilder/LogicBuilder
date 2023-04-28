using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using Telerik.WinControls.UI;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditCell
{
    internal class FunctionsCellEditor : IFunctionsCellEditor
    {
        private readonly ICellXmlHelper _cellXmlHelper;
        private readonly IConfigurationService _configurationService;
        private readonly IMainWindow _mainWindow;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public FunctionsCellEditor(
            ICellXmlHelper cellXmlHelper,
            IConfigurationService configurationService,
            IMainWindow mainWindow,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _cellXmlHelper = cellXmlHelper;
            _configurationService = configurationService;
            _mainWindow = mainWindow;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Edit(DataSet dataSet, GridViewCellInfo currentCell)
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditFunctionsForm editFunctionsForm = disposableManager.GetEditFunctionsForm
            (
                _configurationService.FunctionList.TableFunctions,
                new TreeFolder[] { _configurationService.FunctionList.BuiltInTableFunctionsTreeFolder, _configurationService.FunctionList.TableFunctionsTreeFolder },
                GetXmlDocument()
            );

            editFunctionsForm.ShowDialog(_mainWindow.Instance);
            if (editFunctionsForm.DialogResult != DialogResult.OK)
                return;

            _cellXmlHelper.SetXmlString
            (
                dataSet,
                currentCell,
                editFunctionsForm.ShapeXml,
                editFunctionsForm.ShapeVisibleText
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
