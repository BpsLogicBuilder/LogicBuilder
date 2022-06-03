using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.RulesGenerator;
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
        private readonly IDiagramRulesBuilder _diagramRulesBuilder;
        private readonly ISaveDiagramResources _saveDiagramResources;
        private readonly ISaveDiagramRules _saveDiagramRules;
        private readonly ISaveTableResources _saveTableResources;
        private readonly ISaveTableRules _saveTableRules;
        private readonly IRulesAssembler _rulesAssembler;
        private readonly ITableRulesBuilder _tableRulesBuilder;

        public BuildSaveAssembleRulesForSelectedDocuments(IApplicationTypeInfoManager applicationTypeInfoManager, IDiagramRulesBuilder diagramRulesBuilder, ISaveDiagramResources saveDiagramResources, ISaveDiagramRules saveDiagramRules, ISaveTableResources saveTableResources, ISaveTableRules saveTableRules, IRulesAssembler rulesAssembler, ITableRulesBuilder tableRulesBuilder)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _diagramRulesBuilder = diagramRulesBuilder;
            _saveDiagramResources = saveDiagramResources;
            _saveDiagramRules = saveDiagramRules;
            _saveTableResources = saveTableResources;
            _saveTableRules = saveTableRules;
            _rulesAssembler = rulesAssembler;
            _tableRulesBuilder = tableRulesBuilder;
        }

        public async Task<IList<ResultMessage>> BuildRules(IList<string> sourceFiles, FlowBuilder.Configuration.Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
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
                        BuildRulesResult buildResults = await _diagramRulesBuilder.BuildRules
                        (
                            sourceFile, 
                            visioDocument, 
                            _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name), 
                            progress, 
                            cancellationTokenSource
                        );
                        visioDocument.Close();

                        if (buildResults.ResultMessages.Count > 0)
                        {
                            resultMessages.AddRange(buildResults.ResultMessages);
                            continue;
                        }
                        else
                        {
                            _saveDiagramRules.Save(sourceFile, buildResults.Rules);
                            _saveDiagramResources.Save(sourceFile, buildResults.ResourceStrings);
                        }
                    }
                    else if (sourceFile.EndsWith(FileExtensions.TABLEFILEEXTENSION))
                    {
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
                            resultMessages.Add(new ResultMessage(ex.Message));
                            continue;
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            resultMessages.Add(new ResultMessage(ex.Message));
                            continue;
                        }
                        catch (ArgumentException ex)
                        {
                            resultMessages.Add(new ResultMessage(ex.Message));
                            continue;
                        }
                        catch (IOException ex)
                        {
                            resultMessages.Add(new ResultMessage(ex.Message));
                            continue;
                        }
                        catch (System.Security.SecurityException ex)
                        {
                            resultMessages.Add(new ResultMessage(ex.Message));
                            continue;
                        }

                        BuildRulesResult buildResults = await _tableRulesBuilder.BuildRules
                        (
                            sourceFile,
                            dataSet,
                            _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name),
                            progress,
                            cancellationTokenSource
                        );

                        if (buildResults.ResultMessages.Count > 0)
                        {
                            resultMessages.AddRange(buildResults.ResultMessages);
                            continue;
                        }
                        else
                        {
                            _saveTableRules.Save(sourceFile, dataSet, buildResults.Rules);
                            _saveTableResources.Save(sourceFile, buildResults.ResourceStrings);
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

            await _rulesAssembler.AssembleRules(sourceFiles, progress, cancellationTokenSource);
            await _rulesAssembler.AssembleResources(sourceFiles, progress, cancellationTokenSource);

            return new ResultMessage[] { new ResultMessage(Strings.buildSuccessful) };
        }
    }
}
