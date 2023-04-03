using ABIS.LogicBuilder.FlowBuilder.Commands;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Commands
{
    internal class EditObjectRichTextBoxFunctionCommand : ClickCommandBase
    {
        private readonly IEditObjectFunctionHelper _editObjectFunctionHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectRichTextBoxFunctionCommand(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            _editObjectFunctionHelper = fieldControlHelperFactory.GetEditObjectFunctionHelper(objectRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        public override void Execute()
        {
            if (objectRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{4AB49915-B9A3-4753-BA19-FCEA0D3D3E7D}");

            XmlElement? childElement = GetChildElement();
            _editObjectFunctionHelper.Edit
            (
                objectRichTextBoxValueControl.AssignedTo,
                childElement?.Name == XmlDataConstants.FUNCTIONELEMENT
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
