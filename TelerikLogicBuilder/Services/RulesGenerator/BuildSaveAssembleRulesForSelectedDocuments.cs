using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator.Factories;
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
    internal class BuildSaveAssembleRulesForSelectedDocuments : IBuildSaveAssembleRulesForSelectedDocuments
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IDisplayResultMessages _displayResultMessages;
        private readonly ISaveDiagramResources _saveDiagramResources;
        private readonly ISaveDiagramRules _saveDiagramRules;
        private readonly ISaveTableResources _saveTableResources;
        private readonly ISaveTableRules _saveTableRules;
        private readonly IRulesAssembler _rulesAssembler;
        private readonly IRulesGeneratorFactory _rulesGeneratorFactory;

        public BuildSaveAssembleRulesForSelectedDocuments(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IDisplayResultMessages displayResultMessages,
            ISaveDiagramResources saveDiagramResources,
            ISaveDiagramRules saveDiagramRules,
            ISaveTableResources saveTableResources,
            ISaveTableRules saveTableRules,
            IRulesAssembler rulesAssembler,
            IRulesGeneratorFactory rulesGeneratorFactory)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _rulesGeneratorFactory = rulesGeneratorFactory;
            _displayResultMessages = displayResultMessages;
            _saveDiagramResources = saveDiagramResources;
            _saveDiagramRules = saveDiagramRules;
            _saveTableResources = saveTableResources;
            _saveTableRules = saveTableRules;
            _rulesAssembler = rulesAssembler;
        }

        public async Task<IList<ResultMessage>> BuildRules(IList<string> sourceFiles, FlowBuilder.Configuration.Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> resultMessages = new();
            InvisibleApp visioApplication = new();
            _displayResultMessages.Clear(MessageTab.Documents);

            try
            {
                foreach (string sourceFile in sourceFiles)
                {
                    if (sourceFile.EndsWith(FileExtensions.VISIOFILEEXTENSION)
                                        || sourceFile.EndsWith(FileExtensions.VSDXFILEEXTENSION))
                    {
                        List<ResultMessage> visioDocumentResults = new();
                        try
                        {
                            Document visioDocument = visioApplication.Documents.OpenEx(sourceFile, (short)VisOpenSaveArgs.visOpenCopy);
                            BuildRulesResult buildResults = await _rulesGeneratorFactory.GetDiagramRulesBuilder
                            (
                                sourceFile,
                                visioDocument,
                                _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name),
                                progress,
                                cancellationTokenSource
                            ).BuildRules();
                            visioDocument.Close();
                            visioDocumentResults.AddRange(buildResults.ResultMessages);

                            if (visioDocumentResults.Count == 0)
                            {
                                _saveDiagramRules.Save(sourceFile, buildResults.Rules);
                                _saveDiagramResources.Save(sourceFile, buildResults.ResourceStrings);
                            }
                        }
                        catch (LogicBuilderException ex)
                        {
                            visioDocumentResults.Add(new ResultMessage(ex.Message));
                        }

                        foreach (ResultMessage resultMessage in visioDocumentResults)
                        {
                            resultMessages.Add(resultMessage);
                            _displayResultMessages.AppendMessage(resultMessage, MessageTab.Documents);
                        }
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

                        try
                        {
                            BuildRulesResult buildResults = await _rulesGeneratorFactory.GetTableRulesBuilder
                            (
                                sourceFile,
                                dataSet,
                                _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name),
                                progress,
                                cancellationTokenSource
                            ).BuildRules();

                            tableDocumentResults.AddRange(buildResults.ResultMessages);
                            if (tableDocumentResults.Count == 0)
                            {
                                _saveTableRules.Save(sourceFile, dataSet, buildResults.Rules);
                                _saveTableResources.Save(sourceFile, buildResults.ResourceStrings);
                            }
                        }
                        catch (LogicBuilderException ex)
                        {
                            tableDocumentResults.Add(new ResultMessage(ex.Message));
                        }

                        foreach (ResultMessage resultMessage in tableDocumentResults)
                        {
                            resultMessages.Add(resultMessage);
                            _displayResultMessages.AppendMessage(resultMessage, MessageTab.Documents);
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

            if (resultMessages.Count > 0)
                return resultMessages;

            try
            {
                await _rulesAssembler.AssembleRules(sourceFiles, progress, cancellationTokenSource);
                await _rulesAssembler.AssembleResources(sourceFiles, progress, cancellationTokenSource);
            }
            catch (LogicBuilderException ex)
            {
                ResultMessage resultMessage = new(ex.Message);
                resultMessages.Add(resultMessage);
                _displayResultMessages.AppendMessage(resultMessage, MessageTab.Documents);
            }

            if (resultMessages.Count == 0)
            {
                resultMessages.Add(new ResultMessage(Strings.buildSuccessful));
                _displayResultMessages.AppendMessage(resultMessages[0], MessageTab.Documents);
            }

            return resultMessages;
        }
    }
}
