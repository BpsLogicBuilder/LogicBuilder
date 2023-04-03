using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditObjectRichTextBoxConstructorCommand : ClickCommandBase
    {
        private readonly IEditObjectConstructorHelper _editObjectConstructorHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectRichTextBoxConstructorCommand(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            _editObjectConstructorHelper = fieldControlHelperFactory.GetEditObjectConstructorHelper(objectRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        public override void Execute()
        {
            if (objectRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{DF0A7DAA-6899-490E-8D34-3DC31B58FB09}");

            XmlElement? childElement = GetChildElement();
            _editObjectConstructorHelper.Edit
            (
                objectRichTextBoxValueControl.AssignedTo,
                childElement?.Name == XmlDataConstants.CONSTRUCTORELEMENT
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
