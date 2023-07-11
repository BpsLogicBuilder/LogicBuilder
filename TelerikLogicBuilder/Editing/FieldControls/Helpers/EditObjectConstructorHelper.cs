using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Intellisense.Constructors;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.DataParsers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditObjectConstructorHelper : IEditObjectConstructorHelper
    {
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IConstructorTypeHelper _constructorTypeHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;

        private readonly IObjectRichTextBoxValueControl objectRichTextBoxValueControl;

        public EditObjectConstructorHelper(
            IConstructorDataParser constructorDataParser,
            IConstructorTypeHelper constructorTypeHelper,
            IExceptionHelper exceptionHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IObjectRichTextBoxValueControl objectRichTextBoxValueControl)
        {
            _constructorDataParser = constructorDataParser;
            _constructorTypeHelper = constructorTypeHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.objectRichTextBoxValueControl = objectRichTextBoxValueControl;
        }

        private ApplicationTypeInfo Application => objectRichTextBoxValueControl.Application;
        private ObjectRichTextBox RichTextBox => objectRichTextBoxValueControl.RichTextBox;

        public void Edit(Type assignedTo, XmlElement? constructorElement = null)
        {
            IDictionary<string, Constructor> constructors = _constructorTypeHelper.GetConstructors(assignedTo, Application);
            if (constructors.Count == 0)
            {
                objectRichTextBoxValueControl.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredForObjectTypeFormat, assignedTo.ToString()));
                return;
            }

            XmlDocument xmlDocument = GetXmlDocument();
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IEditConstructorForm editConstructorForm = disposableManager.GetEditConstructorForm
            (
                assignedTo,
                xmlDocument,
                constructors.Keys.Order().ToHashSet(),
                _xmlDocumentHelpers
                    .GetDocumentElement(xmlDocument)
                    .Attributes[XmlDataConstants.NAMEATTRIBUTE]!.Value,
                false
            );

            editConstructorForm.ShowDialog(RichTextBox);
            if (editConstructorForm.DialogResult != DialogResult.OK)
                return;

            objectRichTextBoxValueControl.UpdateXmlElement(editConstructorForm.XmlResult.OuterXml);
            RichTextBox.SetLinkFormat();
            RichTextBox.Text = objectRichTextBoxValueControl.VisibleText;
            objectRichTextBoxValueControl.RequestDocumentUpdate();

            XmlDocument GetXmlDocument()
            {
                if (constructorElement != null)
                {
                    if (constructorElement.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                        throw _exceptionHelper.CriticalException("{840E1E29-E8AA-46A7-B875-C144AB4A4EBB}");

                    ConstructorData? constructorData = _constructorDataParser.Parse(constructorElement);
                    if (constructors.ContainsKey(constructorData.Name))
                        return _xmlDocumentHelpers.ToXmlDocument(constructorElement.OuterXml);
                }

                string selectedConstructor = constructors.Keys.Order().First();
                return _xmlDocumentHelpers.ToXmlDocument
                (
                    _xmlDataHelper.BuildEmptyConstructorXml(selectedConstructor, selectedConstructor)
                );
            }
        }
    }
}
