using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class VariableObjectRichTextBoxEventsHelper : IVariableObjectRichTextBoxEventsHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IEditVariableLiteralListHelper _editVariableLiteralListHelper;
        private readonly IEditObjectConstructorHelper _editObjectConstructorHelper;
        private readonly IEditObjectFunctionHelper _editObjectFunctionHelper;
        private readonly IEditVariableObjectListHelper _editVariableObjectListHelper;
        private readonly IEditObjectVariableHelper _editObjectVariableHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IVariableRichTextBoxValueControl variableRichTextBoxValueControl;

        public VariableObjectRichTextBoxEventsHelper(
            IExceptionHelper exceptionHelper,
            IFieldControlHelperFactory fieldControlHelperFactory,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
        {
            _editVariableLiteralListHelper = fieldControlHelperFactory.GetEditVariableLiteralListHelper(variableRichTextBoxValueControl);
            _editObjectConstructorHelper = fieldControlHelperFactory.GetEditObjectConstructorHelper(variableRichTextBoxValueControl);
            _editObjectFunctionHelper = fieldControlHelperFactory.GetEditObjectFunctionHelper(variableRichTextBoxValueControl);
            _editVariableObjectListHelper = fieldControlHelperFactory.GetEditVariableObjectListHelper(variableRichTextBoxValueControl);
            _editObjectVariableHelper = fieldControlHelperFactory.GetEditObjectVariableHelper(variableRichTextBoxValueControl);
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.variableRichTextBoxValueControl = variableRichTextBoxValueControl;
            RichTextBox.Disposed += RichTextBox_Disposed;
        }

        private IObjectRichTextBox RichTextBox => variableRichTextBoxValueControl.RichTextBox;

        public void Setup()
        {
            RichTextBox.MouseClick += RichTextBox_MouseClick;
        }

        private void EditExisting()
        {
            if (variableRichTextBoxValueControl.AssignedTo == null)
                throw _exceptionHelper.CriticalException("{EAFB3619-3EF7-42C2-B745-A86802964CBB}");

            if (variableRichTextBoxValueControl.XmlElement == null)
                return;

            XmlElement? childElement = _xmlDocumentHelpers.GetSingleOrDefaultChildElement(variableRichTextBoxValueControl.XmlElement);
            if (childElement == null)
                return;

            switch (childElement.Name)
            {
                case XmlDataConstants.CONSTRUCTORELEMENT:
                    _editObjectConstructorHelper.Edit(variableRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.FUNCTIONELEMENT:
                    _editObjectFunctionHelper.Edit(variableRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.VARIABLEELEMENT:
                    _editObjectVariableHelper.Edit(variableRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.LITERALLISTELEMENT:
                    _editVariableLiteralListHelper.Edit(variableRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                case XmlDataConstants.OBJECTLISTELEMENT:
                    _editVariableObjectListHelper.Edit(variableRichTextBoxValueControl.AssignedTo, childElement);
                    break;
                default:
                    throw _exceptionHelper.CriticalException("{C7EBF781-D57A-4495-8B3C-B46863D5A1EF}");
            }
        }

        private void RichTextBox_Disposed(object? sender, System.EventArgs e)
        {
            RichTextBox.MouseClick -= RichTextBox_MouseClick;
        }

        private void RichTextBox_MouseClick(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            EditExisting();
        }
    }
}
