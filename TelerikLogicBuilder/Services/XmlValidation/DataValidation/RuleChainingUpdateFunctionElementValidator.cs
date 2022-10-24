using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Services.XmlValidation.DataValidation
{
    internal class RuleChainingUpdateFunctionElementValidator : IRuleChainingUpdateFunctionElementValidator
    {
        private readonly IExceptionHelper _exceptionHelper;

        public RuleChainingUpdateFunctionElementValidator(IExceptionHelper exceptionHelper)
        {
            _exceptionHelper = exceptionHelper;
        }

        public void Validate(Function function, IList<XmlElement> parameterElementsList, List<string> validationErrors)
        {
            if (function.FunctionCategory != FunctionCategories.RuleChainingUpdate)
                throw _exceptionHelper.CriticalException("{71221267-4D2F-4E9F-809F-B46734714729}");

            if (function.Parameters.Count != 1)
                throw _exceptionHelper.CriticalException("{A44A1D22-84AC-4382-8E9A-3A5A7CA75444}");

            if (parameterElementsList.Count != 1) throw _exceptionHelper.CriticalException("{EFC886E0-860C-4573-B98A-B680C1168170}");

            if (parameterElementsList[0].ChildNodes.Count != 1 
                || parameterElementsList[0].ChildNodes[0]?.NodeType != XmlNodeType.Text)
            {
                validationErrors.Add(Strings.chainingUpdateValidationError);
            }
        }
    }
}
