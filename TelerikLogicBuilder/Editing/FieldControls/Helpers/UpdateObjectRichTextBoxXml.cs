using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.XmlValidation.DataValidation;
using System.Collections.Generic;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class UpdateObjectRichTextBoxXml : IUpdateObjectRichTextBoxXml
    {
        private readonly IConstructorElementValidator _constructorElementValidator;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IFunctionElementValidator _functionElementValidator;
        private readonly ILiteralListElementValidator _literalListElementValidator;
        private readonly IObjectListElementValidator _objectListElementValidator;
        private readonly IVariableElementValidator _variableElementValidator;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public UpdateObjectRichTextBoxXml(
            IConstructorElementValidator constructorElementValidator,
            IExceptionHelper exceptionHelper,
            IFunctionElementValidator functionElementValidator,
            ILiteralListElementValidator literalListElementValidator,
            IObjectListElementValidator objectListElementValidator,
            IVariableElementValidator variableElementValidator,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            _constructorElementValidator = constructorElementValidator;
            _exceptionHelper = exceptionHelper;
            _functionElementValidator = functionElementValidator;
            _literalListElementValidator = literalListElementValidator;
            _objectListElementValidator = objectListElementValidator;
            _variableElementValidator = variableElementValidator;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        private ObjectRichTextBox RichTextBox => objectRichTextBoxValueControl.RichTextBox;

        public void Update(XmlElement? richTextBoxXmlElement)
        {
            if (richTextBoxXmlElement == null)
            {
                objectRichTextBoxValueControl.ResetControl();
                return;
            }

            objectRichTextBoxValueControl.UpdateXmlElement(richTextBoxXmlElement.InnerXml);
            if (objectRichTextBoxValueControl.XmlElement == null)
                throw _exceptionHelper.CriticalException("{FDA68A35-717E-43B5-A58A-A8F8A9A9D53B}");

            XmlElement? childElement = _xmlDocumentHelpers.GetSingleOrDefaultChildElement(objectRichTextBoxValueControl.XmlElement);
            if (childElement == null)
            {
                objectRichTextBoxValueControl.ResetControl();
                return;
            }

            if (ValidateChildElement(childElement))
            {
                RichTextBox.SetLinkFormat();
                RichTextBox.Text = objectRichTextBoxValueControl.VisibleText;
                return;
            }
            else
            {
                objectRichTextBoxValueControl.ResetControl();
                return;
            }
        }

        private bool ValidateChildElement(XmlElement childElement)
        {
            if (objectRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{BE08C328-6FE8-4DFD-A466-F851BCF956A2}");

            List<string> errors = new();
            switch (childElement.Name)
            {
                case XmlDataConstants.CONSTRUCTORELEMENT:
                    _constructorElementValidator.ValidateTypeOnly
                    (
                        childElement,
                        objectRichTextBoxValueControl.AssignedTo,
                        objectRichTextBoxValueControl.Application,
                        errors
                    );
                    break;
                case XmlDataConstants.FUNCTIONELEMENT:
                    _functionElementValidator.ValidateTypeOnly
                    (
                        childElement,
                        objectRichTextBoxValueControl.AssignedTo,
                        objectRichTextBoxValueControl.Application,
                        errors
                    );
                    break;
                case XmlDataConstants.VARIABLEELEMENT:
                    _variableElementValidator.Validate
                    (
                        childElement,
                        objectRichTextBoxValueControl.AssignedTo,
                        objectRichTextBoxValueControl.Application,
                        errors
                    );
                    break;
                case XmlDataConstants.LITERALLISTELEMENT:
                    _literalListElementValidator.ValidateTypeOnly
                    (
                        childElement,
                        objectRichTextBoxValueControl.AssignedTo,
                        objectRichTextBoxValueControl.Application,
                        errors
                    );
                    break;
                case XmlDataConstants.OBJECTLISTELEMENT:
                    _objectListElementValidator.ValidateTypeOnly
                    (
                        childElement,
                        objectRichTextBoxValueControl.AssignedTo,
                        objectRichTextBoxValueControl.Application,
                        errors
                    );
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{3EC201D0-50B8-4EE7-8580-4D8C0CAEC8F2}");
            }

            return errors.Count == 0;
        }
    }
}
