﻿using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using ABIS.LogicBuilder.FlowBuilder.Structures;
using System;
using System.Collections.Generic;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.EditLiteralList
{
    internal class LiteralListFormListBoxItem : ILiteralListBoxItem
    {
        private readonly ILiteralElementValidator _literalElementValidator;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        public LiteralListFormListBoxItem(
            ILiteralElementValidator literalElementValidator,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            string visibleText,
            string hiddenText,
            Type assignedTo,
            IApplicationControl applicationControl)
        {
            _literalElementValidator = literalElementValidator;
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
                _literalElementValidator.Validate
                (
                    _xmlDocumentHelpers.ToXmlElement
                    (
                        _xmlDataHelper.BuildLiteralXml(HiddenText)
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

            LiteralListFormListBoxItem literalListBoxItem = (LiteralListFormListBoxItem)listBoxItem;

            return object.ReferenceEquals(this, literalListBoxItem);
        }

        public override int GetHashCode() => this.VisibleText.GetHashCode();
    }
}
