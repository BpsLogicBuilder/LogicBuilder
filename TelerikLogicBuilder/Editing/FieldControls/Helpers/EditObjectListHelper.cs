using ABIS.LogicBuilder.FlowBuilder.Components;
using ABIS.LogicBuilder.FlowBuilder.Constants;
using ABIS.LogicBuilder.FlowBuilder.Data;
using ABIS.LogicBuilder.FlowBuilder.Editing.EditObjectList;
using ABIS.LogicBuilder.FlowBuilder.Editing.Factories;
using ABIS.LogicBuilder.FlowBuilder.Editing.Helpers;
using ABIS.LogicBuilder.FlowBuilder.Reflection;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces;
using ABIS.LogicBuilder.FlowBuilder.ServiceInterfaces.Data;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace ABIS.LogicBuilder.FlowBuilder.Editing.FieldControls.Helpers
{
    internal class EditObjectListHelper : IEditObjectListHelper
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IRefreshVisibleTextHelper _refreshVisibleTextHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IParameterRichTextBoxValueControl parameterRichTextBoxValueControl;

        public EditObjectListHelper(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IRefreshVisibleTextHelper refreshVisibleTextHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IParameterRichTextBoxValueControl parameterRichTextBoxValueControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _refreshVisibleTextHelper = refreshVisibleTextHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.parameterRichTextBoxValueControl = parameterRichTextBoxValueControl;
        }

        private ApplicationTypeInfo Application => parameterRichTextBoxValueControl.Application;
        private ObjectRichTextBox RichTextBox => parameterRichTextBoxValueControl.RichTextBox;

        public void Edit(Type assignedTo, XmlElement? objectListElement = null)
        {
            ObjectListParameterElementInfo objectListElementInfo = parameterRichTextBoxValueControl.ObjectListElementInfo;
            using IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            IEditObjectListForm editObjectListForm = disposableManager.GetEditObjectListForm
            (
                assignedTo,
                objectListElementInfo,
                GetXmlDocument()
            );

            editObjectListForm.ShowDialog(RichTextBox);
            if (editObjectListForm.DialogResult != DialogResult.OK)
                return;

            XmlElement resultElement = _refreshVisibleTextHelper.RefreshObjectListVisibleTexts
            (
                _xmlDocumentHelpers.ToXmlElement
                (
                    _xmlDocumentHelpers.GetDocumentElement(editObjectListForm.XmlDocument).OuterXml
                ),
                Application
            );

            parameterRichTextBoxValueControl.UpdateXmlElement(resultElement.OuterXml);
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
