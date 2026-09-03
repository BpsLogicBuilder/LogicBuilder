using LogicBuilder.RulesDirector;
using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Contoso.Test.Flow.Test
{
    internal static class RuleValidationHelper
    {
        internal static RuleValidation GetValidation(RuleSet ruleSet, Type type)
        {
            if (ruleSet == null)
                throw new InvalidOperationException("RuleSet cannot be null.");

            List<Assembly> referennceAssemblies =
            [
                typeof(LogicBuilder.App.Bsl.Business.Responses.BaseResponse).Assembly,
                typeof(LogicBuilder.App.Spa.Forms.Parameters.CommandButtonParameters).Assembly,
                typeof(LogicBuilder.App.Utils.Interfaces.ITypeHelper).Assembly,
                typeof(LogicBuilder.Forms.Parameters.Expansions.SelectExpandDefinitionParameters).Assembly,
                typeof(DirectorBase).Assembly,
                typeof(string).Assembly
            ];
            RuleValidation ruleValidation = new(type, referennceAssemblies);

            if (!ruleSet.Validate(ruleValidation))
            {
                throw new InvalidOperationException
                (
                    string.Join
                    (
                        Environment.NewLine,
                        ruleValidation.Errors.Aggregate
                        (
                            new List<string>
                            {
                                $"Invalid ruleSet {ruleSet.Name}"
                            },
                            (list, next) =>
                            {
                                list.Add(next.ErrorText);
                                return list;
                            }
                        )
                    )
                );
            }

            return ruleValidation;
        }
    }
}
