﻿using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
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

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class ValidateSelectedDocuments : IValidateSelectedDocuments
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IDiagramValidator _diagramValidator;
        private readonly ITableValidator _tableValidator;
        private readonly IDisplayResultMessages _displayResultMessages;

        public ValidateSelectedDocuments(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IDiagramValidator diagramValidator,
            ITableValidator tableValidator,
            IDisplayResultMessages displayResultMessages)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _diagramValidator = diagramValidator;
            _tableValidator = tableValidator;
            _displayResultMessages = displayResultMessages;
        }

        public async Task<IList<ResultMessage>> Validate(IList<string> sourceFiles, FlowBuilder.Configuration.Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> resultMessages = new();
            InvisibleApp visioApplication = new();

            try
            {
                foreach (string sourceFile in sourceFiles)
                {
                    if (sourceFile.EndsWith(FileExtensions.VISIOFILEEXTENSION)
                                        || sourceFile.EndsWith(FileExtensions.VSDXFILEEXTENSION))
                    {
                        Document visioDocument = visioApplication.Documents.OpenEx(sourceFile, (short)VisOpenSaveArgs.visOpenCopy);
                        IList<ResultMessage> visioDocumentResults = await _diagramValidator.Validate
                        (
                            sourceFile,
                            visioDocument,
                            _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name),
                            progress,
                            cancellationTokenSource
                        );

                        foreach (ResultMessage resultMessage in visioDocumentResults)
                        {
                            resultMessages.Add(resultMessage);
                            _displayResultMessages.AppendMessage(resultMessage, MessageTab.Documents);
                        }

                        visioDocument.Close();
                    }
                    else if (sourceFile.EndsWith(FileExtensions.TABLEFILEEXTENSION))
                    {
                        List<ResultMessage> tableDocumentResults = new List<ResultMessage>();
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

                        tableDocumentResults.AddRange
                        (
                            await _tableValidator.Validate
                            (
                                sourceFile,
                                dataSet,
                                _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name),
                                progress,
                                cancellationTokenSource
                            )
                        );

                        foreach (ResultMessage resultMessage in tableDocumentResults)
                        {
                            resultMessages.Add(resultMessage);
                            _displayResultMessages.AppendMessage(resultMessage, MessageTab.Documents);
                        }

                        dataSet.Dispose();
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

            return resultMessages.Count == 0 
                ? new ResultMessage[] { new ResultMessage(Strings.validationSuccessful) } 
                : resultMessages;
        }
    }
}
