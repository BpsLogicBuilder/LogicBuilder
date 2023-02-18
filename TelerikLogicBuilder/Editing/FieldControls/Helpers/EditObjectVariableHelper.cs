using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.SelectVariable;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditObjectVariableHelper : IEditObjectVariableHelper
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectVariableHelper(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        private ObjectRichTextBox RichTextBox => objectRichTextBoxValueControl.RichTextBox;

        public void Edit(Type assignedTo, VariableData? variableData = null)
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            ISelectVariableForm selectVariableForm = disposableManager.GetSelectVariableForm(assignedTo);
            if (variableData != null)
                selectVariableForm.SetVariable(variableData.Name);

            selectVariableForm.ShowDialog(RichTextBox);
            if (selectVariableForm.DialogResult != DialogResult.OK)
                return;

            objectRichTextBoxValueControl.UpdateXmlElement(BuildVariableXml(selectVariableForm.VariableName));
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
