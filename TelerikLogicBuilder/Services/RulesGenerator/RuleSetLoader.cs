using ABIS.LogicBuilder.FlowBuilder.Exceptions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.RulesGenerator;
using LogicBuilder.Workflow.Activities.Rules;
using System;
using System.IO;
using System.Text;

namespace ABIS.LogicBuilder.FlowBuilder.Services.RulesGenerator
{
    internal class RuleSetLoader : IRuleSetLoader
    {
        private readonly IRulesAssembler _rulesAssembler;

        public RuleSetLoader(IRulesAssembler rulesAssembler)
        {
            _rulesAssembler = rulesAssembler;
        }

        public RuleSet LoadRuleSet(string fullPath)
        {
            try
            {
                using StreamReader inStream = new(fullPath, Encoding.Unicode);
                return _rulesAssembler.DeserializeRuleSet(inStream.ReadToEnd(), fullPath);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (NotSupportedException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (ArgumentException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (IOException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
            catch (System.Security.SecurityException ex)
            {
                throw new LogicBuilderException(ex.Message, ex);
            }
        }
    }
}
