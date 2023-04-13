using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditVariableObjectRichTextBoxObjectListCommand : ClickCommandBase
    {
        private readonly IEditVariableObjectListHelper _editVariableObjectListHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IVariableRichTextBoxValueControl variableRichTextBoxValueControl;

        public EditVariableObjectRichTextBoxObjectListCommand(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
        {
            _editVariableObjectListHelper = fieldControlHelperFactory.GetEditVariableObjectListHelper(variableRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.variableRichTextBoxValueControl = variableRichTextBoxValueControl;
        }

        public override void Execute()
        {
            if (variableRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{86B32E54-AB1E-4BA3-A21E-4322111E6620}");

            XmlElement? childElement = GetChildElement();
            _editVariableObjectListHelper.Edit
            (
                variableRichTextBoxValueControl.AssignedTo,
                childElement?.Name == XmlDataConstants.OBJECTLISTELEMENT
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
