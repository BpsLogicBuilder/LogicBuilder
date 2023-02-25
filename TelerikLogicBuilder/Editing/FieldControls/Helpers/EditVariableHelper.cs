using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditVariableHelper : IEditVariableHelper
    {
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public EditVariableHelper(
            IXmlDocumentHelpers xmlDocumentHelpers,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.richInputBoxValueControl = richInputBoxValueControl;
        }

        private RichInputBox RichInputBox => richInputBoxValueControl.RichInputBox;

        public void Edit(Type assignedTo, VariableData? variableData = null)
        {
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditVariableForm editVariableForm = disposableManager.GetEditVariableForm(assignedTo);
            if (variableData != null)
                editVariableForm.SetVariable(variableData.Name);

            editVariableForm.ShowDialog(RichInputBox);
            if (editVariableForm.DialogResult != DialogResult.OK)
                return;

            XmlElement variableElement = BuildXmlElement(editVariableForm.VariableName);
            RichInputBox.SelectionProtected = false;
            RichInputBox.InsertLink
            (
                variableElement.GetAttribute(XmlDataConstants.NAMEATTRIBUTE),
                variableElement.OuterXml,
                LinkType.Variable
            );


            XmlElement BuildXmlElement(string variableName) 
                => _xmlDocumentHelpers.ToXmlElement(BuildVariableXml(variableName));

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
