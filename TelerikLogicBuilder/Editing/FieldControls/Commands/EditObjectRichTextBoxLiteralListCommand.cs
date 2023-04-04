using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditObjectRichTextBoxLiteralListCommand : ClickCommandBase
    {
        private readonly IEditLiteralListHelper _editLiteralListHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IParameterRichTextBoxValueControl parameterRichTextBoxValueControl;

        public EditObjectRichTextBoxLiteralListCommand(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
        {
            _editLiteralListHelper = fieldControlHelperFactory.GetEditLiteralListHelper(parameterRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.parameterRichTextBoxValueControl = parameterRichTextBoxValueControl;
        }

        public override void Execute()
        {
            if (parameterRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{B526A7AD-3656-408D-9179-CCBC131C2737}");

            XmlElement? childElement = GetChildElement();
            _editLiteralListHelper.Edit
            (
                parameterRichTextBoxValueControl.AssignedTo,
                childElement?.Name == XmlDataConstants.LITERALLISTELEMENT
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
