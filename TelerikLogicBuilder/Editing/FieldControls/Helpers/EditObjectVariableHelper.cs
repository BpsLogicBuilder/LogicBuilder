using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditObjectVariableHelper : IEditObjectVariableHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectVariableHelper(
            IExceptionHelper exceptionHelper,
            IVariableDataParser variableDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            _exceptionHelper = exceptionHelper;
            _variableDataParser = variableDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        private IObjectRichTextBox RichTextBox => objectRichTextBoxValueControl.RichTextBox;

        public void Edit(Type assignedTo, XmlElement? variableElement = null)
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IEditVariableForm editVariableForm = disposableManager.GetEditVariableForm(assignedTo);
            if (variableElement != null)
            {
                if (variableElement.Name != XmlDataConstants.VARIABLEELEMENT)
                    throw _exceptionHelper.CriticalException("{F155735E-25EF-4DAF-8DE6-BB5370A7B6A2}");

                editVariableForm.SetVariable(_variableDataParser.Parse(variableElement).Name);
            }

            editVariableForm.ShowDialog(RichTextBox);
            if (editVariableForm.DialogResult != DialogResult.OK)
                return;

            objectRichTextBoxValueControl.UpdateXmlElement(BuildVariableXml(editVariableForm.VariableName));
            RichTextBox.SetLinkFormat();
            RichTextBox.Text = objectRichTextBoxValueControl.VisibleText;
            objectRichTextBoxValueControl.RequestDocumentUpdate();

            string BuildVariableXml(string variableName)
            {
                StringBuilder stringBuilder = new();
                using (XmlWriter xmlTextWriter = _xmlDocumentHelpers.CreateUnformattedXmlWriter(stringBuilder))
                {
                    xmlTextWriter.WriteStartElement(XmlDataConstants.VARIABLEELEMENT);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.NAMEATTRIBUTE, variableName);
                        xmlTextWriter.WriteAttributeString(XmlDataConstants.VISIBLETEXTATTRIBUTE, variableName);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.Flush();
                }

                return stringBuilder.ToString();
            }
        }
    }
}
