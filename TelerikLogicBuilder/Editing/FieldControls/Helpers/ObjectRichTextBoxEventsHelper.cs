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
        private readonly IEditObjectConstructorHelper _editObjectConstructorHelper;
        private readonly IEditObjectFunctionHelper _editObjectFunctionHelper;
        private readonly IEditObjectVariableHelper _editObjectVariableHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public ObjectRichTextBoxEventsHelper(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            _editObjectConstructorHelper = fieldControlHelperFactory.GetEditObjectConstructorHelper(objectRichTextBoxValueControl);
            _editObjectFunctionHelper = fieldControlHelperFactory.GetEditObjectFunctionHelper(objectRichTextBoxValueControl);
            _editObjectVariableHelper = fieldControlHelperFactory.GetEditObjectVariableHelper(objectRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        private ObjectRichTextBox RichTextBox => objectRichTextBoxValueControl.RichTextBox;

        public void Setup()
        {
            RichTextBox.MouseClick += RichTextBox_MouseClick;
        }

        private void EditExisting()
        {
            if (objectRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{8000DE13-3D26-4E0E-818F-E601CDCE5E69}");

            if (objectRichTextBoxValueControl.XmlElement == null)
                return;

            XmlElement? childElement = _xmlDocumentHelpers.GetSingleOrDefaultChildElement(objectRichTextBoxValueControl.XmlElement);
            if (childElement == null)
                return;

            switch (childElement.Name)
            {
                case XmlDataConstants.CONSTRUCTORELEMENT:
                    _editObjectConstructorHelper.Edit(objectRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.FUNCTIONELEMENT:
                    _editObjectFunctionHelper.Edit(objectRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.VARIABLEELEMENT:
                    _editObjectVariableHelper.Edit(objectRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.LITERALLISTELEMENT:
                    //EditLiteralList();
                    break;
                case XmlDataConstants.OBJECTLISTELEMENT:
                    //EditObjectList();
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
