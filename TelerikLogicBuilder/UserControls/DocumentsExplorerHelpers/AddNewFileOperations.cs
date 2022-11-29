using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Forms;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.UserControls.DocumentsExplorerHelpers
{
    internal class AddNewFileOperations : IAddNewFileOperations
    {
        private readonly ICheckVisioConfiguration _checkVisioConfiguration;
        private readonly IMainWindow _mainWindow;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public AddNewFileOperations(ICheckVisioConfiguration checkVisioConfiguration, IMainWindow mainWindow, IXmlDocumentHelpers xmlDocumentHelpers)
        {
            _checkVisioConfiguration = checkVisioConfiguration;
            _mainWindow = mainWindow;
            _xmlDocumentHelpers = xmlDocumentHelpers;
        }

        public void AddNewTableFile(string fullName)
        {
            using XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateFormattedXmlWriter(fullName, Encoding.Unicode);
            xmlTextWriter.WriteElementString(XmlDataConstants.TABLESELEMENT, string.Empty);
            xmlTextWriter.Flush();
            xmlTextWriter.Close();
        }

        public void AddNewVisioFile(string fullName)
        {
            IList<string> configErrors = _checkVisioConfiguration.Check();
            if (configErrors.Count > 0)
            {
                using IScopedDisposableManager<TextViewer> disposableManager = Program.ServiceProvider.GetRequiredService<IScopedDisposableManager<TextViewer>>();
                TextViewer textViewer = disposableManager.ScopedService;
                textViewer.SetText(configErrors.ToArray());
                textViewer.ShowDialog(_mainWindow.Instance);
                return;
            }

            InvisibleApp visioApplication = new();
            Document visioDocument = visioApplication.Documents.Add("");
            Shape pageSheet = visioDocument.Pages[1].PageSheet;
            SetToLandscape
            (
                pageSheet.get_CellsSRC
                (
                    (short)VisSectionIndices.visSectionObject,
                    (short)VisRowIndices.visRowPage,
                    (short)VisCellIndices.visPageWidth
                ),
                pageSheet.get_CellsSRC
                (
                    (short)VisSectionIndices.visSectionObject,
                    (short)VisRowIndices.visRowPage,
                    (short)VisCellIndices.visPageHeight
                ),
                pageSheet.get_CellsSRC
                (
                    (short)VisSectionIndices.visSectionObject,
                    (short)VisRowIndices.visRowPrintProperties,
                    (short)VisCellIndices.visPrintPropertiesPageOrientation
                )
            );

            visioDocument.SaveAs(fullName);
            visioApplication.Quit();

            static void SetToLandscape(Cell width, Cell height, Cell orientation)
            {
                width.FormulaU = "11 in";
                height.FormulaU = "8.5 in";
                orientation.FormulaU = "2";
            }
        }
    }
}
