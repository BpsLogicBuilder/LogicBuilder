using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using LogicBuilder.Workflow.Activities.Rules;
using LogicBuilder.Workflow.ComponentModel.Compiler;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class RulesValidator : IRulesValidator
    {
        public Task<IList<ResultMessage>> Validate(RuleSet ruleSet, ApplicationTypeInfo application, CancellationTokenSource cancellationTokenSource)
        {
            return Task.Run
            (
               () => (IList<ResultMessage>)ValidateRules(ruleSet, application),
               cancellationTokenSource.Token
            );
        }

        private static List<ResultMessage> ValidateRules(RuleSet ruleSet, ApplicationTypeInfo application)
        {
            List<ResultMessage> resultMessages = [];
            if (!application.AssemblyAvailable)
            {
                resultMessages.Add(new ResultMessage(application.UnavailableMessage));
                return resultMessages;
            }

            RuleValidation ruleValidation = new(application.ActivityType, application.AllAssemblies);

            try
            {
                if (!ruleSet.Validate(ruleValidation))
                {
                    foreach (ValidationError validationError in ruleValidation.Errors)
                        resultMessages.Add(new ResultMessage(validationError.ErrorText));
                }
            }
            catch (Exception ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }

            return resultMessages;
        }
    }
}
