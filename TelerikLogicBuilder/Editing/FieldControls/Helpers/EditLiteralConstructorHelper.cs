using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditConstructor;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Enums;
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
    internal class EditLiteralConstructorHelper : IEditLiteralConstructorHelper
    {
        private readonly IConstructorDataParser _constructorDataParser;
        private readonly IConstructorTypeHelper _constructorTypeHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IRichInputBoxValueControl richInputBoxValueControl;

        public EditLiteralConstructorHelper(
            IConstructorDataParser constructorDataParser,
            IConstructorTypeHelper constructorTypeHelper,
            IExceptionHelper exceptionHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IRichInputBoxValueControl richInputBoxValueControl)
        {
            _exceptionHelper = exceptionHelper;
            _constructorDataParser = constructorDataParser;
            _constructorTypeHelper = constructorTypeHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.richInputBoxValueControl = richInputBoxValueControl;
        }

        private ApplicationTypeInfo Application => richInputBoxValueControl.Application;
        private RichInputBox RichInputBox => richInputBoxValueControl.RichInputBox;

        public void Edit(Type assignedTo, XmlElement? constructorElement = null)
        {
            IDictionary<string, Constructor> constructors = _constructorTypeHelper.GetConstructors(assignedTo, Application);
            if (constructors.Count == 0)
            {
                richInputBoxValueControl.SetErrorMessage(string.Format(CultureInfo.CurrentCulture, Strings.constructorNotConfiguredForObjectTypeFormat, assignedTo.ToString()));
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
                true
            );

            editConstructorForm.ShowDialog(RichInputBox);
            if (editConstructorForm.DialogResult != DialogResult.OK)
                return;

            RichInputBox.SelectionProtected = false;
            RichInputBox.InsertLink
            (
                editConstructorForm.VisibleText,
                editConstructorForm.XmlResult.OuterXml,
                LinkType.Constructor
            );

            XmlDocument GetXmlDocument()
            {
                if (constructorElement != null)
                {
                    if (constructorElement.Name != XmlDataConstants.CONSTRUCTORELEMENT)
                        throw _exceptionHelper.CriticalException("{85EE9AD7-D688-4410-A182-B34425ABF048}");

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
