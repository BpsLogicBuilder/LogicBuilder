using ABIS.LogicBuilder.FlowBuilder.Components;
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
    internal class EditVariableObjectListHelper : IEditVariableObjectListHelper
    {
        private readonly IEnumHelper _enumHelper;
        private readonly IExceptionHelper _exceptionHelper;
        private readonly IXmlDataHelper _xmlDataHelper;
        private readonly IXmlDocumentHelpers _xmlDocumentHelpers;
        private readonly IVariableRichTextBoxValueControl variableRichTextBoxValueControl;

        public EditVariableObjectListHelper(
            IEnumHelper enumHelper,
            IExceptionHelper exceptionHelper,
            IXmlDataHelper xmlDataHelper,
            IXmlDocumentHelpers xmlDocumentHelpers,
            IVariableRichTextBoxValueControl variableRichTextBoxValueControl)
        {
            _enumHelper = enumHelper;
            _exceptionHelper = exceptionHelper;
            _xmlDataHelper = xmlDataHelper;
            _xmlDocumentHelpers = xmlDocumentHelpers;
            this.variableRichTextBoxValueControl = variableRichTextBoxValueControl;
        }

        private ObjectRichTextBox RichTextBox => variableRichTextBoxValueControl.RichTextBox;

        public void Edit(Type assignedTo, XmlElement? objectListElement = null)
        {
            ObjectListVariableElementInfo objectListElementInfo = variableRichTextBoxValueControl.ObjectListElementInfo;
            IEditingFormFactory disposableManager = Program.ServiceProvider.GetRequiredService<IEditingFormFactory>();
            using IEditVariableObjectListForm editObjectListForm = disposableManager.GetEditVariableObjectListForm
            (
                assignedTo,
                objectListElementInfo,
                GetXmlDocument()
            );

            editObjectListForm.ShowDialog(RichTextBox);
            if (editObjectListForm.DialogResult != DialogResult.OK)
                return;

            variableRichTextBoxValueControl.UpdateXmlElement(editObjectListForm.XmlResult.OuterXml);
            RichTextBox.SetLinkFormat();
            RichTextBox.Text = variableRichTextBoxValueControl.VisibleText;
            variableRichTextBoxValueControl.RequestDocumentUpdate();

            XmlDocument GetXmlDocument()
            {
                if (objectListElement != null)
                {
                    if (objectListElement.Name != XmlDataConstants.OBJECTLISTELEMENT)
                        throw _exceptionHelper.CriticalException("{901CA9D9-2D27-4B5E-807A-412F1A02D3C7}");

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
                            Strings.listVariableCountFormat,
                            objectListElementInfo.HasVariable
                                ? objectListElementInfo.Variable.Name
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
