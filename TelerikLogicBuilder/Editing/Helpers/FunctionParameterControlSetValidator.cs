using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Parameters;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.Helpers
{
    internal class FunctionParameterControlSetValidator : IFunctionParameterControlSetValidator
    {
        private readonly IBinaryOperatorFunctionElementValidator _binaryOperatorFunctionElementValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IParameterElementValidator _parameterElementValidator;
        private readonly IRuleChainingUpdateFunctionElementValidator _ruleChainingUpdateFunctionElementValidator;

        public FunctionParameterControlSetValidator(
            IBinaryOperatorFunctionElementValidator binaryOperatorFunctionElementValidator,
            IExceptionHelper exceptionHelper,
            IParameterElementValidator parameterElementValidator,
            IRuleChainingUpdateFunctionElementValidator ruleChainingUpdateFunctionElementValidator)
        {
            _binaryOperatorFunctionElementValidator = binaryOperatorFunctionElementValidator;
            _exceptionHelper = exceptionHelper;
            _parameterElementValidator = parameterElementValidator;
            _ruleChainingUpdateFunctionElementValidator = ruleChainingUpdateFunctionElementValidator;
        }

        public void Validate(IDictionary<string, ParameterControlSet> editControlsSet, Function function, ApplicationTypeInfo application, List<string> validationErrors)
        {
            IList<string> requiredFieldErrors = ValidateRequiredFields(editControlsSet, function);
            if(requiredFieldErrors.Count > 0)
            {
                validationErrors.AddRange(requiredFieldErrors);
                return;
            }

            if (function.FunctionCategory == FunctionCategories.BinaryOperator)
            {
                _binaryOperatorFunctionElementValidator.Validate
                (
                    function,
                    function.Parameters
                        .Where(p => editControlsSet[p.Name].ChkInclude.Checked)
                        .Select(p =>
                        {
                            if (editControlsSet[p.Name].ValueControl.XmlElement == null)
                                throw _exceptionHelper.CriticalException("{8A4985B6-4969-4CDE-9A16-3594C2B49487}");

                            return editControlsSet[p.Name].ValueControl.XmlElement!;
                        })
                        .ToArray(),
                    application,
                    validationErrors
                );

                if(validationErrors.Count > 0)//Just highlight both fields
                {
                    function.Parameters.ForEach
                    (
                        p =>
                        {
                            if (editControlsSet[p.Name].ChkInclude.Checked)
                                editControlsSet[p.Name].ValueControl.SetErrorBackColor();
                        }
                    );
                }

                return;
            }

            if (function.FunctionCategory == FunctionCategories.RuleChainingUpdate)
            {
                _ruleChainingUpdateFunctionElementValidator.Validate
                (
                    function,
                    function.Parameters
                        .Where(p => editControlsSet[p.Name].ChkInclude.Checked)
                        .Select(p =>
                        {
                            if (editControlsSet[p.Name].ValueControl.XmlElement == null)
                                throw _exceptionHelper.CriticalException("{DCF62E77-DAE8-4604-918B-E40A01FF2FB0}");

                            return editControlsSet[p.Name].ValueControl.XmlElement!;
                        })
                        .ToArray(),
                    validationErrors
                );

                if (validationErrors.Count > 0)//Just highlight both fields
                {
                    function.Parameters.ForEach
                    (
                        p =>
                        {
                            if (editControlsSet[p.Name].ChkInclude.Checked)
                                editControlsSet[p.Name].ValueControl.SetErrorBackColor();
                        }
                    );
                }

                return;
            }

            foreach (ParameterBase parameter in function.Parameters)
            {
                if (!editControlsSet[parameter.Name].ChkInclude.Checked)
                    continue;

                List<string> parameterErrors = new();
                _parameterElementValidator.Validate
                (
                    editControlsSet[parameter.Name].ValueControl.XmlElement!,
                    parameter,
                    application,
                    parameterErrors
                );

                if (parameterErrors.Count > 0)
                {
                    validationErrors.AddRange(parameterErrors);
                    editControlsSet[parameter.Name].ValueControl.SetErrorBackColor();
                }
                else
                {
                    editControlsSet[parameter.Name].ValueControl.SetNormalBackColor();
                }
            }
        }

        private static IList<string> ValidateRequiredFields(IDictionary<string, ParameterControlSet> editControlsSet, Function function)
        {
            List<string> errors = new();
            foreach (ParameterBase parameter in function.Parameters)
            {
                ParameterControlSet parameterControlSet = editControlsSet[parameter.Name];
                if (parameterControlSet.ChkInclude.Checked && parameterControlSet.ValueControl.IsEmpty)
                {
                    parameterControlSet.ValueControl.SetErrorBackColor();
                    errors.Add
                    (
                        parameter.IsOptional
                            ? string.Format(CultureInfo.CurrentCulture, Strings.checkedOptionalParameterIsEmptyFormat, parameter.Name)
                            : string.Format(CultureInfo.CurrentCulture, Strings.requiredParameterIsEmptyFormat, parameter.Name)
                    );
                }
            }
            return errors;
        }
    }
}
