using ABIS.LogicBuilder.FlowBuilder.Configuration;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
        private readonly IDisplayResultMessages _displayResultMessages;
        private readonly IPathHelper _pathHelper;
        private readonly IRuleSetLoader _ruleSetLoader;
        private readonly IRulesValidator _rulesValidator;

        public ValidateSelectedRules(
            IApplicationTypeInfoManager applicationTypeInfoManager,
            IDisplayResultMessages displayResultMessages,
            IPathHelper pathHelper,
            IRuleSetLoader ruleSetLoader,
            IRulesValidator rulesValidator)
        {
            _applicationTypeInfoManager = applicationTypeInfoManager;
            _displayResultMessages = displayResultMessages;
            _pathHelper = pathHelper;
            _ruleSetLoader = ruleSetLoader;
            _rulesValidator = rulesValidator;
        }

        public async Task<IList<ResultMessage>> Validate(IList<string> sourceFiles, Application application, IProgress<ProgressMessage> progress, CancellationTokenSource cancellationTokenSource)
        {
            List<ResultMessage> resultMessages = new();
            ApplicationTypeInfo applicationTypeInfo = _applicationTypeInfoManager.GetApplicationTypeInfo(application.Name);
            _displayResultMessages.Clear(MessageTab.Rules);

            for (int i = 0; i < sourceFiles.Count; i++)
            {
                string fileName = string.Empty;
                List<ResultMessage> validationMessages = new();
                try
                {
                    fileName = _pathHelper.GetFileName(sourceFiles[i]);
                    RuleSet ruleSet = _ruleSetLoader.LoadRuleSet(sourceFiles[i]);

                    validationMessages.AddRange
                    (
                        await _rulesValidator.Validate
                        (
                            ruleSet,
                            applicationTypeInfo,
                            cancellationTokenSource
                        )
                    );

                    if (validationMessages.Count == 0)
                    {
                        validationMessages.Add
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
                    validationMessages.Add(new ResultMessage(ex.Message));
                    continue;
                }
                catch (ArgumentException ex)
                {
                    validationMessages.Add(new ResultMessage(ex.Message));
                    continue;
                }
                catch (IOException ex)
                {
                    validationMessages.Add(new ResultMessage(ex.Message));
                    continue;
                }
                catch (NotSupportedException ex)
                {
                    validationMessages.Add(new ResultMessage(ex.Message));
                    continue;
                }
                finally
                {
                    foreach (ResultMessage resultMessage in validationMessages)
                    {
                        resultMessages.Add(resultMessage);
                        _displayResultMessages.AppendMessage(resultMessage, MessageTab.Rules);
                    }

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
