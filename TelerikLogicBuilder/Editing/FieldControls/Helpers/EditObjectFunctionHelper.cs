using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditObjectFunctionHelper : IEditObjectFunctionHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectFunctionHelper(
            IExceptionHelper exceptionHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            _exceptionHelper = exceptionHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        private IObjectRichTextBox RichTextBox => objectRichTextBoxValueControl.RichTextBox;

        public void Edit(Type assignedTo, XmlElement? functionElement = null)
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IDataGraphEditingForm editingForm = assignedTo == typeof(bool)
                                                    ? disposableManager.GetEditBooleanFunctionForm(GetXmlDocument())
                                                    : disposableManager.GetEditValueFunctionForm(assignedTo, GetXmlDocument());

            editingForm.ShowDialog(RichTextBox);
            if (editingForm.DialogResult != DialogResult.OK)
                return;

            objectRichTextBoxValueControl.UpdateXmlElement(editingForm.XmlResult.OuterXml);
            RichTextBox.SetLinkFormat();
            RichTextBox.Text = objectRichTextBoxValueControl.VisibleText;
            objectRichTextBoxValueControl.RequestDocumentUpdate();

            XmlDocument? GetXmlDocument()
            {
                if (functionElement != null)
                {
                    if (functionElement.Name != XmlDataConstants.FUNCTIONELEMENT)
                        throw _exceptionHelper.CriticalException("{EA453DA6-B89A-4B58-A42F-5F548A06EC9C}");

                    return _xmlDocumentHelpers.ToXmlDocument(functionElement.OuterXml);
                }

                return null;
            }
        }
    }
}
