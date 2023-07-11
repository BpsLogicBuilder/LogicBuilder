using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditVariable;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Enums;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditLiteralVariableHelper : IEditLiteralVariableHelper
    {
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IVariableDataParser _variableDataParser;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public EditLiteralVariableHelper(
            IExceptionHelper exceptionHelper,
            IVariableDataParser variableDataParser,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _exceptionHelper = exceptionHelper;
            _variableDataParser = variableDataParser;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.richInputBoxValueControl = richInputBoxValueControl;
        }

        private RichInputBox RichInputBox => richInputBoxValueControl.RichInputBox;

        public void Edit(Type assignedTo, XmlElement? variableElement = null)
        {
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IEditVariableForm editVariableForm = disposableManager.GetEditVariableForm(assignedTo);
            if (variableElement != null)
            {
                if (variableElement.Name != XmlDataConstants.VARIABLEELEMENT)
                    throw _exceptionHelper.CriticalException("{0C08C137-DAF9-47FA-BBF1-12B3AD6DCD76}");

                editVariableForm.SetVariable(_variableDataParser.Parse(variableElement).Name);
            }

            editVariableForm.ShowDialog(RichInputBox);
            if (editVariableForm.DialogResult != DialogResult.OK)
                return;

            XmlElement resultElement = BuildXmlElement(editVariableForm.VariableName);
            RichInputBox.SelectionProtected = false;
            RichInputBox.InsertLink
            (
                resultElement.Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                resultElement.OuterXml,
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
