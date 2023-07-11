using ABIS.LogicBuilder.FlowBuilder.Editing.EditJump;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape
{
    internal class JumpShapeEditor : IJumpShapeEditor
    {
        private readonly IMainWindow _mainWindow;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public JumpShapeEditor(
            IMainWindow mainWindow,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _mainWindow = mainWindow;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Edit(Shape shape)
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IEditJumpForm editJumpForm = disposableManager.GetEditJumpForm(GetXmlDocument());
            editJumpForm.ShowDialog(_mainWindow.Instance);
            if (editJumpForm.DialogResult != DialogResult.OK)
                return;

            _shapeXmlHelper.SetXmlString
            (
                shape,
                editJumpForm.ShapeXml,
                editJumpForm.ShapeVisibleText
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
