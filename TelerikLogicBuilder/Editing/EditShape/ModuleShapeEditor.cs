using ABIS.LogicBuilder.FlowBuilder.Editing.EditModuleShape;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape
{
    internal class ModuleShapeEditor : IModuleShapeEditor
    {
        private readonly IMainWindow _mainWindow;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ModuleShapeEditor(IMainWindow mainWindow, IShapeXmlHelper shapeXmlHelper, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _mainWindow = mainWindow;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Edit(Shape shape)
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditModuleShapeForm editModuleShapeForm = disposableManager.GetEditModuleShapeForm(GetXmlDocument());
            editModuleShapeForm.ShowDialog(_mainWindow.Instance);
            if (editModuleShapeForm.DialogResult != DialogResult.OK)
                return;

            _shapeXmlHelper.SetXmlString
            (
                shape,
                editModuleShapeForm.ShapeXml,
                editModuleShapeForm.ShapeVisibleText
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
