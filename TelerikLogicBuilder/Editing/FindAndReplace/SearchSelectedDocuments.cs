using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using ABIS.LogicBuilder.FlowBuilder.XmlValidation;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FindAndReplace
{
    internal class SearchSelectedDocuments : ISearchSelectedDocuments
    {
        private readonly IDiagramSearcher _diagramSearcher;
        private readonly IDisplayResultMessages _displayResultMessages;
        private readonly ITableSearcher _tableSearcher;

        public SearchSelectedDocuments(
            IDiagramSearcher diagramSearcher,
            IDisplayResultMessages displayResultMessages,
            ITableSearcher tableSearcher)
        {
            _diagramSearcher = diagramSearcher;
            _displayResultMessages = displayResultMessages;
            _tableSearcher = tableSearcher;
        }

        public async Task<IList<ResultMessage>> Search(
            IList<string> sourceFiles,
            string searchString,
            bool matchCase,
            bool matchWholeWord,
            Func<string, string, bool, bool, IList<string>> matchFunc,
            IProgress<ProgressMessage> progress,
            CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> resultMessages = new();
            InvisibleApp visioApplication = new();
            _displayResultMessages.Clear(MessageTab.PageSearchResults);

            int matchingItems = 0;
            int matchingFiles = 0;

            try
            {
                foreach (string sourceFile in sourceFiles)
                {
                    if (sourceFile.EndsWith(FileExtensions.VISIOFILEEXTENSION)
                        || sourceFile.EndsWith(FileExtensions.VSDXFILEEXTENSION))
                    {
                        Document visioDocument = visioApplication.Documents.OpenEx(sourceFile, (short)VisOpenSaveArgs.visOpenCopy);
                        SearchDiagramResults searchDiagramResults = await _diagramSearcher.Search
                        (
                            sourceFile,
                            visioDocument,
                            searchString,
                            matchCase,
                            matchWholeWord,
                            matchFunc,
                            progress,
                            cancellationTokenSource
                        );

                        foreach (ResultMessage resultMessage in searchDiagramResults.ResultMessages)
                        {
                            resultMessages.Add(resultMessage);
                            _displayResultMessages.AppendMessage(resultMessage, MessageTab.PageSearchResults);
                        }

                        if (searchDiagramResults.ShapeCount > 0)
                        {
                            matchingItems += searchDiagramResults.ShapeCount;
                            matchingFiles++;
                        }

                        visioDocument.Close();
                    }
                    else if (sourceFile.EndsWith(FileExtensions.TABLEFILEEXTENSION))
                    {
                        List<ResultMessage> tableDocumentResults = new();
                        DataSet dataSet = new()
                        {
                            Locale = CultureInfo.InvariantCulture
                        };

                        try
                        {
                            using (StringReader stringReader = new(Schemas.GetSchema(Schemas.TableSchema)))
                            {
                                dataSet.ReadXmlSchema(stringReader);
                                stringReader.Close();
                            }

                            using FileStream fileStream = new(sourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                            dataSet.ReadXml(fileStream);
                        }
                        catch (ConstraintException ex)
                        {
                            tableDocumentResults.Add(new ResultMessage(ex.Message));
                            continue;
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            tableDocumentResults.Add(new ResultMessage(ex.Message));
                            continue;
                        }
                        catch (ArgumentException ex)
                        {
                            tableDocumentResults.Add(new ResultMessage(ex.Message));
                            continue;
                        }
                        catch (IOException ex)
                        {
                            tableDocumentResults.Add(new ResultMessage(ex.Message));
                            continue;
                        }
                        catch (System.Security.SecurityException ex)
                        {
                            tableDocumentResults.Add(new ResultMessage(ex.Message));
                            continue;
                        }

                        SearchTableResults searchTableResults = await _tableSearcher.Search
                        (
                            sourceFile,
                            dataSet,
                            searchString,
                            matchCase,
                            matchWholeWord,
                            matchFunc,
                            progress,
                            cancellationTokenSource
                        );

                        foreach (ResultMessage resultMessage in searchTableResults.ResultMessages)
                        {
                            resultMessages.Add(resultMessage);
                            _displayResultMessages.AppendMessage(resultMessage, MessageTab.PageSearchResults);
                        }

                        if (searchTableResults.CellCount > 0)
                        {
                            matchingItems += searchTableResults.CellCount;
                            matchingFiles++;
                        }
                    }
                }
            }
            finally
            {
                foreach (Document document in visioApplication.Documents)
                {
                    document.Saved = true;
                    document.Close();
                }
                visioApplication.Quit();
            }

            ResultMessage summary = new
            (
                string.Format
                (
                    CultureInfo.CurrentCulture, 
                    Strings.matchingShapesAndCellsFormat, 
                    matchingItems, 
                    matchingFiles, 
                    sourceFiles.Count
                )
            );

            resultMessages.Add(summary);
            _displayResultMessages.AppendMessage(summary, MessageTab.PageSearchResults);
            return resultMessages;
        }
    }
}
