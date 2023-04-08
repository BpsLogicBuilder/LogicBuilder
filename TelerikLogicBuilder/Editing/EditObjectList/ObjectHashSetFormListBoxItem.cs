using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList
{
    internal class ObjectHashSetFormListBoxItem : IObjectListBoxItem
    {
        private readonly IObjectElementValidator _objectElementValidator;
        private readonly IObjectHashSetListBoxItemComparer _objectHashSetListBoxItemComparer;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public ObjectHashSetFormListBoxItem(
            IObjectElementValidator objectElementValidator,
            IObjectHashSetListBoxItemComparer objectHashSetListBoxItemComparer,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string visibleText,
            string hiddenText,
            Type assignedTo,
            IApplicationControl applicationControl)
        {
            _objectElementValidator = objectElementValidator;
            _objectHashSetListBoxItemComparer = objectHashSetListBoxItemComparer;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            VisibleText = visibleText;
            HiddenText = hiddenText;
            this.assignedTo = assignedTo;
            this.applicationControl = applicationControl;
        }

        private readonly Type assignedTo;
        private readonly IApplicationControl applicationControl;

        public string HiddenText { get; }

        public string VisibleText { get; }

        public IList<string> Errors
        {
            get
            {
                List<string> errors = new();
                _objectElementValidator.Validate
                (
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _xmlDataHelper.BuildObjectXml(HiddenText)
                    ),
                    assignedTo,
                    applicationControl.Application,
                    errors
                );
                return errors;
            }
        }

        public override string ToString() => string.IsNullOrEmpty(this.VisibleText)
            ? Strings.emptyStringVisibleText
            : this.VisibleText;

        public override bool Equals(object? listBoxItem)
        {
            if (listBoxItem == null)
                return false;
            if (this.GetType() != listBoxItem.GetType())
                return false;

            return IsEqualTo((ObjectHashSetFormListBoxItem)listBoxItem);
            bool IsEqualTo(ObjectHashSetFormListBoxItem objectListBoxItem)
            {
                return _objectHashSetListBoxItemComparer.Compare
                (
                    _xmlDocumentHelpers.ToXmlElement(HiddenText),
                    _xmlDocumentHelpers.ToXmlElement(objectListBoxItem.HiddenText),
                    applicationControl.Application
                );
            }
        }

        public override int GetHashCode() => this.VisibleText.GetHashCode();
    }
}
