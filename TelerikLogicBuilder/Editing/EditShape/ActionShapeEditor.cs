using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape
{
    internal class ActionShapeEditor : IActionShapeEditor
    {
        private readonly IConfigurationService _configurationService;
        private readonly IMainWindow _mainWindow;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ActionShapeEditor(
            IConfigurationService configurationService,
            IMainWindow mainWindow,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _configurationService = configurationService;
            _mainWindow = mainWindow;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Edit(Shape shape)
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditFunctionsForm editFunctionsForm = disposableManager.GetEditFunctionsForm
            (
                _configurationService.FunctionList.VoidFunctions,
                new TreeFolder[] { _configurationService.FunctionList.BuiltInVoidFunctionsTreeFolder, _configurationService.FunctionList.VoidFunctionsTreeFolder },
                GetXmlDocument()
            );

            editFunctionsForm.ShowDialog(_mainWindow.Instance);
            if (editFunctionsForm.DialogResult != DialogResult.OK)
                return;

            _shapeXmlHelper.SetXmlString
            (
                shape,
                editFunctionsForm.ShapeXml,
                editFunctionsForm.ShapeVisibleText
            );

            XmlDocument? GetXmlDocument()
            {
                string xmlString = _shapeXmlHelper.GetXmlString(shape);
                if (xmlString.Length == 0)
                    return null;

                return _xmlDocumentHelpers.ToXmlDocument(xmlString);
            }
        }
    }
}
