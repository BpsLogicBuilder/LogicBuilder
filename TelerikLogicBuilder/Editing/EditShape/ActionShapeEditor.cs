using ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape
{
    internal class ActionShapeEditor : IActionShapeEditor
    {
        private readonly IMainWindow _mainWindow;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ActionShapeEditor(IMainWindow mainWindow, IShapeXmlHelper shapeXmlHelper, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _mainWindow = mainWindow;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Edit(Shape shape)
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditFunctionsForm editFunctionsForm = disposableManager.GetEditFunctionsForm
            (
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
