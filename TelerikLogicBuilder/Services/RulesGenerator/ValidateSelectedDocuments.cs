using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using Microsoft.Office.Interop.Visio;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class ValidateSelectedDocuments : IValidateSelectedDocuments
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IDiagramValidator _diagramValidator;

        public ValidateSelectedDocuments(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IDiagramValidator diagramValidator)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;  
            _diagramValidator = diagramValidator;
        }

        public async Task<IList<ResultMessage>> Validate(IList<string> sourceFiles, FlowBuilder.Configuration.Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> resultMessages = new();
            InvisibleApp visioApplication = new();
            try
            {
                
                foreach (string sourceFile in sourceFiles)
                {
                    try
                    {
                        if (sourceFile.EndsWith(FileExtensions.VISIOFILEEXTENSION)
                                        || sourceFile.EndsWith(FileExtensions.VSDXFILEEXTENSION))
                        {
                            Document visioDocument = visioApplication.Documents.OpenEx(sourceFile, (short)VisOpenSaveArgs.visOpenCopy);
                            resultMessages.AddRange
                            (
                                await _diagramValidator.Validate
                                (
                                    sourceFile,
                                    visioDocument,
                                    _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name),
                                    progress,
                                    cancellationTokenSource
                                )
                            );
                            visioDocument.Close();
                        }
                        else if (sourceFile.EndsWith(FileExtensions.TABLEFILEEXTENSION))
                        {

                        }
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
