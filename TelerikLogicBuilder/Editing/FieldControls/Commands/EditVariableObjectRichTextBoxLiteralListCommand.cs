using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditVariableObjectRichTextBoxLiteralListCommand : ClickCommandBase
    {
        private readonly IEditVariableLiteralListHelper _editVariableLiteralListHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IVariableRichTextBoxValueControl variableRichTextBoxValueControl;

        public EditVariableObjectRichTextBoxLiteralListCommand(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
        {
            _editVariableLiteralListHelper = fieldControlHelperFactory.GetEditVariableLiteralListHelper(variableRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.variableRichTextBoxValueControl = variableRichTextBoxValueControl;
        }

        public override void Execute()
        {
            if (variableRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{E3DC3F58-BE7F-4A8E-B9BB-B82A937C95B7}");

            XmlElement? childElement = GetChildElement();
            _editVariableLiteralListHelper.Edit
            (
                variableRichTextBoxValueControl.AssignedTo,
                childElement?.Name == XmlDataConstants.LITERALLISTELEMENT
                    ? childElement
                    : null
            );

            XmlElement? GetChildElement()
            {
                if (variableRichTextBoxValueControl.XmlElement == null)
                    return null;

                return _xmlDocumentHelpers.GetSingleOrDefaultChildElement(variableRichTextBoxValueControl.XmlElement);
            }
        }
    }
}
