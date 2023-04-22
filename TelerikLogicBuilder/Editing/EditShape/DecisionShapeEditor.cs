using ABIS.LogicBuilder.FlowBuilder.Editing.EditConditionFunctions;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape
{
    internal class DecisionShapeEditor : IDecisionShapeEditor
    {
        private readonly IMainWindow _mainWindow;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public DecisionShapeEditor(
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
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditDecisionsForm editDecisionsForm = disposableManager.GetEditDecisionsForm
            (
                GetXmlDocument()
            );

            editDecisionsForm.ShowDialog(_mainWindow.Instance);
            if (editDecisionsForm.DialogResult != DialogResult.OK)
                return;

            _shapeXmlHelper.SetXmlString
            (
                shape,
                editDecisionsForm.ShapeXml,
                editDecisionsForm.ShapeVisibleText
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
