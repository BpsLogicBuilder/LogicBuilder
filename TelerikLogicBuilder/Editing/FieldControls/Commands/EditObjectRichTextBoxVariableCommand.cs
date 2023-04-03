using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditObjectRichTextBoxVariableCommand : ClickCommandBase
    {
        private readonly IEditObjectVariableHelper _editObjectVariableHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectRichTextBoxVariableCommand(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            _editObjectVariableHelper = fieldControlHelperFactory.GetEditObjectVariableHelper(objectRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        public override void Execute()
        {
            if (objectRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{72C8A73A-AECD-4512-854C-FE4C37D5F7E1}");

            XmlElement? childElement = GetChildElement();
            _editObjectVariableHelper.Edit
            (
                objectRichTextBoxValueControl.AssignedTo,
                childElement?.Name == XmlDataConstants.VARIABLEELEMENT
                    ? childElement
                    : null
            );

            XmlElement? GetChildElement()
            {
                if (objectRichTextBoxValueControl.XmlElement == null)
                    return null;

                return _xmlDocumentHelpers.GetSingleOrDefaultChildElement(objectRichTextBoxValueControl.XmlElement);
            }
        }
    }
}
