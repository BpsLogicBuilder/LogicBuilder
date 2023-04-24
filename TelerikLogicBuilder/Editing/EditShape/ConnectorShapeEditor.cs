using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConnector;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Prompts;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditShape
{
    internal class ConnectorShapeEditor : IConnectorShapeEditor
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IMainWindow _mainWindow;
        private readonly IShapeHelper _shapeHelper;
        private readonly IShapeXmlHelper _shapeXmlHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ConnectorShapeEditor(
            IExceptionHelper exceptionHelper,
            IMainWindow mainWindow,
            IShapeHelper shapeHelper,
            IShapeXmlHelper shapeXmlHelper,
            IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _exceptionHelper = exceptionHelper;
            _mainWindow = mainWindow;
            _shapeHelper = shapeHelper;
            _shapeXmlHelper = shapeXmlHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void Edit(Shape connector)
        {
            if (!_shapeHelper.HasFromShape(connector))
            {
                DisplayMessage.Show(_mainWindow.Instance, Strings.connectToEditShapes, _mainWindow.RightToLeft);
                return;
            }

            Shape fromShape = _shapeHelper.GetFromShape(connector);
            if (!ShapeCollections.EditConnectorShapes.ToHashSet().Contains(fromShape.Master.NameU))
            {
                DisplayMessage.Show(_mainWindow.Instance, Strings.connectToEditShapes, _mainWindow.RightToLeft);
                return;
            }

            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditConnectorForm editConnectorForm = GetEditConnectorForm();
            editConnectorForm.ShowDialog(_mainWindow.Instance);
            if (editConnectorForm.DialogResult != DialogResult.OK)
                return;

            _shapeXmlHelper.SetXmlString
            (
                connector,
                editConnectorForm.ShapeXml,
                editConnectorForm.ShapeVisibleText
            );

            IEditConnectorForm GetEditConnectorForm()
            {
                return fromShape.Master.NameU switch
                {
                    UniversalMasterName.DIALOG => throw _exceptionHelper.CriticalException("{F1733068-4FBC-4BE7-A30F-AF1A1230AEE4}"),
                    UniversalMasterName.CONDITIONOBJECT or UniversalMasterName.DECISIONOBJECT => disposableManager.GetEditDecisionConnectorForm
                    (
                        _shapeHelper.GetNextUnusedIndex(fromShape, ConnectorCategory.Decision) ?? (short)DecisionOption.Yes,
                        GetXmlDocument()
                    ),
                    _ => throw _exceptionHelper.CriticalException("{B0E2719E-988D-4E48-B575-F466FBC3F4B4}"),
                };
            }

            XmlDocument? GetXmlDocument()
            {
                string xmlString = _shapeXmlHelper.GetXmlString(connector);
                if (xmlString.Length == 0)
                    return null;

                return _xmlDocumentHelpers.ToXmlDocument(xmlString);
            }
        }
    }
}
