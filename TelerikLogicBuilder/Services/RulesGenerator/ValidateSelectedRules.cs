using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class ValidateSelectedRules : IValidateSelectedRules
    {
        private readonly IApplicationTypeInfoManager _applicationTypeInfoManager;
        private readonly IPathHelper _pathHelper;
        private readonly IRuleSetLoader _ruleSetLoader;
        private readonly IRulesValidator _rulesValidator;

        public ValidateSelectedRules(IApplicationTypeInfoManager applicationTypeInfoManager, IPathHelper pathHelper, IRuleSetLoader ruleSetLoader, IRulesValidator rulesValidator)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _pathHelper = pathHelper;
            _ruleSetLoader = ruleSetLoader;
            _rulesValidator = rulesValidator;
        }

        public async Task<IList<ResultMessage>> Validate(IList<string> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> resultMessages = new();
            ApplicationTypeInfo applicationTypeInfo = _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name);

            for (int i = 0; i < sourceFiles.Count; i++)
            {
                string fileName = string.Empty;
                try
                {
                    fileName = _pathHelper.GetFileName(sourceFiles[i]);
                    RuleSet ruleSet = _ruleSetLoader.LoadRuleSet(sourceFiles[i]);

                    IList<ResultMessage> validationMessages = await _rulesValidator.Validate
                    (
                        ruleSet,
                        applicationTypeInfo,
                        cancellationTokenSource
                    );

                    if (validationMessages.Count > 0)
                    {
                        resultMessages.AddRange(validationMessages);
                    }
                    else
                    {
                        resultMessages.Add
                        (
                            new ResultMessage
                            (
                                string.Format
                                (
                                    CultureInfo.CurrentCulture,
                                    Strings.ruleSetIsValidFormat,
                                    fileName
                                )
                            )
                        );
                    }
                }
                catch (LogicBuilderException ex)
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
                catch (NotSupportedException ex)
                {
                    resultMessages.Add(new ResultMessage(ex.Message));
                    continue;
                }
                finally
                {
                    progress.Report
                    (
                        new ProgressMessage
                        (
                            (int)((float)i / (float)sourceFiles.Count * 100),
                            string.Format(CultureInfo.CurrentCulture, Strings.progressFormTaskValidatingFormat, fileName)
                        )
                    );
                }
            }

            return resultMessages;
        }
    }
}
