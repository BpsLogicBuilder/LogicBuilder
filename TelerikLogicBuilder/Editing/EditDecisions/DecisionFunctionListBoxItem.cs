using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions
{
    internal class DecisionFunctionListBoxItem : IDecisionFunctionListBoxItem
    {
        private readonly IConfigurationService _configurationService;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionDataParser _functionDataParser;
        private readonly IFunctionElementValidator _functionElementValidator;
        private readonly IFunctionHelper _functionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IApplicationControl applicationControl;

        public DecisionFunctionListBoxItem(
            IConfigurationService configurationService,
            IExceptionHelper exceptionHelper,
            IFunctionDataParser functionDataParser,
            IFunctionElementValidator functionElementValidator,
            IFunctionHelper functionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string visibleText,
            string hiddenText,
            IApplicationControl applicationControl)
        {
            _configurationService = configurationService;
            _exceptionHelper = exceptionHelper;
            _functionDataParser = functionDataParser;
            _functionElementValidator = functionElementValidator;
            _functionHelper = functionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.applicationControl = applicationControl;
            VisibleText = visibleText;
            HiddenText = hiddenText;
        }

        private ApplicationTypeInfo Application => applicationControl.Application;

        public IList<string> Errors
        {
            get
            {
                List<string> errors = new();
                XmlElement functionElement = _xmlDocumentHelpers.ToXmlElement(HiddenText);
                FunctionData functionData = _functionDataParser.Parse(functionElement);

                if (!_configurationService.FunctionList.Functions.TryGetValue(functionData.Name, out Function? function))
                {
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionData.Name));
                    return errors;
                }

                if (!_functionHelper.IsBoolean(function))
                {
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeMustBeBooleanFormat, function.Name));
                    return errors;
                }

                switch (functionElement.Name)
                {
                    case XmlDataConstants.NOTELEMENT:
                    case XmlDataConstants.FUNCTIONELEMENT:
                        _functionElementValidator.Validate(functionElement, typeof(bool), Application, errors);
                        break;
                    default:
                        throw _exceptionHelper.CriticalException("{AE4A8F52-88BE-41D8-B5A5-017218D168A0}");
                }
                return errors;
            }
        }
        public string HiddenText { get; }

        public string VisibleText { get; }

        public override string ToString() => this.VisibleText;

        public override bool Equals(object? item)
        {
            if (item == null)
                return false;
            if (this.GetType() != item.GetType())
                return false;

            DecisionFunctionListBoxItem funcListBoxItem = (DecisionFunctionListBoxItem)item;
            return this.HiddenText == funcListBoxItem.HiddenText && this.VisibleText == funcListBoxItem.VisibleText;
        }

        public override int GetHashCode()
        {
            return this.VisibleText.GetHashCode();
        }
    }
}
