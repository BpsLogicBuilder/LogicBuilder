﻿using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class ObjectRichTextBoxEventsHelper : IObjectRichTextBoxEventsHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IEditLiteralListHelper _editLiteralListHelper;
        private readonly IEditObjectConstructorHelper _editObjectConstructorHelper;
        private readonly IEditObjectFunctionHelper _editObjectFunctionHelper;
        private readonly IEditObjectListHelper _editObjectListHelper;
        private readonly IEditObjectVariableHelper _editObjectVariableHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IParameterRichTextBoxValueControl parameterRichTextBoxValueControl;

        public ObjectRichTextBoxEventsHelper(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
        {
            _editLiteralListHelper = fieldControlHelperFactory.GetEditLiteralListHelper(parameterRichTextBoxValueControl);
            _editObjectConstructorHelper = fieldControlHelperFactory.GetEditObjectConstructorHelper(parameterRichTextBoxValueControl);
            _editObjectFunctionHelper = fieldControlHelperFactory.GetEditObjectFunctionHelper(parameterRichTextBoxValueControl);
            _editObjectListHelper = fieldControlHelperFactory.GetEditObjectListHelper(parameterRichTextBoxValueControl);
            _editObjectVariableHelper = fieldControlHelperFactory.GetEditObjectVariableHelper(parameterRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.parameterRichTextBoxValueControl = parameterRichTextBoxValueControl;
        }

        private ObjectRichTextBox RichTextBox => parameterRichTextBoxValueControl.RichTextBox;

        public void Setup()
        {
            RichTextBox.MouseClick += RichTextBox_MouseClick;
        }

        private void EditExisting()
        {
            if (parameterRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{8000DE13-3D26-4E0E-818F-E601CDCE5E69}");

            if (parameterRichTextBoxValueControl.XmlElement == null)
                return;

            XmlElement? childElement = _xmlDocumentHelpers.GetSingleOrDefaultChildElement(parameterRichTextBoxValueControl.XmlElement);
            if (childElement == null)
                return;

            switch (childElement.Name)
            {
                case XmlDataConstants.CONSTRUCTORELEMENT:
                    _editObjectConstructorHelper.Edit(parameterRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.FUNCTIONELEMENT:
                    _editObjectFunctionHelper.Edit(parameterRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.VARIABLEELEMENT:
                    _editObjectVariableHelper.Edit(parameterRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.LITERALLISTELEMENT:
                    _editLiteralListHelper.Edit(parameterRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.OBJECTLISTELEMENT:
                    _editObjectListHelper.Edit(parameterRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{3EC201D0-50B8-4EE7-8580-4D8C0CAEC8F2}");
            }
        }

        private void RichTextBox_MouseClick(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            EditExisting();
        }
    }
}
