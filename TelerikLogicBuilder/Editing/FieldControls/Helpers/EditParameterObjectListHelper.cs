﻿using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditParameterObjectListHelper : IEditParameterObjectListHelper
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IParameterRichTextBoxValueControl parameterRichTextBoxValueControl;

        public EditParameterObjectListHelper(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.parameterRichTextBoxValueControl = parameterRichTextBoxValueControl;
        }

        private IObjectRichTextBox RichTextBox => parameterRichTextBoxValueControl.RichTextBox;

        public void Edit(Type assignedTo, XmlElement? objectListElement = null)
        {
            ObjectListParameterElementInfo objectListElementInfo = parameterRichTextBoxValueControl.ObjectListElementInfo;
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IEditParameterObjectListForm editObjectListForm = disposableManager.GetEditParameterObjectListForm
            (
                assignedTo,
                objectListElementInfo,
                GetXmlDocument()
            );

            editObjectListForm.ShowDialog(RichTextBox);
            if (editObjectListForm.DialogResult != DialogResult.OK)
                return;

            parameterRichTextBoxValueControl.UpdateXmlElement(editObjectListForm.XmlResult.OuterXml);
            RichTextBox.SetLinkFormat();
            RichTextBox.Text = parameterRichTextBoxValueControl.VisibleText;
            parameterRichTextBoxValueControl.RequestDocumentUpdate();

            XmlDocument GetXmlDocument()
            {
                if (objectListElement != null)
                {
                    if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                        throw _exceptionHelper.CriticalException("{CD7F1F13-E777-433C-BC77-4B7268703D14}");

                    return _xmlDocumentHelpers.ToXmlDocument(objectListElement.OuterXml);
                }

                return _xmlDocumentHelpers.ToXmlDocument
                (
                    _xmlDataHelper.BuildObjectListXml
                    (
                        objectListElementInfo.ObjectType,
                        objectListElementInfo.ListType,
                        string.Format
                        (
                            CultureInfo.CurrentCulture,
                            Strings.listParameterCountFormat,
                            objectListElementInfo.HasParameter
                                ? objectListElementInfo.Parameter.Name
                                : _enumHelper.GetTypeDescription(objectListElementInfo.ListType, objectListElementInfo.ObjectType),
                                0
                        ),
                        string.Empty
                    )
                );
            }
        }
    }
}
