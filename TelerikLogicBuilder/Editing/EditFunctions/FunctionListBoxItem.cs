using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Configuration;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Intellisense.Functions;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditFunctions
{
    internal class FunctionListBoxItem : IFunctionListBoxItem
    {
        private readonly IAssertFunctionElementValidator _assertFunctionElementValidator;
        private readonly IConfigurationService _configurationService;
        private readonly IFunctionElementValidator _functionElementValidator;
        private readonly IFunctionHelper _functionHelper;
        private readonly IRetractFunctionElementValidator _retractFunctionElementValidator;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly Type assignedTo;
        private readonly IApplicationControl applicationControl;

        private ApplicationTypeInfo Application => applicationControl.Application;

        public FunctionListBoxItem(
            IAssertFunctionElementValidator assertFunctionElementValidator,
            IConfigurationService configurationService,
            IFunctionElementValidator functionElementValidator,
            IFunctionHelper functionHelper,
            IRetractFunctionElementValidator retractFunctionElementValidator,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string visibleText,
            string hiddenText,
            Type assignedTo,
            IApplicationControl applicationControl)
        {
            _assertFunctionElementValidator = assertFunctionElementValidator;
            _configurationService = configurationService;
            _functionElementValidator = functionElementValidator;
            _functionHelper = functionHelper;
            _retractFunctionElementValidator = retractFunctionElementValidator;
            _xmlDocumentHelpers = xmlDocumentHelpers;

            VisibleText = visibleText;
            HiddenText = hiddenText;
            this.assignedTo = assignedTo;
            this.applicationControl = applicationControl;
        }

        public IList<string> Errors
        {
            get
            {
                List<string> errors = new();
                XmlElement functionElement = _xmlDocumentHelpers.ToXmlElement(HiddenText);

                string functionName = functionElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE);
                if (!_configurationService.FunctionList.Functions.TryGetValue(functionName, out Function? function))
                {
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.functionNotConfiguredFormat, functionName));
                    return errors;
                }

                switch (functionElement.Name)
                {
                    case XmlDataConstants.FUNCTIONELEMENT:
                        if (function.FunctionCategory == FunctionCategories.Assert
                            || function.FunctionCategory == FunctionCategories.Retract)
                        {
                            errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.invalidFunctionCategoryFormat, function.Name, Enum.GetName(typeof(FunctionCategories), function.FunctionCategory), functionElement.Name));
                            return errors;
                        }
                        _functionElementValidator.Validate(functionElement, assignedTo, Application, errors);
                        break;
                    case XmlDataConstants.ASSERTFUNCTIONELEMENT:
                        _assertFunctionElementValidator.Validate(functionElement, Application, errors);
                        break;
                    case XmlDataConstants.RETRACTFUNCTIONELEMENT:
                        _retractFunctionElementValidator.Validate(functionElement, Application, errors);
                        break;
                }
                if (errors.Count > 0)
                    return errors;

                if (!_functionHelper.IsVoid(function))
                    errors.Add(string.Format(CultureInfo.CurrentCulture, Strings.returnTypeMustBeVoidFormat, functionName));

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

            return object.ReferenceEquals(this, item);
        }

        public override int GetHashCode()
        {
            return this.VisibleText.GetHashCode();
        }
    }
}
