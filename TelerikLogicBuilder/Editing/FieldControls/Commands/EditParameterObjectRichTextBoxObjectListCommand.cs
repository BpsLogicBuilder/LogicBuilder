using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditParameterObjectRichTextBoxObjectListCommand : ClickCommandBase
    {
        private readonly IEditParameterObjectListHelper _editParameterObjectListHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IParameterRichTextBoxValueControl parameterRichTextBoxValueControl;

        public EditParameterObjectRichTextBoxObjectListCommand(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
        {
            _editParameterObjectListHelper = fieldControlHelperFactory.GetEditParameterObjectListHelper(parameterRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.parameterRichTextBoxValueControl = parameterRichTextBoxValueControl;
        }

        public override void Execute()
        {
            if (parameterRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{F155C420-6A1C-4188-A629-92111EE64E42}");

            XmlElement? childElement = GetChildElement();
            _editParameterObjectListHelper.Edit
            (
                parameterRichTextBoxValueControl.AssignedTo,
                childElement?.Name == XmlDataConstants.OBJECTLISTELEMENT
                    ? childElement
                    : null
            );

            XmlElement? GetChildElement()
            {
                if (parameterRichTextBoxValueControl.XmlElement == null)
                    return null;

                return _xmlDocumentHelpers.GetSingleOrDefaultChildElement(parameterRichTextBoxValueControl.XmlElement);
            }
        }
    }
}
