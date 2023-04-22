using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditDecisions
{
    internal class DecisionListBoxItem : IDecisionListBoxItem
    {
        private readonly IDecisionElementValidator _decisionElementValidator;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IApplicationControl applicationControl;

        public DecisionListBoxItem(
            IDecisionElementValidator decisionElementValidator,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string visibleText,
            string hiddenText,
            IApplicationControl applicationControl)
        {
            _decisionElementValidator = decisionElementValidator;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            VisibleText = visibleText;
            HiddenText = hiddenText;
            this.applicationControl = applicationControl;
        }

        private ApplicationTypeInfo Application => applicationControl.Application;

        public string HiddenText { get; }

        public string VisibleText { get; }

        public IList<string> Errors
        {
            get
            {
                List<string> errors = new();
                _decisionElementValidator.Validate
                (
                    _xmlDocumentHelpers.ToXmlElement(HiddenText),
                    Application,
                    errors
                );

                return errors;
            }
        }

        public override string ToString() => this.VisibleText;

        public override bool Equals(object? item)
        {
            if (item == null)
                return false;
            if (this.GetType() != item.GetType())
                return false;

            DecisionListBoxItem decisionListBoxItem = (DecisionListBoxItem)item;
            return this.HiddenText == decisionListBoxItem.HiddenText && this.VisibleText == decisionListBoxItem.VisibleText;
        }

        public override int GetHashCode()
        {
            return this.VisibleText.GetHashCode();
        }
    }
}
